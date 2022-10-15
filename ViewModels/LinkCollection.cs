using BackupProgram.Models;
using BackupProgram.Services;
using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels.Base;
using BackupProgram.ViewModels.Dialogs;
using BackupProgram.ViewModels.Interfaces;
using BackupProgram.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BackupProgram.ViewModels
{
    public class LinkCollection : BaseViewModel
    {
        public event EventHandler ClosingRequest;

        private ObservableCollection<SourceLinkViewModel> _sourceLinks = new();

        public ObservableCollection<SourceLinkViewModel> SourceLinks
        {
            get => _sourceLinks;
            set => _sourceLinks = value;
        }

        public SourceLinkViewModel? CurrentSelectedSource { get; set; }
        public ILinkViewModel? RecentClickedLink { get; set; }
        public string RecentClickedLinkInfo => RecentClickedLink is null ? string.Empty : RecentClickedLink.ReturnLinkInfo();
        
        private bool _copyInProgress;

        public bool CopyInProgress
        {
            get { return _copyInProgress; }
            set { _copyInProgress = value;}
        }

        public int CopyCompletionPercentage { get; set; }

        private CancellationTokenSource _cancellationToken = new(); 

        #region Commands

        public BaseCommand RemoveSourceLink { get; set; }
        public BaseCommand RemoveDestLink { get; set; }
        public BaseCommand SelectedSourceChanged { get; set; }
        public BaseCommand ShowAddLinkDialog { get; set; }
        public BaseCommand ShowAddDestLinkDialog { get; set; }
        public BaseCommand Copy { get; set; }
        public BaseCommand Save { get; set; }
        public BaseCommand CancelCopy { get; set; }
        public BaseCommand ClickedLink { get; set; }
        public BaseCommand ToggleCurrentLinkIsEnabled { get; set; }

        #endregion

        IDialogService _dialogService;
        CopyService _copyService;
        DeleteService _deleteService;

        public LinkCollection(List<SourceLinkModel> links)
        {
            Progress<CopyProgressModel> progress = new();
            progress.ProgressChanged += UpdateProgress;
            _copyService = new CopyService(progress, _cancellationToken.Token);
            _deleteService = new DeleteService();
            _dialogService = new DialogService();
            RemoveSourceLink = new BaseCommand(RemoveSourceLinkCommand);
            RemoveDestLink = new BaseCommand(RemoveDestLinkCommand);
            SelectedSourceChanged = new BaseCommand(SelectedSourceChangedCommand);
            ShowAddLinkDialog = new BaseCommand(ShowAddLinkDialogCommand);
            ShowAddDestLinkDialog = new BaseCommand(ShowAddDestLinkDialogCommand);
            ClickedLink = new BaseCommand(ClickedLinkCommand);
            Copy = new BaseCommand(CopyCommand, (_) => { return !CopyInProgress; },
                this, nameof(CopyInProgress));
            Save = new BaseCommand(SaveCommand);
            CancelCopy = new BaseCommand(CancelCopyCommand);
            ToggleCurrentLinkIsEnabled = new BaseCommand(ToggleCurrentLinkIsEnabledCommand);

            foreach (var link in links)
            {
                SourceLinks.Add(new SourceLinkViewModel(link));
            }
            
        }

        #region MethodCommands

        private void ShowAddLinkDialogCommand(object? paramater)
        {
            var item = (ListBoxItem)paramater!;
            if (item is null)
            {
                _dialogService.ShowInjectedUserControlDialog<CreateSourceLinkViewModel>(result => { },
                false, null, SourceLinks);
            }
            else
            {
                // Create copy to prevent changes being made to real data until user is finished.
                SourceLinkViewModel originalLink = (SourceLinkViewModel)item.Content;
                originalLink.UpdateModelDestLinks();
                SourceLinkViewModel copy = new SourceLinkViewModel(originalLink.LinkModel);
                _dialogService.ShowInjectedUserControlDialog<CreateSourceLinkViewModel>(result => 
                {
                    if (result)
                    {
                        int replaceIdx = SourceLinks.IndexOf(originalLink);
                        SourceLinks[replaceIdx] = copy;
                    }
                },
                true, copy, null);
            }
        }

        private void ShowAddDestLinkDialogCommand(object? parameter)
        {
            if (CurrentSelectedSource is null) { return; }

            var item = (ListBoxItem)parameter!;
            if (item is null)
            {
                Action<DestLinkViewModel> myAction = (link) =>
                {
                    CurrentSelectedSource.DestLinks.Add(link);
                };
                _dialogService.ShowDialog<MainCreateDestLinkView, MainCreateDestLinkViewModel>(myAction, false);
            }
            else
            {
                // Create copy to prevent changes being made to real data until user is finished.
                DestLinkViewModel originalLink = (DestLinkViewModel)item.Content;
                DestLinkViewModel copy = new DestLinkViewModel(originalLink.LinkModel);
                int replaceIdx = CurrentSelectedSource.DestLinks.IndexOf(originalLink);
                Action<DestLinkViewModel> myAction = (link) =>
                {
                    CurrentSelectedSource.DestLinks[replaceIdx] = link;
                };
                _dialogService.ShowDialog<MainCreateDestLinkView, MainCreateDestLinkViewModel>(myAction, true, copy);
            }
        }

        private void RemoveSourceLinkCommand(object? parameter)
        {
            if (parameter is null) { return; }
            int selectedIndex = (int)parameter;
            if (SourceLinks.Count == 0)
            {
                MessageBox.Show("No more source links.");
                return;
            }
            if (selectedIndex == -1)
            {
                SourceLinks.RemoveAt(SourceLinks.Count - 1);
            }
            else
            {
                SourceLinks.RemoveAt(selectedIndex);
            }
        }

        private void RemoveDestLinkCommand(object? parameter)
        {
            if (parameter is null || CurrentSelectedSource is null) { return; }
            int selectedIndex = (int)parameter;
            if (CurrentSelectedSource.DestLinks.Count == 0)
            {
                MessageBox.Show("No more dest links.");
                return;
            }
            if (selectedIndex == -1)
            {
                CurrentSelectedSource.DestLinks.RemoveAt(CurrentSelectedSource.DestLinks.Count - 1);
            }
            else
            {
                CurrentSelectedSource.DestLinks.RemoveAt(selectedIndex);
            }
        }

        private void SelectedSourceChangedCommand(object? parameter)
        {
            ListBox listBox = (ListBox)parameter!;
            int selectedIndex = listBox.SelectedIndex;
            if (selectedIndex == -1) { CurrentSelectedSource = null; }
            else
            {
                CurrentSelectedSource = (SourceLinkViewModel)listBox.SelectedItem;
            }
        }

        private void ClickedLinkCommand(object? parameter)
        {
            var item = (ListBoxItem)parameter!;
            RecentClickedLink = (ILinkViewModel)item.Content;
        }

        private async void CopyCommand(object? parameter)
        {
            if (SourceLinks.Count() == 0) { MessageBox.Show("No source links."); return; }
            CopyInProgress = true;
            bool cancelled = false;
            try
            {
                await _copyService.CopyAsync(SourceLinks.ToList());
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) 
                { 
                    MessageBox.Show("Copying cancelled."); 
                    cancelled = true;
                    ResetCancelCopyToken();
                }
                else { }
            }
            if (!cancelled)
            {
                MessageBox.Show("Finished copying.");
            }
            _copyService.ResetProgress();
            CopyInProgress = false;
        }

        private void SaveCommand(object? parameter)
        {
            SaveLinks();
        }

        private void CancelCopyCommand(object? parameter)
        {
            CancelCopyInProgress();
        }

        private void ToggleCurrentLinkIsEnabledCommand(object? paremeter)
        {
            if (RecentClickedLink is null) { return; }
            RecentClickedLink.IsEnabled = !RecentClickedLink.IsEnabled;
        }

        #endregion

        #region Methods
        public async Task AutoRun()
        {
            bool cancelled = false;
            CopyInProgress = true;
            try
            {
                await _copyService.AutoCopyAsync(SourceLinks.ToList());
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException) 
                { 
                    MessageBox.Show("Copying cancelled."); 
                    cancelled = true;
                    ResetCancelCopyToken();
                }
            }
            finally { CopyInProgress = false; }

            await _deleteService.DeleteAsync(SourceLinks.ToList());

            SaveLinks();
            if (!cancelled) { OnClosingRequest(); }
         
        }

        private void UpdateProgress(object? sender, CopyProgressModel e)
        {
            CopyCompletionPercentage = e.CompletionPercentage;
        }

        private void OnClosingRequest()
        {
            if (ClosingRequest is not null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }

        private void SaveLinks()
        {
            foreach (var link in SourceLinks) { link.UpdateModelDestLinks(); }
            var linkModels = SourceLinks.Select(x => x.LinkModel).ToList();
            LinkSaveLoadService.SaveLinksJson(linkModels);
        }

        private void CancelCopyInProgress()
        {
            _cancellationToken.Cancel();
        }

        private void ResetCancelCopyToken()
        {
            _cancellationToken.Dispose();
            _cancellationToken = new();
            _copyService.CancellationToken = _cancellationToken.Token;
        }
        #endregion
    }
}