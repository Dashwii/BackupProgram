using BackupProgram.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupProgram.ViewModels.Dialogs
{
    public class SelectDestLinkTypeViewModel : BaseViewModel
    {
        MainCreateDestLinkViewModel _parentViewModel;
        public ICommand NextButton { get; }

        public SelectDestLinkTypeViewModel(params object?[] paramList)
        {
            _parentViewModel = (MainCreateDestLinkViewModel)paramList[0]!;
            NextButton = new BaseCommand(NextButtonCommand);
        }

        public void SetLinkType(int typeIndex)
        {
            switch (typeIndex)
            {
                case 0:
                    _parentViewModel.DestLinkViewModel.DestType = Models.TargetType.LOCAL;
                    break;
                case 1:
                    _parentViewModel.DestLinkViewModel.DestType = Models.TargetType.GOOGLE;
                    break;
                case 2:
                    _parentViewModel.DestLinkViewModel.DestType = Models.TargetType.DROPBOX;
                    break;
                case 3:
                    _parentViewModel.DestLinkViewModel.DestType = Models.TargetType.MEGA;
                    break;
            }
        }

        public void NextButtonCommand(object? parameter)
        {
            SetLinkType((int)parameter!);
            _parentViewModel.SwitchViewModel<CreateDestLinkViewModel>();
        }

    }
}
