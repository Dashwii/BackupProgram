using BackupProgram.ViewModels;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private LinkCollection _viewModel = default!;

        public LinkCollection ViewModel
        {
            get => _viewModel;
            private set => _viewModel = value;
        }

        public HomeView()
        {     
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (LinkCollection)DataContext;
        }

        private void SourceListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.ShowAddLinkDialog.Execute(sender);
        }

        private void SourceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedSourceChanged.Execute(sender);
        }

        private void SourceListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.ClickedLink.Execute(sender);
        }

        private void DestinationListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel.ShowAddDestLinkDialog.Execute(sender);
        }

        private void DestinationListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.ClickedLink.Execute(sender);
        }
        
    }
}
