using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAccommodationsStatisticsViewModel : ViewModelBase
    {
        public MyICommand NavigateToMonthStatsCommand { get; set; }

        private UserService userService;
        private AccommodationService accommodationService;
        private AccommodationStatisticsService accommodationStatisticsService;

        private User loggedInUser;

        private NavigationService navigationService;

        public List<Accommodation> Accommodations { get; set; }

        private Accommodation selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get { return selectedAccommodation; }
            set
            {
                selectedAccommodation = value;
                OnPropertyChanged(nameof(SelectedAccommodation));
                UpdateStatistics();
            }
        }

        private AccommodationStatisticsDTO statisticsDTO;
        public AccommodationStatisticsDTO StatisticsDTO
        {
            get { return statisticsDTO; }
            set
            {
                statisticsDTO = value;
                OnPropertyChanged(nameof(StatisticsDTO));
            }
        }
        private AccommodationStatisticsByYearDTO selectedYearStats;

        public AccommodationStatisticsByYearDTO SelectedYearStats
        {
            get { return selectedYearStats; }
            set
            {
                selectedYearStats = value;
                OnPropertyChanged(nameof(SelectedYearStats));
            }
        }


        public OwnerAccommodationsStatisticsViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;

            NavigateToMonthStatsCommand = new MyICommand(Execute_NavigateToMonthStatsCommand);

            userService = new UserService();
            accommodationService = new AccommodationService();
            accommodationStatisticsService = new AccommodationStatisticsService();

            loggedInUser = userService.GetLoggedInUser();

            Accommodations = accommodationService.GetActiveByOwner(loggedInUser);
            
            if (Accommodations.Count > 0 )
            {
                SelectedAccommodation = Accommodations[0];
            }
        }

        private void Execute_NavigateToMonthStatsCommand()
        {
            if (SelectedYearStats != null)
            {
                OwnerAccommodationStatisticsByYearViewModel vm = new OwnerAccommodationStatisticsByYearViewModel(SelectedYearStats);
                OwnerAccommodationStatisticsByYearView page = new OwnerAccommodationStatisticsByYearView(vm);
                this.navigationService.Navigate(page);
            }
            else
            {
                MessageBox.Show("Select a year.");
            }
        }

        private void UpdateStatistics()
        {
            StatisticsDTO = accommodationStatisticsService.GetStatisticsForAccommodation(SelectedAccommodation);
        }
    }
}
