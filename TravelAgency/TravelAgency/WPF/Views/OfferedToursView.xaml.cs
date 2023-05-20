using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class OfferedToursView : Page
    {
        private OfferedToursViewModel toursViewModel;
        public OfferedToursView(int guestId, bool tourReserved, int selectedTourOccurrenceId = -1)
        {
            if (tourReserved)
                MessageBox.Show("Tour successfully reserved.");
            toursViewModel = new OfferedToursViewModel(guestId, selectedTourOccurrenceId);
            InitializeComponent();
            DataContext = toursViewModel;
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            SearchHelpButton.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            DataGridBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
        }

        private void ReserveTour_Click(object sender, RoutedEventArgs e)
        {
            if(!toursViewModel.CanTourBeReserved())
            {
            }
            else if(toursViewModel.TourIsFull())
            {
                AlternativeToursView alternativeTours = new AlternativeToursView(toursViewModel.SelectedTourOccurrence, toursViewModel.currentGuestId, this.NavigationService);
                alternativeTours.Show();
            }
            else
            {
                TourReservationView reservationView = new TourReservationView(toursViewModel.SelectedTourOccurrence, toursViewModel.currentGuestId);
                this.NavigationService.Navigate(reservationView);
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            toursViewModel.Search();
        }

        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (toursViewModel.SelectedTourOccurrence != null)
            {
                TourDetailedView tourDetailedView = new TourDetailedView(toursViewModel.SelectedTourOccurrence, toursViewModel.currentGuestId);
                this.NavigationService.Navigate(tourDetailedView);
            }
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersView vouchersView = new VouchersView(toursViewModel.currentGuestId);
            this.NavigationService.Navigate(vouchersView);
        }
    }
}
