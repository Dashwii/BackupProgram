using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupProgram.ViewModels.Base
{
    public interface IDialogWindowViewModel
    {
        void SwitchViewModel<TViewModel>();
        void CloseWindow();
    }
}
