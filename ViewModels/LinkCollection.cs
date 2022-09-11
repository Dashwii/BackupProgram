using BackupProgram.Models;
using BackupProgram.Services;
using BackupProgram.ViewModels.Base;
using BackupProgram.ViewModels.Dialogs;
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
    internal class LinkCollection : ViewModelBase
    {
        private ObservableCollection<SourceLinkViewModel> _sourceLinks = new();

        public ObservableCollection<SourceLinkViewModel> SourceLinks
        {
            get { return _sourceLinks; }
            set { _sourceLinks = value; }
        }

        public SourceLinkViewModel? CurrentViewedLink { get; set; }

        public int SelectedDestIndex { get; set; }

        public ICommand RemoveSourceLink { get; set; }
        public ICommand RemoveDestLink { get; set; }
        public ICommand ChangeSelectedLink { get; set; }
        public ICommand ShowAddLinkDialog { get; set; }
        public ICommand ShowAddDestLinkDialog { get; set; }
        public ICommand Copy { get; set; }
        public ICommand Save { get; set; }

        IDialogService _dialogService;
        ICopyService _copyService;

        public LinkCollection(List<SourceLinkModel> links)
        {
            _copyService = new CopyService(this);
            _dialogService = new DialogService();
            RemoveSourceLink = new BaseCommand(RemoveSourceLinkCommand);
            RemoveDestLink = new BaseCommand(RemoveDestLinkCommand);
            ChangeSelectedLink = new BaseCommand(ChangeSelectedLinkCommand);
            ShowAddLinkDialog = new BaseCommand(ShowAddLinkDialogCommand);
            ShowAddDestLinkDialog = new BaseCommand(ShowAddDestLinkDialogCommand);
            Copy = new BaseCommand(CopyCommand);
            Save = new BaseCommand(SaveCommand);

            foreach (var link in links)
            {
                SourceLinks.Add(new SourceLinkViewModel(link));
            }
        }

        #region Commands

        private void ShowAddLinkDialogCommand(object? paramater)
        {
            if (paramater is null || (int)paramater == -1)
            {
                _dialogService.ShowDialog<AddLinkDialogViewModel>(result =>
                {
                    var test = result;
                },
                this, null, null);
            }
            else
            {
                _dialogService.ShowDialog<AddLinkDialogViewModel>(result =>
                {
                    var test = result;
                },
                this, SourceLinks[(int)paramater], (int)paramater);

                // Set CurrentViewedLink again due to it deselecting when we change our sourceLink object.
                CurrentViewedLink = SourceLinks[(int)paramater];
            }
        }

        private void ShowAddDestLinkDialogCommand(object? parameter)
        {
            // Create dummy destLink to prevent user from seeing edits in real time.
            if (CurrentViewedLink is null) { return; }
            if (parameter is null || (int)parameter == -1)
            { 
                _dialogService.ShowDialog<AddDestLinkDialogViewModel>(result => { },
                    this, CurrentViewedLink.DestLinks, null, null);
            }
            else
            {
                _dialogService.ShowDialog<AddDestLinkDialogViewModel>(result => { },
                this, CurrentViewedLink.DestLinks, CurrentViewedLink.DestLinks[(int)parameter], (int)parameter);
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
            if (parameter is null || CurrentViewedLink is null) { return; }
            int selectedIndex = (int)parameter;
            if (CurrentViewedLink!.DestLinks.Count == 0)
            {
                MessageBox.Show("No more dest links.");
                return;
            }
            if (selectedIndex == -1)
            {
                CurrentViewedLink!.DestLinks.RemoveAt(CurrentViewedLink!.DestLinks.Count - 1);
            }
            else
            {
                CurrentViewedLink!.DestLinks.RemoveAt(selectedIndex);
            }
        }

        private void ChangeSelectedLinkCommand(object? parameter)
        {
            if (parameter is null || (int)parameter == -1)
            {
                CurrentViewedLink = null;
                return;
            }
            SourceLinkViewModel linkVM = SourceLinks[(int)parameter];
            CurrentViewedLink = linkVM;
        }

        private void CopyCommand(object? parameter)
        {
            _copyService.Copy();
        }

        private void SaveCommand(object? parameter)
        {
            var links = SourceLinks.Select(x => x.LinkModel).ToList();
            LinkSaveLoadService.SaveLinksJson(links);
        }

        #endregion 
    }
}
