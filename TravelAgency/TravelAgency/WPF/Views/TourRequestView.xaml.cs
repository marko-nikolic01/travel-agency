using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class TourRequestView : Page
    {
        public TourRequestView(int id, NavigationService navigationService, bool requestMade = false)
        {
            InitializeComponent();
            DataContext = new TourRequestViewModel(id, navigationService);
            if (requestMade)
                MessageBox.Show("The request was made successfully.", "Tour requests", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
