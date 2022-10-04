using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupProgram.ViewModels.Base
{
    public class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object?> _action;
        private Predicate<object?> _canExecute = (_) => true;
        private string? _propertyName;

        public BaseCommand(Action<object?> action)
        {
            _action = action;
        }

        public BaseCommand(Action<object?> action, Predicate<object?> canExecute, BaseViewModel VM, string PropertyName)
        {
            _action = action;
            _canExecute = canExecute;
            VM.PropertyChanged += OnCanExecuteChanged;
            _propertyName = PropertyName;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public virtual void Execute(object? parameter)
        {
            _action(parameter);
        }

        protected void OnCanExecuteChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _propertyName)
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
