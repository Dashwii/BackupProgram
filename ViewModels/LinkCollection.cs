using BackupProgram.Models;
using BackupProgram.Services;
using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels.Base;
using BackupProgram.ViewModels.Dialogs;
using BackupProgram.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            get { return _sourceLinks; }
            set { _sourceLinks = value; }
        }

        public SourceLinkViewModel? CurrentSelectedSource { get; set; }
        public ILinkViewModel? RecentClickedLink { get; set; }
        public string RecentClickedLinkInfo => RecentClickedLink is null ? string.Empty : RecentClickedLink.ReturnLinkInfo();

        #region Commands

        public ICommand RemoveSourceLink { get; set; }
        public ICommand RemoveDestLink { get; set; }
        public ICommand SelectedSourceChanged { get; set; }
        public ICommand ShowAddLinkDialog { get; set; }
        public ICommand ShowAddDestLinkDialog { get; set; }
        public ICommand Copy { get; set; }
        public ICommand Save { get; set; }
        public ICommand ClickedLink { get; set; }

        #endregion

        IDialogService _dialogService;
        CopyService _copyService;
        DeleteService _deleteService;

        public LinkCollection(List<SourceLinkModel> links)
        {
            _copyService = new CopyService();
            _deleteService = new DeleteService();
            _dialogService = new DialogService();
            RemoveSourceLink = new BaseCommand(RemoveSourceLinkCommand);
            RemoveDestLink = new BaseCommand(RemoveDestLinkCommand);
            SelectedSourceChanged = new BaseCommand(SelectedSourceChangedCommand);
            ShowAddLinkDialog = new BaseCommand(ShowAddLinkDialogCommand);
            ShowAddDestLinkDialog = new BaseCommand(ShowAddDestLinkDialogCommand);
            ClickedLink = new BaseCommand(ClickedLinkCommand);
            Copy = new BaseCommand(CopyCommand);
            Save = new BaseCommand(SaveCommand);

            foreach (var link in links)
            {
                SourceLinks.Add(new SourceLinkViewModel(link));
            }
            
        }

        #region CommandMethods

        private void ShowAddLinkDialogCommand(object? paramater)
        {
            var item = (ListBoxItem)paramater!;
            if (item is null)
            {
                _dialogService.ShowDialog<AddLinkDialogViewModel>(result => { },
                false, null, SourceLinks);
            }
            else
            {
                _dialogService.ShowDialog<AddLinkDialogViewModel>(result => { },
                true, item.Content, null);
            }
        }

        private void ShowAddDestLinkDialogCommand(object? parameter)
        {
            if (CurrentSelectedSource is null) { return; }

            var item = (ListBoxItem)parameter!;
            if (item is null)
            {
                _dialogService.ShowDialog<AddDestLinkDialogViewModel>(result => { },
                    false, null, CurrentSelectedSource.DestLinks);
            }
            else
            {
                // Create copy to prevent changes being made to real data until user is finished.
                DestLinkViewModel originalLink = (DestLinkViewModel)item.Content;
                DestLinkViewModel copy = new DestLinkViewModel(originalLink.LinkModel);
                _dialogService.ShowDialog<AddDestLinkDialogViewModel>(result => 
                { 
                    if (result)
                    {
                        int replaceIdx = CurrentSelectedSource.DestLinks.IndexOf(originalLink);
                        CurrentSelectedSource.DestLinks[replaceIdx] = copy;
                    }
                },
                true, copy, null);
                
                
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

        private void CopyCommand(object? parameter)
        {
            if (SourceLinks.Count() == 0) { MessageBox.Show("No source links."); return; }
            _copyService.Copy(SourceLinks.ToList());
            MessageBox.Show("Finished copying.");
        }

        private void SaveCommand(object? parameter)
        {
            foreach (var link in SourceLinks)
            {
                link.UpdateModelDestLinks();
            }
            var linkModels = SourceLinks.Select(x => x.LinkModel).ToList();
            LinkSaveLoadService.SaveLinksJson(linkModels);
        }


        #endregion

        public void AutoRun()
        {
            _copyService.AutoCopy(SourceLinks.ToList());
            _deleteService.Delete(SourceLinks.ToList());
            OnClosingRequest();
        }

        private void OnClosingRequest()
        {
            if (ClosingRequest is not null)
            {
                ClosingRequest(this, EventArgs.Empty);
            }
        }
    }
}