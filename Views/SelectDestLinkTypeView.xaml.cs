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
    /// Interaction logic for DestinationLinkType.xaml
    /// </summary>
    public partial class SelectDestLinkTypeView : UserControl
    {
        public SelectDestLinkTypeViewModel ViewModel = default!;

        public SelectDestLinkTypeView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (SelectDestLinkTypeViewModel)DataContext;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int typeIndex = listBox.SelectedIndex;
            ViewModel.NextButtonCommand(typeIndex);
        }

    }
}
