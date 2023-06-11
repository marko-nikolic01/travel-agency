using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerRatingsViewModel
    {
        private User loggedInUser;
        private UserService userService;
        private AccommodationOwnerRatingService ownerRatingService;
        private AccommodationGuestRatingService guestRatingService;

        private NavigationService navigationService;

        public ObservableCollection<AccommodationOwnerRating> OwnerRatings { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }

        public OwnerRatingsViewModel()
        {
            userService = new UserService();
            ownerRatingService = new AccommodationOwnerRatingService();
            guestRatingService = new AccommodationGuestRatingService();

            loggedInUser = userService.GetLoggedInUser();

            OwnerRatings = new ObservableCollection<AccommodationOwnerRating>(ownerRatingService.GetRatingsVisibleToOwner(loggedInUser));
            Reservations = new ObservableCollection<AccommodationReservation>(guestRatingService.GetUnratedReservationsByOwner(loggedInUser));
        }
    }
}
