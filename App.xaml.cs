using BackupProgram.Services;
using BackupProgram.ViewModels;
using BackupProgram.ViewModels.Dialogs;
using BackupProgram.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BackupProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DialogService.RegisterDialog<AddLinkDialogViewModel, AddLink>();
            DialogService.RegisterDialog<AddDestLinkDialogViewModel, AddDestLink>();
            var d = new MainWindow();
            var vm = new LinkCollection(LinkSaveLoadService.LoadLinksJson());
            d.DataContext = vm;
            vm.ClosingRequest += (sender, e) => d.Close();

            if (e.Args.Length > 0)
            {
                if (e.Args[0] == "true") { vm.AutoRun(); }
            }
        }
    }
}
