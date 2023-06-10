using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerReservationsViewModel
    {
        public MyICommand AcceptRequestCommand { get; set; }

        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationReservationMoveService accommodationReservationMoveService;

        public ObservableCollection<AccommodationReservation> AccommodationReservations { get; set; }
        public ObservableCollection<AccommodationReservationMoveRequestWithAvailabilityDTO> MoveRequests { get; set; }
        public AccommodationReservationMoveRequestWithAvailabilityDTO SelectedMoveRequest { get; set; }


        public OwnerReservationsViewModel()
        {
            userService = new UserService();
            accommodationReservationService = new AccommodationReservationService();
            accommodationReservationMoveService = new AccommodationReservationMoveService();

            LoggedInUser = userService.GetLoggedInUser();

            AccommodationReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetActiveByOwner(LoggedInUser));
            MoveRequests = new ObservableCollection<AccommodationReservationMoveRequestWithAvailabilityDTO>(accommodationReservationMoveService.GetMoveRequestsWithAvailability(LoggedInUser));

            AcceptRequestCommand = new MyICommand(Execute_AcceptRequest);
        }

        private void Execute_AcceptRequest()
        {
            if (SelectedMoveRequest == null)
            {
                MessageBox.Show("Select a move request.", "No move request selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            accommodationReservationMoveService.AcceptMoveRequest(SelectedMoveRequest.MoveRequest);
            UpdateMoveRequests();
        }

        private void UpdateMoveRequests()
        {
            MoveRequests.Clear();

            foreach (var moveRequest in accommodationReservationMoveService.GetMoveRequestsWithAvailability(LoggedInUser))
            {
                MoveRequests.Add(moveRequest);
            }
        }
    }
}
