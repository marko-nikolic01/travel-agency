using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{

    public partial class Guest2NotificationsView : Page
    {
        private Guest2NotificationsViewModel notificationsViewModel;
        public Guest2NotificationsView(int id)
        {
            InitializeComponent();
            notificationsViewModel = new Guest2NotificationsViewModel(id);
            this.DataContext = notificationsViewModel;
        }

        private void ConfirmPresence_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            notificationsViewModel.ConfirmPresence();
        }
        private void RejectPresence_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            notificationsViewModel.RejectPresence();
        }
    }
}
