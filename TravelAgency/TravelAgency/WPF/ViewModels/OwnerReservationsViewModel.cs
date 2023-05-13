using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerReservationsViewModel
    {
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationReservationMoveService accommodationReservationMoveService;

        public ObservableCollection<AccommodationReservation> AccommodationReservations { get; set; }
        public ObservableCollection<AccommodationReservationMoveRequestWithAvailabilityDTO> MoveRequests { get; set; }

        public OwnerReservationsViewModel()
        {

            userService = new UserService();
            accommodationReservationService = new AccommodationReservationService();
            accommodationReservationMoveService = new AccommodationReservationMoveService();

            LoggedInUser = userService.GetLoggedInUser();

            AccommodationReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetActiveByOwner(LoggedInUser));
            MoveRequests = new ObservableCollection<AccommodationReservationMoveRequestWithAvailabilityDTO>(accommodationReservationMoveService.GetMoveRequestsWithAvailability(LoggedInUser));
        }
    }
}
