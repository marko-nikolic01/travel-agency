using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class AccommodationSearchService
    {
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }

        public AccommodationSearchService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
        }

        public List<Accommodation> Search(AccommodationSearchFilter filter)
        {
            List<Accommodation> searchedAccommodations = AccommodationRepository.GetActive();
            searchedAccommodations = FilterByName(filter.NameFilter, searchedAccommodations);
            searchedAccommodations = FilterByCountry(filter.CountryFilter, searchedAccommodations);
            searchedAccommodations = FilterByCity(filter.CityFilter, searchedAccommodations);
            searchedAccommodations = FilterByType(filter.TypeFilter, searchedAccommodations);
            searchedAccommodations = FilterByGuestNumber(filter.GuestNumberFilter, searchedAccommodations);
            searchedAccommodations = FilterByDayNumber(filter.DayNumberFilter, searchedAccommodations);

            return searchedAccommodations;
        }

        private List<Accommodation> FilterByName(string nameFilter, List<Accommodation> accommodations)
        {
            if (nameFilter != "")
            {
                return accommodations.Where(accommodation => accommodation.Name.ToLower().Trim().Contains(nameFilter.ToLower().Trim())).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByCountry(string countryFilter, List<Accommodation> accommodations)
        {
            if (countryFilter != "Not specified" && countryFilter != "-")
            {
                return accommodations.Where(accommodation => accommodation.Location.Country.ToLower().Contains(countryFilter.ToLower())).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByCity(string cityFilter, List<Accommodation> accommodations)
        {
            if (cityFilter != "Not specified" && cityFilter != "-")
            {
                return accommodations.Where(accommodation => accommodation.Location.City.ToLower().Contains(cityFilter.ToLower())).ToList();
            }
            return accommodations;
        }

        private List<Accommodation> FilterByType(string typeFilter, List<Accommodation> accommodations)
        {
            switch (typeFilter)
            {
                case "Appartment":
                case "Apartman":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.APARTMENT).ToList();
                case "House":
                case "Kuća":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.HOUSE).ToList();
                case "Hut":
                case "Koliba":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.HUT).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> FilterByGuestNumber(int guestNumberFilter, List<Accommodation> accommodations)
        {
            if (guestNumberFilter > 0)
            {
                return accommodations.Where(accommodation => guestNumberFilter <= accommodation.MaxGuests).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> FilterByDayNumber(int DayFilter, List<Accommodation> accommodations)
        {
            if (DayFilter > 0)
            {
                return accommodations.Where(accommodation => DayFilter >= accommodation.MinDays).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> CancelSearch()
        {
            return AccommodationRepository.GetActive();
        }
    }
}
