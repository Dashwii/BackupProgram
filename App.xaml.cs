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
            bool autoRun = false;
            if (e.Args.Length > 0)
            { 
                if (e.Args[0] == "true") { autoRun = true; }
            }

            DialogService.RegisterDialog<CreateSourceLinkViewModel, CreateSourceLinkView>();
            DialogService.RegisterDialog<CreateDestLinkViewModel, CreateDestLinkView>();

            var vm = new LinkCollection(LinkSaveLoadService.LoadLinksJson());
            var d = new MainWindow(vm, autoRun);
            vm.ClosingRequest += (sender, e) => d.Close();
            if (autoRun) { vm.AutoRun().GetAwaiter(); }

        }
    }
}


