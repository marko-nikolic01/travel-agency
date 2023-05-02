using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class GuideMainViewModel
    {
        public NavigationService NavService { get; set; }
        public RelayCommand NavigateToCreateTourPageCommand { get; set; }
        public RelayCommand NavigateToTodaysToursPageCommand { get; set; }
        public RelayCommand NavigateToUpcommingToursPageCommand { get; set; }
        public RelayCommand NavigateToTourRatingsPageCommand { get; set; }
        public RelayCommand NavigateToTourStatisticsPageCommand { get; set; }
        public RelayCommand NavigateToRequestsPageCommand { get; set; }
        public RelayCommand NavigateToRequestStatisticsPageCommand { get; set; }
        public int ActiveGuideId { get; set; }
        public GuideMainViewModel(NavigationService navigationService, int id)
        {
            ActiveGuideId = id;
            NavService = navigationService;
            NavigateToCreateTourPageCommand = new RelayCommand(Execute_NavigateToCreateTourPageCommand, CanExecute_NavigateCommand);
            NavigateToTodaysToursPageCommand = new RelayCommand(Execute_NavigateToTodaysToursPageCommand, CanExecute_NavigateCommand);
            NavigateToUpcommingToursPageCommand = new RelayCommand(Execute_NavigateToUpcommingToursPageCommand, CanExecute_NavigateCommand);
            NavigateToTourRatingsPageCommand = new RelayCommand(Execute_NavigateToTourRatingsPageCommand, CanExecute_NavigateCommand);
            NavigateToTourStatisticsPageCommand = new RelayCommand(Execute_NavigateToTourStatisticsPageCommand, CanExecute_NavigateCommand);
            NavigateToRequestsPageCommand = new RelayCommand(Execute_NavigateToRequestsStatisticsPageCommand, CanExecute_NavigateCommand);
            NavigateToRequestStatisticsPageCommand = new RelayCommand(Execute_NavigateToRequestStatisticsPageCommand, CanExecute_NavigateCommand);
        }
        private void Execute_NavigateToRequestStatisticsPageCommand(object obj)
        {
            Page requests = new TourRequestStatisticsView();
            NavService.Navigate(requests);
        }
        private void Execute_NavigateToRequestsStatisticsPageCommand(object obj)
        {
            Page requests = new TourRequestBookingView(ActiveGuideId);
            NavService.Navigate(requests);
        }
        private void Execute_NavigateToTourStatisticsPageCommand(object obj)
        {
            Page statistics = new TourStatisticsView(ActiveGuideId, NavService);
            NavService.Navigate(statistics);
        }
        private void Execute_NavigateToTourRatingsPageCommand(object obj)
        {
            Page ratings = new TourRatingsPageView(ActiveGuideId, NavService);
            NavService.Navigate(ratings);
        }
        private void Execute_NavigateToUpcommingToursPageCommand(object obj)
        {
            Page tours = new UpcommingToursView(ActiveGuideId, NavService);
            NavService.Navigate(tours);
        }
        private void Execute_NavigateToTodaysToursPageCommand(object obj)
        {
            Page tours = new TodaysToursView(ActiveGuideId);
            NavService.Navigate(tours);
        }
        private void Execute_NavigateToCreateTourPageCommand(object obj)
        {
            Page create = new CreateTourForm(ActiveGuideId, NavService);
            NavService.Navigate(create);
        }
        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }
    }
}
