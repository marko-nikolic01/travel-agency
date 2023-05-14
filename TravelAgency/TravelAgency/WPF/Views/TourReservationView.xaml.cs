using System.Windows;
using System.Windows.Controls;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourReservationView.xaml
    /// </summary>
    public partial class TourReservationView : Page
    {

        private TourReservationViewModel tourReservationViewModel;
        private VoucherViewModel voucherViewModel;
        private TourOccurrence occurrence;
        private int id;
        public TourReservationView(TourOccurrence tourOccurrence, int guestId)
        {
            InitializeComponent();
            tourReservationViewModel = new TourReservationViewModel(tourOccurrence, guestId);
            DataContext = tourReservationViewModel;
            voucherViewModel = new VoucherViewModel(guestId);
            vouchersList.DataContext = voucherViewModel;
            occurrence = tourOccurrence;
            id = guestId;
          }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            voucherViewModel.UpdateVoucher(occurrence.Id);
            tourReservationViewModel.SubmitReservation();
            OfferedToursView offeredTours = new OfferedToursView(id);
            this.NavigationService.Navigate(offeredTours);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OfferedToursView offeredTours = new OfferedToursView(id);
            this.NavigationService.Navigate(offeredTours);
        }
        private void Deselect_Click(object sender, RoutedEventArgs e)
        {
            vouchersList.UnselectAll();
        }
    }
}

