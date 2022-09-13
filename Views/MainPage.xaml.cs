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
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            LinkCollection vm = (LinkCollection)DataContext;
            vm.ShowAddLinkDialog.Execute(sender);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LinkCollection vm = (LinkCollection)DataContext;
            vm.SelectedSourceChanged.Execute(sender);
        }

        private void ListBoxItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            LinkCollection vm = (LinkCollection)DataContext;
            vm.ShowAddDestLinkDialog.Execute(sender);
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            LinkCollection vm = (LinkCollection)DataContext;
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LinkCollection vm = (LinkCollection)DataContext;
            vm.ClickedLink.Execute(sender);
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            LinkCollection vm = (LinkCollection)DataContext;
            vm.ClickedLink.Execute(sender);
        }
    }
}
