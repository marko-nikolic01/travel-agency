using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerMainViewModel : ViewModelBase
    {
        public NavigationService NavigationService { get; set; }
        public RelayCommand NavigateToMyProfilePageCommand { get; set; }
        public RelayCommand NavigateToAccommodationsPageCommand { get; set; }
        public RelayCommand NavigateToReservationsPageCommand { get; set; }
        public RelayCommand NavigateToRatingsPageCommand { get; set; }

        public OwnerMainViewModel(NavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToMyProfilePageCommand = new RelayCommand(Execute_NavigateToMyProfilePageCommand, CanExecute_NavigateCommand);
            NavigateToAccommodationsPageCommand = new RelayCommand(Execute_NavigateToAccommodationsPageCommand, CanExecute_NavigateCommand);
            NavigateToReservationsPageCommand = new RelayCommand(Execute_NavigateToReservationsPageCommand, CanExecute_NavigateCommand);
            NavigateToRatingsPageCommand = new RelayCommand(Execute_NavigateToRatingsPageCommand, CanExecute_NavigateCommand);
        }

        private void Execute_NavigateToMyProfilePageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Pages/OwnerProfilePage.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToAccommodationsPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Pages/OwnerAccommodationsPage.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToReservationsPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Pages/OwnerReservationsPage.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToRatingsPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Pages/OwnerRatingsPage.xaml", UriKind.Relative));
        }

        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }
    }
}
