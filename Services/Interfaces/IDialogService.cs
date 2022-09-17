using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BackupProgram.Services.Interfaces
{
    public interface IDialogService
    {
        public void ShowDialog(string name, Action<string> callback, params object?[] paramList);
        void ShowDialog<ViewModel>(Action<string> callback, params object?[] paramList);
    }
}
