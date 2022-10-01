using BackupProgram.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackupProgram.Views
{
    /// <summary>
    /// Interaction logic for AddDestLink.xaml
    /// </summary>
    public partial class AddDestLink : UserControl
    {
        private AddDestLinkDialogViewModel _viewModel = default!;

        public AddDestLinkDialogViewModel ViewModel
        {
            get => _viewModel;
            private set => _viewModel = value;
        }

        public AddDestLink()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (AddDestLinkDialogViewModel)DataContext;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        { 
            if (ViewModel.ConfirmDestLink())
            {
                var window = Window.GetWindow(this);
                window.DialogResult = true;
                CloseWindow(sender, e);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

    }
}
