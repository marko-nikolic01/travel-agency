using System.Windows;
using System.Windows.Controls;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourDetailedView.xaml
    /// </summary>
    public partial class TourDetailedView : Page
    {
        TourDetailedViewModel viewModel;
        public TourDetailedView(TourOccurrence tourOccurrence, int guestId)
        {
            viewModel = new TourDetailedViewModel(tourOccurrence, guestId);
            TourPhotosViewModel photosViewModel = new TourPhotosViewModel(tourOccurrence.Tour.Photos);
            InitializeComponent();
            DataContext = viewModel;
            img.DataContext = photosViewModel;
            btn1.DataContext = photosViewModel;
            btn2.DataContext = photosViewModel;
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            TourReservationView reservationView = new TourReservationView(viewModel.tourOccurrence, viewModel.currentGuestId);
            this.NavigationService.Navigate(reservationView);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OfferedToursView offeredTours = new OfferedToursView(viewModel.currentGuestId, false);
            this.NavigationService.Navigate(offeredTours);
        }
    }
}
