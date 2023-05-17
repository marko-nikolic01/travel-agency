using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ILocationRepository
    {
        public List<Location> GetAll();

        public Location Save(Location location);

        public List<string> GetAllCountries();

        public List<string> GetAllCities();

        public List<string> GetCitiesByCountry(string country);

        public Location GetByID(int id);

        public Location GetLocationForCountryAndCity(string country, string city);

        public bool CountryExists(string country);

        public bool CityExists(string country);
    }
}
