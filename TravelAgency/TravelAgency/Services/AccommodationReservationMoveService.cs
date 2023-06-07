using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Injector;
using TravelAgency.Repositories;
using TravelAgency.Serializer;

namespace TravelAgency.Services
{
    public class AccommodationReservationMoveService
    {
        public IAccommodationReservationMoveRequestRepository MoveRequestRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        private NotificationService _notificationService;

        public AccommodationReservationMoveService()
        {
            MoveRequestRepository = Injector.Injector.CreateInstance<IAccommodationReservationMoveRequestRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            _notificationService = new NotificationService();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetActive());
            MoveRequestRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public List<AccommodationReservationMoveRequest> GetRequestsByGuest(User guest)
        {
            return MoveRequestRepository.GetAllByGuest(guest);
        }

        public bool CreateMoveRequest(AccommodationReservationMoveRequest moveRequest)
        {
            if (moveRequest.IsValid)
            {
                MoveRequestRepository.Save(moveRequest);
                return true;
            }

            return false;
        }

        public void AcceptMoveRequest(AccommodationReservationMoveRequest moveRequest)
        {
            ReservationRepository.UpdateDateSpan(moveRequest.Reservation, moveRequest.DateSpan);
            DeleteOverlappingReservations(moveRequest.Reservation);
            MoveRequestRepository.UpdateStatus(moveRequest, AccommodationReservationMoveRequestStatus.ACCEPTED);
            MoveRequestRepository.UpdateStatusChangedFlag(moveRequest, true);
            _notificationService.NotifyReservationMoveRequestAccepted(moveRequest.Reservation.Guest);
        }

        public void RejectMoveRequest(AccommodationReservationMoveRequest moveRequest)
        {
            MoveRequestRepository.UpdateStatus(moveRequest, AccommodationReservationMoveRequestStatus.REJECTED);
            MoveRequestRepository.UpdateStatusChangedFlag(moveRequest, true);
            _notificationService.NotifyReservationMoveRequestRejected(moveRequest.Reservation.Guest);
        }

        private void DeleteOverlappingReservations(AccommodationReservation reservation)
        {
            var reservations = new List<AccommodationReservation>(ReservationRepository.GetAll());

            foreach (var _reservation in reservations)
            {
                if (_reservation.Id != reservation.Id &&
                    AreDateSpansOverlapping(reservation.DateSpan, _reservation.DateSpan))
                {
                    ReservationRepository.CancelReservation(_reservation);
                }
            }
        }

        private bool AreDateSpansOverlapping(DateSpan dateSpan1, DateSpan dateSpan2)
        {
            return dateSpan1.StartDate.CompareTo(dateSpan2.StartDate) >= 0 && dateSpan1.StartDate.CompareTo(dateSpan2.EndDate) <= 0 ||
                     dateSpan1.EndDate.CompareTo(dateSpan2.StartDate) >= 0 && dateSpan1.EndDate.CompareTo(dateSpan2.EndDate) <= 0 ||
                     dateSpan1.StartDate.CompareTo(dateSpan2.StartDate) <= 0 && dateSpan1.EndDate.CompareTo(dateSpan2.EndDate) >= 0;
        }

        public bool CanReservationBeMoved(AccommodationReservationMoveRequest moveRequest)
        {
            foreach (var reservation in ReservationRepository.GetAll())
            {
                if (reservation.AccommodationId == moveRequest.Reservation.AccommodationId)
                {
                    if (reservation.AccommodationId == moveRequest.Reservation.AccommodationId &&
                        AreDateSpansOverlapping(moveRequest.DateSpan, reservation.DateSpan) &&
                        reservation != moveRequest.Reservation)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<AccommodationReservationMoveRequest> GetWaitingMoveRequestsByOwner(User owner)
        {
            return MoveRequestRepository.GetWaitingByOwner(owner);
        }

        public List<AccommodationReservationMoveRequestWithAvailabilityDTO> GetMoveRequestsWithAvailability(User owner)
        {
            var moveRequests = GetWaitingMoveRequestsByOwner(owner);
            List<AccommodationReservationMoveRequestWithAvailabilityDTO> dtos = new();

            foreach (var moveRequest in moveRequests)
            {
                bool isNewDateSpanAvailable = CanReservationBeMoved(moveRequest);
                AccommodationReservationMoveRequestWithAvailabilityDTO dto = new(moveRequest, isNewDateSpanAvailable);
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}
