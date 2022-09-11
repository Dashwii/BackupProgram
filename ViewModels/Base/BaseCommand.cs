using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupProgram.ViewModels.Base
{
    internal class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public Action<object?> _action;

        public BaseCommand(Action<object?> action)
        {
            _action = action;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public virtual void Execute(object? parameter)
        {
            _action(parameter);
        }
    }
}
