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
            voucherViewModel = new VoucherViewModel(guestId);
            tourReservationViewModel = new TourReservationViewModel(tourOccurrence, guestId);
            DataContext = tourReservationViewModel;
            vouchersList.DataContext = voucherViewModel;
            occurrence = tourOccurrence;
            id = guestId;
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            VouchersBtn.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            NumGuestsBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
            GuestListBtn.DataContext = toolTipViewModel;
            popup3.DataContext = toolTipViewModel;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reserve \nthis tour?", "Tour reservation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                voucherViewModel.UpdateVoucher(occurrence.Id);
                tourReservationViewModel.SubmitReservation();
                OfferedToursView offeredTours = new OfferedToursView(id, true);
                this.NavigationService.Navigate(offeredTours);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OfferedToursView offeredTours = new OfferedToursView(id, false);
            this.NavigationService.Navigate(offeredTours);
        }
        private void Deselect_Click(object sender, RoutedEventArgs e)
        {
            vouchersList.UnselectAll();
        }
    }
}

