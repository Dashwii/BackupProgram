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
    public class CreateDestLinkViewModel : BaseViewModel
    {
        #region Fields
        private MainCreateDestLinkViewModel _parentViewModel;

        public DestLinkViewModel DestLink
        {
            get => _parentViewModel.DestLinkViewModel;
            set => _parentViewModel.DestLinkViewModel = value;
        }

        public string Name => _parentViewModel.DestLinkViewModel.Name;

        public string FilePath
        {
            get => DestLink.FilePath;
            set => DestLink.FilePath = value;
        }

        public bool IsEnabled
        {
            get => DestLink.IsEnabled;
            set => DestLink.IsEnabled = value;
        }

        public TargetType DestType
        {
            get => DestLink.DestType;
            set => DestLink.DestType = value;
        }

        public int AutoCopyFrequency
        {
            get => DestLink.AutoCopyFrequency;
            set => DestLink.AutoCopyFrequency = value;
        }

        public int AutoDeleteFrequency
        {
            get => DestLink.AutoDeleteFrequency; 
            set => DestLink.AutoDeleteFrequency = value;
        }

        public ICommand Back { get; }
        public ICommand Done { get; }

        #endregion

        public CreateDestLinkViewModel(params object[] paramList)
        {
            _parentViewModel = (MainCreateDestLinkViewModel)paramList[0];
            Back = new BaseCommand(BackCommand);
            Done = new BaseCommand(DoneCommand);
        }

        public void DoneCommand(object? parameter)
        {
            if (ConfirmLinkValid())
            {
                _parentViewModel.Finished();
            }
        }

        public bool ConfirmLinkValid()
        {
            switch (DestLink.DestType)
            {
                case TargetType.LOCAL:
                    if (!Directory.Exists(DestLink.FilePath))
                    {
                        MessageBox.Show("Invalid Path");
                        return false;
                    }
                    break;
                case TargetType.GOOGLE:
                    break;
                case TargetType.DROPBOX:
                    break;
                case TargetType.MEGA:
                    break;
            }
            return true;
        }

        public void BackCommand(object? parameter)
        {
            _parentViewModel.SwitchViewModel<SelectDestLinkTypeViewModel>();
        }
        
    }
}
