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
    /// Interaction logic for AddLink.xaml
    /// </summary>
    public partial class CreateSourceLinkView : UserControl
    {
        private CreateSourceLinkViewModel _viewModel = default!;

        public CreateSourceLinkViewModel ViewModel
        {
            get => _viewModel;
            private set => _viewModel = value;
        }

        public CreateSourceLinkView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (CreateSourceLinkViewModel)DataContext;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        private void DestinationsListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AddDestLink.Execute(sender);
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.ConfirmLink())
            {
                var window = Window.GetWindow(this);
                window.DialogResult = true;
                CloseWindow(sender, e);
            }
        }
        
        private void AddDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddDestLink.Execute(null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow(sender, e);
        }

        
    }
}
