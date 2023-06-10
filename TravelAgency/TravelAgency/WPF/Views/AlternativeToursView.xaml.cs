using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for AlternativeTours.xaml
    /// </summary>
    public partial class AlternativeToursView : Window
    {
        AlternativeToursViewModel viewModel;
        NavigationService navigationService;
        public AlternativeToursView(TourOccurrence occurrence, int guestId, NavigationService navigationService)
        {
            InitializeComponent();
            viewModel = new AlternativeToursViewModel(occurrence, guestId);
            this.navigationService = navigationService;
            DataContext = viewModel;
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.CanReserve())
            {
                TourReservationView reservationView = new TourReservationView(viewModel.SelectedTourOccurrence, viewModel.CurrentGuestId);
                this.navigationService.Navigate(reservationView);
                Close();
            }
        }
        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTourOccurrence != null)
            {
                TourDetailedView tourDetailedView = new TourDetailedView(viewModel.SelectedTourOccurrence, viewModel.CurrentGuestId);
                this.navigationService.Navigate(tourDetailedView);
                Close();
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
