using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerProfileViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationService accommodationService;
        private AccommodationOwnerRatingService accommodationOwnerRatingService;
        private AccommodationGuestRatingService accommodationGuestRatingService;
        private SuperOwnerService superOwnerService;

        public int AccommodationsCount { get; set; }
        public int RatingsCount { get; set; }
        public double AverageRating { get; set; }
        public int RatingsGiven { get; set; }
        public bool IsSuperOwner { get; set; }

        public OwnerProfileViewModel()
        {
            userService = new UserService();
            accommodationService = new AccommodationService();
            accommodationOwnerRatingService = new AccommodationOwnerRatingService();
            superOwnerService = new SuperOwnerService();
            accommodationGuestRatingService = new AccommodationGuestRatingService();

            LoggedInUser = userService.GetLoggedInUser();
            AccommodationsCount = accommodationService.GetAccommodationsCountForOwner(LoggedInUser);
            RatingsCount = accommodationOwnerRatingService.GetRatingsCountForOwner(LoggedInUser);
            AverageRating = superOwnerService.GetAverageRatingForOwner(LoggedInUser);
            RatingsGiven = accommodationGuestRatingService.GetRatingsCountByOwner(LoggedInUser);
            IsSuperOwner = superOwnerService.IsSuperOwner(LoggedInUser);
        }
    }
}
