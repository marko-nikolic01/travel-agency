using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class LocationService
    {
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public LocationService()
        {
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
        }

        public List<Location> GetAllLocations() 
        {
            return LocationRepository.GetAll();
        }

        public List<string> GetCountries()
        {
            return LocationRepository.GetAllCountries();
        }

        public List<string> GetCities()
        {
            return LocationRepository.GetAllCities();
        }

        public List<string> GetCitiesByCountry(string country)
        {
            return LocationRepository.GetCitiesByCountry(country);
        }

        public Location GetLocationForCountryAndCity(string country, string city)
        {
            foreach (Location location in LocationRepository.GetAll())
            {
                if (location.Country == country && location.City == city)
                {
                    return location;
                }
            }

            return null;
        }

        public List<Location> GetLocationsByCountry(string country)
        {
            List<Location> locations = new List<Location>();
            foreach (Location location in LocationRepository.GetAll())
            {
                if (location.Country == country)
                {
                    locations.Add(location);
                }
            }
            return locations;
        }

        public List<Location> GetLocationsByCity(string city)
        {
            List<Location> locations = new List<Location>();
            foreach (Location location in LocationRepository.GetAll())
            {
                if (location.City == city)
                {
                    locations.Add(location);
                }
            }
            return locations;
        }

        public bool CountryExists(string country)
        {
            return LocationRepository.CountryExists(country);
        }

        public bool CityExists(string city)
        {
            return LocationRepository.CityExists(city);
        }

        public List<Location> GetLocationsByOwner(User owner)
        {
            var locations = new List<Location>();

            foreach (var accommodation in AccommodationRepository.GetActiveByOwner(owner))
            {
                if (!locations.Contains(accommodation.Location))
                {
                    locations.Add(accommodation.Location);
                }
            }

            return locations;
        }
    }
}
