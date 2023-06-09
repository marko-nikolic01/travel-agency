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
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerNotifications.xaml
    /// </summary>
    public partial class OwnerNotifications : Page
    {
        public OwnerNotificationsViewModel ViewModel { get; set; }

        public OwnerNotifications()
        {
            InitializeComponent();
            ViewModel = new OwnerNotificationsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void MarkAllAsReadCommand_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MarkAllAsReadCommand.Execute();
        }

        private void MarkAsReadCommand_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MarkAsReadCommand.Execute();
        }
    }
}
