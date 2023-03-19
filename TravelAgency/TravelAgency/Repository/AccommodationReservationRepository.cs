using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationReservationRepository : IRepository<AccommodationReservation>
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;
        private int reservationLength;

        public AccommodationReservationRepository(AccommodationRepository accommodationRepository, UserRepository userRepository)
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            reservationLength = 1;


            foreach (AccommodationReservation accommodationReservation in _accommodationReservations)
            {
                foreach (Accommodation accommodation in accommodationRepository.GetAll())
                {
                    if (accommodationReservation.AccomodationId == accommodation.Id)
                    {
                        accommodationReservation.Accommodation = accommodation;
                    }
                }

                foreach (User user in userRepository.GetUsers())
                {
                    if (accommodationReservation.GuestId == user.Id)
                    {
                        accommodationReservation.Guest = user;
                    }
                }
            }
        }

        public void SetReservationLength(int length)
        {
            reservationLength = length;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }

        public AccommodationReservation GetById(int id)
        {
            foreach (AccommodationReservation accommodationReservation in _accommodationReservations)
            {
                if (accommodationReservation.Id == id)
                {
                    return accommodationReservation; ;
                }
            }
            return null;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public void SaveAll(IEnumerable<AccommodationReservation> entities)
        {
            throw new NotImplementedException();
        }

        public int CalculateDaysLeftForRating(AccommodationReservation accommodationReservation)
        {
            return 5 - DateOnly.Parse(DateTime.Now.Date.ToShortDateString()).DayNumber + accommodationReservation.DateSpan.EndDate.DayNumber;
        }

        public bool ReservationIsActive(AccommodationReservation accommodationReservation)
        {
            return (DateOnly.Parse(DateTime.Now.Date.ToShortDateString()).DayNumber - accommodationReservation.DateSpan.EndDate.DayNumber) < 0;
        }

        public List<AccommodationReservation> GetUnrated(IEnumerable<AccommodationGuestRating> ratings)
        {
            List<AccommodationReservation> unrated = new List<AccommodationReservation>(_accommodationReservations);

            foreach (var accommodationReservation in _accommodationReservations)
            {
                if (CalculateDaysLeftForRating(accommodationReservation) < 1 || ReservationIsActive(accommodationReservation))
                {
                    unrated.Remove(accommodationReservation);
                    continue;
                }

                foreach (var rating in ratings)
                {
                    if (accommodationReservation.Id == rating.AccommodationReservationId)
                    {
                        unrated.Remove(accommodationReservation);
                        break;
                    }
                }
            }

            return unrated;
        }

        public List<AccommodationReservation> GetAllByAccommodationId(int accommodationId)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();

            foreach (AccommodationReservation reservation in _accommodationReservations)
            {
                if (reservation.AccomodationId == accommodationId)
                {
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public List<DateSpan> FindAvailableDatesInsideDateSpan(DateTime firstDate, DateTime lastDate, int accommodationId)
        {
            List<DateSpan> availableDates = new List<DateSpan>();

            DateOnly startDateIterator = DateOnly.FromDateTime(firstDate);
            DateOnly endDateIterator = DateOnly.FromDateTime(firstDate).AddDays(reservationLength - 1);
            DateOnly iterationStopperDate = DateOnly.FromDateTime(lastDate);

            int provera = endDateIterator.CompareTo(iterationStopperDate);
            bool isDateSpanAllowed = endDateIterator.CompareTo(iterationStopperDate) <= 0;
            while (isDateSpanAllowed)
            {
                if (IsDateSpanAvailable(startDateIterator, endDateIterator, accommodationId))
                {
                    DateSpan dateSpan = new DateSpan(startDateIterator, endDateIterator);
                    availableDates.Add(dateSpan);
                }

                startDateIterator = startDateIterator.AddDays(1);
                endDateIterator = endDateIterator.AddDays(1);
                isDateSpanAllowed = endDateIterator.CompareTo(iterationStopperDate) <= 0;
            }

            return availableDates;
        }

        public List<DateSpan> FindAvailableDatesOutsideDateSpan(DateTime firstDate, DateTime lastDate, int accommodationId)
        {
            List<DateSpan> availableDates = new List<DateSpan>();

            DateOnly startDateIterator = DateOnly.FromDateTime(firstDate).AddDays(-1);
            DateOnly endDateIterator = DateOnly.FromDateTime(firstDate).AddDays(reservationLength - 2);
            DateOnly iterationStopperDate = DateOnly.FromDateTime(DateTime.Now);

            bool isDateSpanAllowed = startDateIterator.CompareTo(iterationStopperDate) > 0;
            while (isDateSpanAllowed)
            {
                if (IsDateSpanAvailable(startDateIterator, endDateIterator, accommodationId))
                {
                    DateSpan dateSpan = new DateSpan(startDateIterator, endDateIterator);
                    availableDates.Add(dateSpan);
                    break;
                }

                startDateIterator = startDateIterator.AddDays(-1);
                endDateIterator = endDateIterator.AddDays(-1);
                isDateSpanAllowed = startDateIterator.CompareTo(iterationStopperDate) >= 0;
            }

            startDateIterator = DateOnly.FromDateTime(lastDate).AddDays(-reservationLength + 2);
            endDateIterator = DateOnly.FromDateTime(lastDate).AddDays(1);

            while (true)
            {
                if (IsDateSpanAvailable(startDateIterator, endDateIterator, accommodationId))
                {
                    DateSpan dateSpan = new DateSpan(startDateIterator, endDateIterator);
                    availableDates.Add(dateSpan);
                    break;
                }

                startDateIterator = startDateIterator.AddDays(1);
                endDateIterator = endDateIterator.AddDays(1);
            }

            return availableDates;
        }

        public bool IsDateSpanAvailable(DateOnly startDate, DateOnly endDate, int accommodationId)
        {
            List<AccommodationReservation> reservations = GetAllByAccommodationId(accommodationId);
            DateOnly dateIterator = new DateOnly(startDate.Year, startDate.Month, startDate.Day);

            bool isDateInsideSpan = dateIterator.CompareTo(endDate) <= 0;
            while (isDateInsideSpan)
            {
                foreach (AccommodationReservation reservation in reservations)
                {
                    bool isDateNotAvailable = (dateIterator.CompareTo(reservation.DateSpan.StartDate) >= 0) && (dateIterator.CompareTo(reservation.DateSpan.EndDate) <= 0);
                    if (isDateNotAvailable)
                    {
                        return false;
                    }
                }

                dateIterator = dateIterator.AddDays(1);
                isDateInsideSpan = dateIterator.CompareTo(endDate) <= 0;
            }

            return true;
        }
    }
}
