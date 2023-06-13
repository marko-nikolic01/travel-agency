using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class WhereverWheneverService
    {
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        private AccommodationDateFinderService _dateFinderService;
        private AccommodationSearchService _accommodationSearchService;

        public WhereverWheneverService()
        {
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            _dateFinderService = new AccommodationDateFinderService();
            _accommodationSearchService = new AccommodationSearchService();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetActive());
        }

        public List<Accommodation> SearchAccommodationsByFilter(WhereverWheneverSearchFilter filter)
        {
            if (!filter.IsValid)
            {
                return null;
            }
            List<Accommodation> accommodations = AccommodationRepository.GetActive();
            accommodations = _accommodationSearchService.FilterByGuestNumber(filter.GuestNumber, accommodations);
            accommodations = _accommodationSearchService.FilterByDayNumber(filter.DayNumber, accommodations);
            if (filter.SearchInsideDateSpan)
            {
                _dateFinderService.SetReservationLength(filter.DayNumber);
                accommodations = GetAvailableAccommodationsInsideDateSpan(filter.FirstDate, filter.LastDate, accommodations);
            }
            return accommodations;
        }

        private List<Accommodation> GetAvailableAccommodationsInsideDateSpan(DateTime firstDate, DateTime lastDate, List<Accommodation> accommodations)
        {
            List<Accommodation> filteredAccommodations = new List<Accommodation>();
            foreach (Accommodation accommodation in accommodations)
            {
                List<DateSpan> availableDates = _dateFinderService.FindAvailableDatesInsideDateRange(firstDate, lastDate, accommodation);
                if (availableDates.Count() > 0)
                {
                    filteredAccommodations.Add(accommodation);
                }
            }
            return filteredAccommodations;
        }

        public List<DateSpan> GetAvailableDateSpans(WhereverWheneverSearchFilter filter, Accommodation accommodation)
        {
            _dateFinderService.SetReservationLength(filter.DayNumber);
            if (filter.SearchInsideDateSpan)
            {
                return _dateFinderService.FindAvailableDatesInsideDateRange(filter.FirstDate, filter.LastDate, accommodation);
            }
            return _dateFinderService.FindAnyAvailableDates(accommodation);
        }
    }
}
