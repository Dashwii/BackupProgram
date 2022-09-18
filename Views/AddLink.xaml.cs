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
    public partial class AddLink : UserControl
    {
        public AddLink()
        {
            InitializeComponent();
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Close();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddLinkDialogViewModel vm = (AddLinkDialogViewModel)DataContext;
            vm.AddDestLink.Execute(sender);
        }

        /// <summary>
        /// Done button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddLinkDialogViewModel vm = (AddLinkDialogViewModel)DataContext;
            if (vm.ConfirmLink())
            {
                var window = Window.GetWindow(this);
                window.DialogResult = true;
                closeWindow(sender, e);
            }
        }
        
        /// <summary>
        /// Add destination button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddLinkDialogViewModel vm = (AddLinkDialogViewModel)DataContext;
            vm.AddDestLink.Execute(null);
        }

        /// <summary>
        /// Cancel button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindow(sender, e);
        }

        
    }
}
