using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class SpecialTourRequestsView : Page
    {
        public SpecialTourRequestsView(int guestId, NavigationService navigationService, bool requestMade = false)
        {
            InitializeComponent();
            DataContext = new SpecialTourRequestsViewModel(guestId, navigationService);
            if (requestMade)
                MessageBox.Show("The request was made successfully.", "Special tour request", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
