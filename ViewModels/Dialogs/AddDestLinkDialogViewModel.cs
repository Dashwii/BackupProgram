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
    public class AddDestLinkDialogViewModel : DialogViewModelBase
    {
        private bool _editMode;
        private ObservableCollection<DestLinkViewModel>? _parentList;
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

        /// <summary>
        /// param[0] - editMode,
        /// param[1] - existingLink,
        /// param[2] - parentList
        /// </summary>
        /// <param name="paramList"></param>
        /// <exception cref="ArgumentException"></exception>
        public AddDestLinkDialogViewModel(params object[] paramList)
        {
            if (paramList.Length != 3) { throw new ArgumentException("Parameter Exception."); }
            _editMode = (bool)paramList[0];
            if (!_editMode)
            {
                var l = new DestLinkModel()
                {
                    FilePath = string.Empty,
                    IsEnabled = true,
                    LastAutoCopyDate = DateTime.Now.Date
                };
                _destLink = new DestLinkViewModel(l);
                _parentList = (ObservableCollection<DestLinkViewModel>)paramList[2];
            }
            else
            {
                _destLink = (DestLinkViewModel)paramList[1];
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
            if (!_editMode)
            {
                _parentList!.Add(_destLink);
            }
            return true;
        }
        
    }
}
