using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAccommodationsStatisticsViewModel
    {
        private UserService userService;
        private AccommodationService accommodationService;

        private User loggedInUser;

        public List<Accommodation> Accommodations { get; set; }

        public OwnerAccommodationsStatisticsViewModel()
        {
            userService = new UserService();
            accommodationService = new AccommodationService();

            loggedInUser = userService.GetLoggedInUser();

            Accommodations = accommodationService.GetByOwner(loggedInUser);
        }
    }
}
