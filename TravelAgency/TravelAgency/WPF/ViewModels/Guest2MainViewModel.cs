using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TravelAgency.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest2MainViewModel
    {
        public NavigationService NavService { get; set; }
        public RelayCommand NavigateToOfferedToursCommand { get; set; }
        public RelayCommand NavigateToMyToursCommand { get; set; }


        private void Execute_NavigateToOfferedToursCommand(object obj)
        {
            this.NavService.Navigate(
                new Uri("Wpf/Views/OfferedToursView.xaml", UriKind.Relative));
        }
        private void Execute_NavigateToMyToursCommand(object obj)
        {
            this.NavService.Navigate(
                new Uri("Wpf/Views/MyTours.xaml", UriKind.Relative));
        }
        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }
        public Guest2MainViewModel(NavigationService navService)
        {
            NavService = navService;
            NavigateToOfferedToursCommand = new RelayCommand(Execute_NavigateToOfferedToursCommand, CanExecute_NavigateCommand);
            NavigateToMyToursCommand = new RelayCommand(Execute_NavigateToMyToursCommand, CanExecute_NavigateCommand);
        }
    }
}
