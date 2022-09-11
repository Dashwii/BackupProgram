using BackupProgram.Models;
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
    internal class AddDestLinkDialogViewModel : DialogViewModelBase
    {
        private int? _editIndex = null;
        private object _parentViewModel;
        private ObservableCollection<DestLinkViewModel> _parentDestLinksList;
        private DestLinkViewModel _destLink;
        public DestLinkViewModel DestLink
        {
            get { return _destLink; }
            set { _destLink = value; }
        }

        public string Name => _destLink.Name;

        public string FilePath
        {
            get { return _destLink.FilePath; }
            set { _destLink.FilePath = value; }
        }

        public bool IsEnabled
        {
            get { return _destLink.IsEnabled; }
            set { _destLink.IsEnabled = value; }
        }

        public bool CloudDest
        {
            get { return _destLink.CloudDest; }
            set { _destLink.CloudDest = value; }
        }

        public int AutoCopyFrequency
        {
            get { return _destLink.AutoCopyFrequency; }
            set { _destLink.AutoCopyFrequency = value; }
        }

        public int AutoDeleteFrequency
        {
            get { return _destLink.AutoDeleteFrequency; }
            set { _destLink.AutoDeleteFrequency = value; }
        }

        public AddDestLinkDialogViewModel(params object[] paramList)
        {
            if (paramList.Length != 4) { throw new ArgumentException("Parameter Exception."); }

            if (paramList[0] is LinkCollection)
            {
                _parentViewModel = (LinkCollection)paramList[0];
            }
            else
            {
                _parentViewModel = (AddLinkDialogViewModel)paramList[0];
            }
            _parentDestLinksList = (ObservableCollection<DestLinkViewModel>)paramList[1];
            DestLinkViewModel? existingLink = (DestLinkViewModel?)paramList[2];
            

            if (existingLink is null)
            {
                var l = new DestLinkModel()
                {
                    FilePath = string.Empty,
                    IsEnabled = true,
                };
                _destLink = new(l);
            }
            else
            {
                _editIndex = (int)paramList[3];
                _destLink = existingLink;
            }

            Width = 630;
            Height = 350;
        }

        public bool ConfirmDestLink()
        {
            if (!Directory.Exists(FilePath))
            {
                MessageBox.Show("Invalid Path.");
                return false;
            }
            else
            {
                if (_editIndex is not null)
                {
                    _parentDestLinksList[(int)_editIndex] = _destLink;
                }
                else
                {
                    _parentDestLinksList.Add(_destLink);
                }
                return true;
            }
        }
        
    }
}
