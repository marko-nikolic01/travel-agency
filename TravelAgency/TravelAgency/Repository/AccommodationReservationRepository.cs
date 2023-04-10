using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private int _reservationLength;
        private DateOnly _startDateIterator;
        private DateOnly _endDateIterator;
        private DateOnly _iterationStopperDate;

        public AccommodationReservationRepository(AccommodationRepository accommodationRepository, UserRepository userRepository)
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _reservationLength = 1;


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
            _reservationLength = length;
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
            List<AccommodationReservation> unrated = new();

            foreach (var accommodationReservation in _accommodationReservations)
            {
                if (IsValidForRating(accommodationReservation, ratings))
                {
                    unrated.Add(accommodationReservation);
                    continue;
                }
            }

            return unrated;
        }

        private bool IsValidForRating(AccommodationReservation accommodationReservation, IEnumerable<AccommodationGuestRating> accommodationGuestRatings)
        {
            return CalculateDaysLeftForRating(accommodationReservation) >= 1 &&
                !ReservationIsActive(accommodationReservation) &&
                !IsRated(accommodationReservation, accommodationGuestRatings);
        }

        private bool IsRated(AccommodationReservation accommodationReservation, IEnumerable<AccommodationGuestRating> accommodationGuestRatings)
        {
            foreach (var accommodationGuestRating in accommodationGuestRatings)
            {
                if (accommodationReservation.Id == accommodationGuestRating.AccommodationReservationId)
                {
                    return true;
                }
            }

            return false;
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

        public List<DateSpan> FindAvailableDatesInsideDateRange(DateTime dateRangeStart, DateTime dateRangeEnd, int accommodationId)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            PrepareDateIterators(dateRangeStart, dateRangeStart.AddDays(_reservationLength - 1), dateRangeEnd);
            bool isDateSpanAllowed = _endDateIterator.CompareTo(_iterationStopperDate) <= 0;
            while (isDateSpanAllowed)
            {
                if (IsDateSpanAvailable(accommodationId))
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(1);
                isDateSpanAllowed = _endDateIterator.CompareTo(_iterationStopperDate) <= 0;
            }
            return availableDates;
        }

        public List<DateSpan> FindAvailableDatesOutsideDateRange(DateTime dateRangeStart, DateTime dateRangeEnd, int accommodationId)
        {
            List<DateSpan> availableDatesBefore = FindAvailableDatesBeforeDateRange(dateRangeStart, accommodationId);
            availableDatesBefore.Reverse();
            List<DateSpan> availableDatesAfter = FindAvailableDatesAfterDateRange(dateRangeEnd, accommodationId);
            List<DateSpan> availableDates = availableDatesBefore.Concat(availableDatesAfter).ToList();
            return availableDates;
        }

        private List<DateSpan> FindAvailableDatesBeforeDateRange(DateTime dateRangeStart, int accommodationId)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            PrepareDateIterators(dateRangeStart.AddDays(-1), dateRangeStart.AddDays(_reservationLength - 2), DateTime.Now.AddDays(1));
            bool isDateSpanAllowed = _startDateIterator.CompareTo(_iterationStopperDate) > 0;
            while (isDateSpanAllowed && availableDates.Count() < 3)
            {
                if (IsDateSpanAvailable(accommodationId))
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(-1);
                isDateSpanAllowed = _startDateIterator.CompareTo(_iterationStopperDate) >= 0;
            }
            return availableDates;
        }

        private List<DateSpan> FindAvailableDatesAfterDateRange(DateTime dateRangeEnd, int accommodationId)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            PrepareDateIterators(dateRangeEnd.AddDays(-_reservationLength + 2), dateRangeEnd.AddDays(1), DateTime.Now);
            while (availableDates.Count() < 3)
            {
                if (IsDateSpanAvailable(accommodationId))
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(1);
            }
            return availableDates;
        }

        private bool IsDateSpanAvailable(int accommodationId)
        {
            DateOnly dateIterator = new DateOnly(_startDateIterator.Year, _startDateIterator.Month, _startDateIterator.Day);
            bool isDateInsideSpan = dateIterator.CompareTo(_endDateIterator) <= 0;
            while (isDateInsideSpan)
            {
                foreach (AccommodationReservation reservation in GetAllByAccommodationId(accommodationId))
                {
                    bool isDateAvailable = (dateIterator.CompareTo(reservation.DateSpan.StartDate) < 0) || (dateIterator.CompareTo(reservation.DateSpan.EndDate) > 0);
                    if (!isDateAvailable) return false;
                }
                dateIterator = dateIterator.AddDays(1);
                isDateInsideSpan = dateIterator.CompareTo(_endDateIterator) <= 0;
            }
            return true;
        }

        private void PrepareDateIterators(DateTime startDateIterator, DateTime endDateIterator, DateTime iterationStopperDate)
        {
            _startDateIterator = DateOnly.FromDateTime(startDateIterator);
            _endDateIterator = DateOnly.FromDateTime(endDateIterator);
            _iterationStopperDate = DateOnly.FromDateTime(iterationStopperDate);
        }

        private void AddDaysToIterators(int dayNumber)
        {
            _startDateIterator = _startDateIterator.AddDays(dayNumber);
            _endDateIterator = _endDateIterator.AddDays(dayNumber);
        }

        private DateSpan CreateDateSpan()
        {
            DateSpan dateSpan = new DateSpan(_startDateIterator, _endDateIterator);
            return dateSpan;
        }

        public int CalculateDaysLeftForRating2(AccommodationReservation accommodationReservation)
        {
            return 5 - DateOnly.FromDateTime(DateTime.Now).DayNumber + accommodationReservation.DateSpan.EndDate.DayNumber;
        }

        public bool IsReservationActive2(AccommodationReservation accommodationReservation)
        {
            return DateOnly.FromDateTime(DateTime.Now).CompareTo(accommodationReservation.DateSpan.EndDate) <= 0;
        }

        public List<AccommodationReservation> GetUnrated2(IEnumerable<AccommodationOwnerRating> ratings)
        {
            List<AccommodationReservation> unrated = new();

            foreach (var accommodationReservation in _accommodationReservations)
            {
                if (IsValidForRating2(accommodationReservation, ratings))
                {
                    unrated.Add(accommodationReservation);
                    continue;
                }
            }

            return unrated;
        }

        private bool IsValidForRating2(AccommodationReservation accommodationReservation, IEnumerable<AccommodationOwnerRating> accommodationOwnerRatings)
        {
            return CalculateDaysLeftForRating2(accommodationReservation) >= 0 &&
                !IsReservationActive2(accommodationReservation) &&
                !IsRated2(accommodationReservation, accommodationOwnerRatings);
        }

        private bool IsRated2(AccommodationReservation accommodationReservation, IEnumerable<AccommodationOwnerRating> accommodationOwnerRatings)
        {
            foreach (var rating in accommodationOwnerRatings)
            {
                if (accommodationReservation.Id == rating.AccommodationReservationId)
                {
                    return true;
                }
            }

            return false;
        }

        
    }
}
