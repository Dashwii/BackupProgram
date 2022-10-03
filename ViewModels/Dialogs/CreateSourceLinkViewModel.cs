using BackupProgram.Models;
using BackupProgram.Services;
using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels.Base;
using BackupProgram.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BackupProgram.ViewModels.Dialogs
{
    public class CreateSourceLinkViewModel : DialogViewModelBase
    {
        #region Fields
        private bool _editMode = false;
        private ObservableCollection<SourceLinkViewModel>? _parentList;
        private SourceLinkViewModel _sourceLink;

        public SourceLinkViewModel SourceLink
        {
            get => _sourceLink;
            set => _sourceLink = value;
        }

        public ObservableCollection<DestLinkViewModel> DestLinks
        {
            get => SourceLink.DestLinks;
            set => SourceLink.DestLinks = value;
        }

        public string Name => _sourceLink.Name;

        public string FilePath
        {
            get => SourceLink.FilePath;
            set => SourceLink.FilePath = value;
        }

        public bool IsEnabled
        {
            get => SourceLink.IsEnabled;
            set => SourceLink.IsEnabled = value;
        }

        public bool AutoCopyEnabled
        {
            get => SourceLink.AutoCopyEnabled;
            set => SourceLink.AutoCopyEnabled = value;
        }

        public bool AutoDeleteEnabled
        {
            get => SourceLink.AutoDeleteEnabled;
            set => SourceLink.AutoDeleteEnabled = value;
        }

        public int SelectedDestIndex { get; set; }

        public ICommand AddDestLink { get; set; }
        private IDialogService _dialogService;

        #endregion

        /// <summary>
        /// param[0] - editMode,
        /// param[1] - existingLink,
        /// param[2] - parentList
        /// </summary>
        /// <param name="paramList"></param>
        /// <exception cref="ArgumentException"></exception>
        public CreateSourceLinkViewModel(params object[] paramList)
        {
            if (paramList.Length != 3) { throw new ArgumentException("Parameter Exception."); }
            _dialogService = new DialogService();
            _editMode = (bool)paramList[0];

            if (!_editMode)
            {
                var l = new SourceLinkModel()
                {
                    FilePath = string.Empty,
                    IsEnabled = true,
                    DestLinks = new()
                };
                _sourceLink = new SourceLinkViewModel(l);
                _parentList = (ObservableCollection<SourceLinkViewModel>)paramList[2];
            }
            else
            {
                _sourceLink = (SourceLinkViewModel)paramList[1];
            }

            AddDestLink = new BaseCommand(ShowAddDestLinkDialogCommand);

            Width = 1420;
            Height = 670;
        }

        public void ShowAddDestLinkDialogCommand(object? parameter)
        {
            var item = (ListBoxItem)parameter!;
            if (parameter is null)
            {
                Action<DestLinkViewModel> myAction = (link) =>
                {
                    DestLinks.Add(link);
                };
                _dialogService.ShowDialog<MainCreateDestLinkView, MainCreateDestLinkViewModel>(myAction, false);
            }
            else
            {
                DestLinkViewModel originalLink = (DestLinkViewModel)item.Content;
                DestLinkViewModel copy = new DestLinkViewModel(originalLink.LinkModel);
                int replaceIdx = DestLinks.IndexOf(originalLink);

                Action<DestLinkViewModel> myAction = (link) =>
                {
                    DestLinks[replaceIdx] = link;
                };
                _dialogService.ShowDialog<MainCreateDestLinkView, MainCreateDestLinkViewModel>(myAction, true, copy);
            }
        }

        public bool ConfirmLink()
        {
            if (!Directory.Exists(FilePath))
            {
                MessageBox.Show("Invalid source path.");
                return false;
            }
            if (DestLinks.Count() <= 0)
            {
                MessageBox.Show("No destination links.");
                return false;
            }
            if (!_editMode)
            {
                _parentList!.Add(_sourceLink);
            }
            return true;
        }
    }
}
