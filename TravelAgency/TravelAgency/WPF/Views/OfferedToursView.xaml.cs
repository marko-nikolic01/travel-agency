using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class OfferedToursView : Page
    {
        private OfferedToursViewModel toursViewModel;
        public OfferedToursView(int guestId)
        {
            toursViewModel = new OfferedToursViewModel(guestId);
            InitializeComponent();
            DataContext = toursViewModel;   
        }

        private void ReserveTour_Click(object sender, RoutedEventArgs e)
        {
            if(toursViewModel.CanTourBeReserved())
            {
                TourReservationView reservationView = new TourReservationView(toursViewModel.SelectedTourOccurrence, toursViewModel.currentGuestId);
                this.NavigationService.Navigate(reservationView);
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            toursViewModel.Search();
        }

        private void ShowPhotos_Click(object sender, RoutedEventArgs e)
        {
            if (toursViewModel.SelectedTourOccurrence != null)
            {
                TourPhotosView tourPhotosView = new TourPhotosView(toursViewModel.SelectedTourOccurrence);
                tourPhotosView.Show();
            }
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersView vouchersView = new VouchersView(toursViewModel.currentGuestId);
            this.NavigationService.Navigate(vouchersView);
        }

        /*   private void AllertIfSelectеd(User activeGuest)
           {
               TourOccurrenceAttendanceService tourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
               TourOccurrenceAttendance attendance;
               if( (attendance = tourOccurrenceAttendanceService.GetAttendance(activeGuest.Id)) != null)
               {
                   if (MessageBox.Show("You have just been selected as present on the tour! Do you confirm?", "Notification", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                   {
                       tourOccurrenceAttendanceService.SaveAnswer(true, attendance);
                   }
                   else
                   {
                       tourOccurrenceAttendanceService.SaveAnswer(false, attendance);
                   }
               }
           }
        */

    }
}
