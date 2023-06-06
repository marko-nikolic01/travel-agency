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
        public RelayCommand NavigateToForumPageCommand { get; set; }

        public OwnerMainViewModel(NavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigateToMyProfilePageCommand = new RelayCommand(Execute_NavigateToMyProfilePageCommand, CanExecute_NavigateCommand);
            NavigateToAccommodationsPageCommand = new RelayCommand(Execute_NavigateToAccommodationsPageCommand, CanExecute_NavigateCommand);
            NavigateToReservationsPageCommand = new RelayCommand(Execute_NavigateToReservationsPageCommand, CanExecute_NavigateCommand);
            NavigateToRatingsPageCommand = new RelayCommand(Execute_NavigateToRatingsPageCommand, CanExecute_NavigateCommand);
            NavigateToForumPageCommand = new RelayCommand(Execute_NavigateToForumPageCommand, CanExecute_NavigateCommand);
        }

        private void Execute_NavigateToMyProfilePageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerProfileView.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToAccommodationsPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToReservationsPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerReservationsView.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToRatingsPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerRatingsView.xaml", UriKind.Relative));
        }

        private void Execute_NavigateToForumPageCommand(object obj)
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerForumView.xaml", UriKind.Relative));
        }

        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }
    }
}
