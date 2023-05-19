using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Injector;

namespace TravelAgency.Services
{
    public class ReservationDateFinderService
    {
        private int _reservationLength;
        private DateOnly _startDateIterator;
        private DateOnly _endDateIterator;
        private DateOnly _iterationStopperDate;
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IAccommodationRenovationRepository RenovationRepository { get; set; }

        public ReservationDateFinderService()
        {
            _reservationLength = 1;
            _startDateIterator = DateOnly.FromDateTime(DateTime.Now);
            _endDateIterator = DateOnly.FromDateTime(DateTime.Now);
            _iterationStopperDate = DateOnly.FromDateTime(DateTime.Now);
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            RenovationRepository = Injector.Injector.CreateInstance<IAccommodationRenovationRepository>();
        }

        public void SetReservationLength(int length)
        {
            _reservationLength = length;
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

        public List<DateSpan> FindAvailableDatesInsideDateRange(DateTime dateRangeStart, DateTime dateRangeEnd, Accommodation accommodation)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            PrepareDateIterators(dateRangeStart, dateRangeStart.AddDays(_reservationLength - 1), dateRangeEnd);
            bool isDateSpanAllowed = _endDateIterator.CompareTo(_iterationStopperDate) <= 0;
            while (isDateSpanAllowed)
            {
                if (IsDateSpanAvailable(accommodation))
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(1);
                isDateSpanAllowed = _endDateIterator.CompareTo(_iterationStopperDate) <= 0;
            }
            return availableDates;
        }

        public List<DateSpan> FindAvailableDatesInsideDateRange(DateTime dateRangeStart, DateTime dateRangeEnd, int numberOfDays, Accommodation accommodation)
        {
            SetReservationLength(numberOfDays);
            return FindAvailableDatesInsideDateRange(dateRangeStart, dateRangeEnd, accommodation);
        }

        public List<DateSpan> FindAvailableDatesOutsideDateRange(DateTime dateRangeStart, DateTime dateRangeEnd, Accommodation accommodation)
        {
            List<DateSpan> availableDatesBefore = FindAvailableDatesBeforeDateRange(dateRangeStart, accommodation);
            availableDatesBefore.Reverse();
            List<DateSpan> availableDatesAfter = FindAvailableDatesAfterDateRange(dateRangeEnd, accommodation);
            List<DateSpan> availableDates = availableDatesBefore.Concat(availableDatesAfter).ToList();
            return availableDates;
        }

        private List<DateSpan> FindAvailableDatesBeforeDateRange(DateTime dateRangeStart, Accommodation accommodation)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            PrepareDateIterators(dateRangeStart.AddDays(-1), dateRangeStart.AddDays(_reservationLength - 2), DateTime.Now.AddDays(1));
            bool isDateSpanAllowed = _startDateIterator.CompareTo(_iterationStopperDate) > 0;
            while (isDateSpanAllowed && availableDates.Count() < 3)
            {
                if (IsDateSpanAvailable(accommodation))
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(-1);
                isDateSpanAllowed = _startDateIterator.CompareTo(_iterationStopperDate) >= 0;
            }
            return availableDates;
        }

        private List<DateSpan> FindAvailableDatesAfterDateRange(DateTime dateRangeEnd, Accommodation accommodation)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            PrepareDateIterators(dateRangeEnd.AddDays(-_reservationLength + 2), dateRangeEnd.AddDays(1), DateTime.Now);
            while (availableDates.Count() < 3)
            {
                if (IsDateSpanAvailable(accommodation))
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(1);
            }
            return availableDates;
        }

        public List<DateSpan> FindDatesForReservationMoveRequest(DateTime dateRangeStart, DateTime dateRangeEnd, AccommodationReservation reservation)
        {
            List<DateSpan> availableDates = new List<DateSpan>();
            SetReservationLength(reservation.DateSpan.EndDate.DayNumber - reservation.DateSpan.StartDate.DayNumber + 1);
            PrepareDateIterators(dateRangeStart, dateRangeStart.AddDays(_reservationLength - 1), dateRangeEnd);
            bool isDateSpanAllowed = _endDateIterator.CompareTo(_iterationStopperDate) <= 0;
            while (isDateSpanAllowed)
            {
                if (_startDateIterator.CompareTo(reservation.DateSpan.StartDate) != 0)
                {
                    availableDates.Add(CreateDateSpan());
                }
                AddDaysToIterators(1);
                isDateSpanAllowed = _endDateIterator.CompareTo(_iterationStopperDate) <= 0;
            }
            return availableDates;
        }

        private bool IsDateSpanAvailable(Accommodation accommodation)
        {
            DateOnly dateIterator = new DateOnly(_startDateIterator.Year, _startDateIterator.Month, _startDateIterator.Day);
            bool isDateInsideSpan = dateIterator.CompareTo(_endDateIterator) <= 0;
            while (isDateInsideSpan)
            {
                foreach (AccommodationReservation reservation in ReservationRepository.GetByAccommodation(accommodation))
                {
                    bool isDateAvailable = dateIterator.CompareTo(reservation.DateSpan.StartDate) < 0 || dateIterator.CompareTo(reservation.DateSpan.EndDate) > 0;
                    if (!isDateAvailable) return false;
                }
                foreach (AccommodationRenovation renovation in RenovationRepository.GetByAccommodation(accommodation))
                {
                    bool isDateAvailable = dateIterator.CompareTo(renovation.DateSpan.StartDate) < 0 || dateIterator.CompareTo(renovation.DateSpan.EndDate) > 0;
                    if (!isDateAvailable) return false;
                }
                dateIterator = dateIterator.AddDays(1);
                isDateInsideSpan = dateIterator.CompareTo(_endDateIterator) <= 0;
            }
            return true;
        }

        public bool IsFutureDate(DateTime date)
        {
            return DateOnly.FromDateTime(DateTime.Now).CompareTo(DateOnly.FromDateTime(date)) < 0;
        }
    }
}
