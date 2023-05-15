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
        public LocationService()
        {
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
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

        public bool CountryExists(string country)
        {
            return LocationRepository.CountryExists(country);
        }

        public bool CityExists(string city)
        {
            return LocationRepository.CityExists(city);
        }
    }
}
