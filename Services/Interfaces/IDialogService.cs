using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BackupProgram.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowDialog<TWindowType, TViewModelType>(params object?[] paramList);

        void ShowInjectedUserControlDialog<ViewModel>(Action<bool> callback, params object?[] paramList);
        void ShowInjectedUserControlDialog<ViewModel>(params object?[] paramList);

    }
}
