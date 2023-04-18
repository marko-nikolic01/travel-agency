﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationReservationRepository
    {
        public void LinkAccommodations(List<Accommodation> accommodations);
        public void LinkGuests(List<User> guests);
        public List<AccommodationReservation> GetAll();
        public void Delete(AccommodationReservation reservation);

        public List<AccommodationReservation> GetAllNotCanceledByGuest(User guest);
        public List<AccommodationReservation> GetFutureNotCanceledByGuest(User guest);
        public int NextId();
        public AccommodationReservation Save(AccommodationReservation reservation);
        public bool IsActive(AccommodationReservation reservation);
        public List<AccommodationReservation> GetByAccommodation(Accommodation accommodation);
        public void UpdateCancelationStatus(AccommodationReservation reservation, bool canceled);
        public void UpdateDateSpan(AccommodationReservation reservation, DateSpan dateSpan);
        public bool CancelReservation(AccommodationReservation reservation);
    }
}
