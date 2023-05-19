using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAccommodationsStatisticsViewModel : ViewModelBase
    {
        private UserService userService;
        private AccommodationService accommodationService;
        private AccommodationStatisticsService accommodationStatisticsService;

        private User loggedInUser;

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

        public OwnerAccommodationsStatisticsViewModel()
        {
            userService = new UserService();
            accommodationService = new AccommodationService();
            accommodationStatisticsService = new AccommodationStatisticsService();

            loggedInUser = userService.GetLoggedInUser();

            Accommodations = accommodationService.GetByOwner(loggedInUser);
            
            if (Accommodations.Count > 0 )
            {
                SelectedAccommodation = Accommodations[0];
            }
        }

        private void UpdateStatistics()
        {
            StatisticsDTO = accommodationStatisticsService.GetStatisticsForAccommodation(SelectedAccommodation);
        }
    }
}
