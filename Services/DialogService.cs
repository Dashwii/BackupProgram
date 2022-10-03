using BackupProgram.Services.Interfaces;
using BackupProgram.ViewModels.Base;
using BackupProgram.ViewModels.Dialogs;
using BackupProgram.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BackupProgram.Services
{
    public class DialogService : IDialogService
    {
        private static Dictionary<Type, Type> _mapping = new();

        public static void RegisterDialog<TviewModel, Tview>()
        {
            _mapping.Add(typeof(TviewModel), typeof(Tview));
        }

        public void ShowDialog<TWindowType, TViewModelType>(params object?[] paramList)
        {
            var windowType = typeof(TWindowType);
            var viewModelType = typeof(TViewModelType);
            ShowDialogInternal(windowType, viewModelType, paramList);
        }

        private void ShowDialogInternal(Type WindowType, Type viewModelType, params object?[] paramList)
        {
            var window = (Window)Activator.CreateInstance(WindowType)!;
            var vm = (BaseViewModel)Activator.CreateInstance(viewModelType, window, paramList)!;
            window.DataContext = vm;
            window.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
            window.ShowDialog();

        }

        public void ShowInjectedUserControlDialog<TViewModel>(Action<bool> callback, params object?[] paramList)
        {
            var type = _mapping[typeof(TViewModel)];
            ShowInjectedUserControlDialogInternal(type, callback, typeof(TViewModel), paramList);
        }

        public void ShowInjectedUserControlDialog<TViewModel>(params object?[] paramList)
        {
            var type = _mapping[typeof(TViewModel)];
            ShowInjectedUserControlDialogInternal(type, null, typeof(TViewModel), paramList);
        }

        private void ShowInjectedUserControlDialogInternal(Type type, Action<bool>? callback, Type? vmType, params object?[] paramList)
        {
            var dialog = new DialogWindow();

            if (callback is not null)
            {
                EventHandler closeEventHandler = null!;
                closeEventHandler = (s, e) =>
                {
                    callback((bool)dialog.DialogResult!);
                    dialog.Closed -= closeEventHandler;
                };
                dialog.Closed += closeEventHandler;
            }

            dialog.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

            var content = Activator.CreateInstance(type);
            if (vmType is not null)
            {
                var vm = Activator.CreateInstance(vmType, paramList);
                (content as FrameworkElement).DataContext = vm;
                dialog.Width = (vm as DialogViewModelBase).Width;
                dialog.Height = (vm as DialogViewModelBase).Height;
            }
            
            dialog.Content = content;

            dialog.ShowDialog();
        }
    }
}
