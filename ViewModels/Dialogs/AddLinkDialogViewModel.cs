using BackupProgram.Models;
using BackupProgram.Services;
using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BackupProgram.ViewModels.Dialogs
{
    internal class AddLinkDialogViewModel : DialogViewModelBase
    {
        #region Fields
        private int? _editIndex = null;
        private LinkCollection _parent;
        private SourceLinkViewModel _sourceLink;

        public SourceLinkViewModel SourceLink
        {
            get { return _sourceLink; }
            set { _sourceLink = value; }
        }

        public ObservableCollection<DestLinkViewModel> DestLinks
        {
            get => _sourceLink.DestLinks;
            set { _sourceLink.DestLinks = value; }
        }

        public string Name => _sourceLink.Name;

        public string FilePath
        {
            get { return _sourceLink.FilePath; }
            set { _sourceLink.FilePath = value; }
        }

        public bool IsEnabled
        {
            get { return _sourceLink.IsEnabled; }
            set { _sourceLink.IsEnabled = value; }
        }

        public bool AutoCopyEnabled
        {
            get { return _sourceLink.AutoCopyEnabled; }
            set { _sourceLink.AutoCopyEnabled = value; }
        }

        public bool AutoDeleteEnabled
        {
            get { return _sourceLink.AutoDeleteEnabled; }
            set { _sourceLink.AutoDeleteEnabled = value; }
        }

        public int SelectedDestIndex { get; set; }

        public ICommand AddDestLink { get; set; }
        private IDialogService _dialogService;

        #endregion

        public AddLinkDialogViewModel(params object[] paramList)
        {
            if (paramList.Length != 3) { throw new ArgumentException("Parameter Exception."); }
            _dialogService = new DialogService();

            _parent = (LinkCollection)paramList[0];
            SourceLinkViewModel? existingLink = (SourceLinkViewModel?)paramList[1];

            if (existingLink is null)
            {
                var l = new SourceLinkModel()
                {
                    FilePath = string.Empty,
                    IsEnabled = true,
                    DestLinks = new()
                };
                _sourceLink = new(l);
            }
            else
            {
                _editIndex = (int)paramList[2];
                _sourceLink = existingLink;
            }

            AddDestLink = new BaseCommand(ShowAddDestLinkDialogCommand);

            Width = 1420;
            Height = 670;
        }

        public void ShowAddDestLinkDialogCommand(object? parameter)
        {
            if (parameter is null)
            {
                _dialogService.ShowDialog<AddDestLinkDialogViewModel>(result =>
                {
                    var test = result;
                },
                this, null, null
                );
            }
            else
            {
                // Create dummy destLink to prevent changes being reflected while user is editing.
                var requestedLink = DestLinks[(int)parameter];
                var d = new DestLinkViewModel(new DestLinkModel()
                {
                    FilePath = requestedLink.FilePath,
                    IsEnabled = requestedLink.IsEnabled,
                    CloudDest = requestedLink.CloudDest,
                    AutoCopyFrequency = requestedLink.AutoCopyFrequency,
                    AutoDeleteFrequency = requestedLink.AutoDeleteFrequency
                });

                _dialogService.ShowDialog<AddDestLinkDialogViewModel>(result =>
                {
                    var test = result;
                },
                this, d, (int)parameter);
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
            if (_editIndex is not null)
            {
                _parent.SourceLinks[(int)_editIndex] = _sourceLink;
            }
            else
            {
                _parent.SourceLinks.Add(_sourceLink);
            }
            return true;
        }
    }
}
