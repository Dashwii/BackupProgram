using BackupProgram.Services.Interfaces;
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

        public void ShowDialog<TViewModel>(Action<string> callback, params object?[] paramList)
        {
            var type = _mapping[typeof(TViewModel)];
            ShowDialogInternal(type, callback, typeof(TViewModel), paramList);
        }

        public void ShowDialog(string name, Action<string> callback, params object?[] paramList)
        {
            var type = Type.GetType($"BackupProgram.Views.{name}");
            if (type is null) { return; }
            ShowDialogInternal(type, callback, null, paramList);
        }

        public void ShowDialogInternal(Type type, Action<string> callback, Type? vmType, params object?[] paramList)
        {
            var dialog = new DialogWindow();

            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callback(dialog.DialogResult.ToString());
                dialog.Closed -= closeEventHandler;
            };

            dialog.Closed += closeEventHandler;

            dialog.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);

            if (type == null) { throw new ArgumentException("Invalid UserRefernce."); }

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
