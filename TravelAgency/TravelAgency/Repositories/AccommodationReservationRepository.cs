using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class AccommodationReservationRepository : IAccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
        }

        public void LinkAccommodations(List<Accommodation> accommodations)
        {
            foreach (AccommodationReservation accommodationReservation in _accommodationReservations)
            {
                foreach (Accommodation accommodation in accommodations)
                {
                    if (accommodationReservation.AccommodationId == accommodation.Id)
                    {
                        accommodationReservation.Accommodation = accommodation;
                        break;
                    }
                }
            }
        }

        public void LinkGuests(List<User> guests)
        {
            foreach (AccommodationReservation accommodationReservation in _accommodationReservations)
            {
                foreach (User guest in guests)
                {
                    if (accommodationReservation.GuestId == guest.Id)
                    {
                        accommodationReservation.Guest = guest;
                        break;
                    }
                }
            }
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }

        public List<AccommodationReservation> GetAllNotCanceledByGuest(User guest)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.Guest.Id == guest.Id && !reservation.Canceled)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }

        public List<AccommodationReservation> GetFutureNotCanceledByGuest(User guest)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.Guest.Id == guest.Id && !reservation.Canceled && reservation.DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }

        public int NextId()
        {
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public bool IsActive(AccommodationReservation accommodationReservation)
        {
            return DateOnly.Parse(DateTime.Now.Date.ToShortDateString()).DayNumber - accommodationReservation.DateSpan.EndDate.DayNumber < 0;
        }

        public List<AccommodationReservation> GetByAccommodation(Accommodation accommodation)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.AccommodationId == accommodation.Id)
                {
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public void CancelReservation(AccommodationReservation reservation)
        {
            reservation.Canceled = true;
            UpdateCancelationStatus(reservation, true);
        }

        public void UpdateDateSpan(AccommodationReservation reservation, DateSpan dateSpan)
        {
            reservation.DateSpan = new DateSpan(dateSpan.StartDate, dateSpan.EndDate);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public void UpdateCancelationStatus(AccommodationReservation reservation, bool canceled)
        {
            reservation.Canceled = canceled;
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

        public List<AccommodationReservation> GetByOwner(User owner)
        {
            return _accommodationReservations.FindAll(ar => ar.Accommodation.OwnerId == owner.Id);
        }

        public List<AccommodationReservation> GetActiveByOwner(User owner)
        {
            var reservations = GetByOwner(owner);
            List<AccommodationReservation> activeReservations = new List<AccommodationReservation>();

            foreach (var reservation in reservations)
            {
                if (IsActive(reservation))
                {
                    activeReservations.Add(reservation);
                }
            }

            return activeReservations;
        }
    }
}
