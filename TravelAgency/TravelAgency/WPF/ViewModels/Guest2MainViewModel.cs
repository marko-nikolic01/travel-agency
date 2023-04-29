using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest2MainViewModel
    {
        public NavigationService NavService { get; set; }
        public RelayCommand NavigateToOfferedToursCommand { get; set; }
        public RelayCommand NavigateToMyToursCommand { get; set; }
        public RelayCommand NavigateToRequestsCommand { get; set; }
        public RelayCommand NavigateToProfileCommand { get; set; }
        private int currentGuestId;
        public Guest2MainViewModel(NavigationService navService, int guestId)
        {
            NavService = navService;
            NavigateToOfferedToursCommand = new RelayCommand(Execute_NavigateToOfferedToursCommand, CanExecute_NavigateCommand);
            NavigateToMyToursCommand = new RelayCommand(Execute_NavigateToMyToursCommand, CanExecute_NavigateCommand);
            NavigateToRequestsCommand = new RelayCommand(Execute_NavigateToRequestsCommand, CanExecute_NavigateCommand);
            NavigateToProfileCommand = new RelayCommand(Execute_NavigateToProfileCommand, CanExecute_NavigateCommand);
            currentGuestId = guestId;
        }
        private void Execute_NavigateToOfferedToursCommand(object obj)
        {
            Page OfferedTours = new OfferedToursView(currentGuestId);
            NavService.Navigate(OfferedTours);
        }
        private void Execute_NavigateToMyToursCommand(object obj)
        {
            Page myTours = new MyTours(currentGuestId);
            NavService.Navigate(myTours);
        }
        private void Execute_NavigateToRequestsCommand(object obj)
        {
            Page requests = new TourRequestView(currentGuestId);
            NavService.Navigate(requests);
        }
        private void Execute_NavigateToProfileCommand(object obj)
        {
            Page requests = new Guest2ProfileView();
            NavService.Navigate(requests);
        }
        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }
    }
}
