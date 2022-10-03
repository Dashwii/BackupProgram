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
    public partial class CreateDestLinkView : UserControl
    {
        private CreateDestLinkViewModel _viewModel = default!;

        public CreateDestLinkViewModel ViewModel
        {
            get => _viewModel;
            private set => _viewModel = value;
        }

        public CreateDestLinkView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (CreateDestLinkViewModel)DataContext;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

    }
}
