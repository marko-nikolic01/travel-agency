using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class LocationRepository : IRepository<Location>
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";
        private readonly Serializer<Location> _serializer;
        private List<Location> locations;

        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            locations = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (locations.Count == 0)
            {
                return 1;
            }
            return locations[locations.Count - 1].Id + 1;
        }
        
        public List<Location> GetAll()
        {
            return locations;
        }

        public Location Save(Location location)
        {
            location.Id = NextId();
            locations.Add(location);
            _serializer.ToCSV(FilePath, locations);

            return location;
        }

        public List<string> GetAllCountries()
        {
            List<string> countries = new List<string>();

            foreach (Location location in locations)
            {
                if (!countries.Contains(location.Country))
                {
                    countries.Add(location.Country);
                }
            }

            return countries;
        }

        public List<string> GetAllCities()
        {
            List<string> cities = new List<string>();

            foreach (Location location in locations)
            {
                cities.Add(location.City);
            }

            return cities;
        }

        public List<string> GetCitiesByCountry(string country)
        {
            List<string> cities = new List<string>();

            if (country == "Not Specified")
            {
                return GetAllCities();
            }

            foreach (Location location in locations)
            {
                if (location.Country == country)
                {
                    cities.Add(location.City);
                }
            }

            cities.Sort();

            return cities;
        }

        public Location GetByID(int id)
        {
            foreach (Location location in locations)
            {
                if (location.Id == id)
                {
                    return location;
                }
            }
            return null;
        }

        public Location GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<Location> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Location entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public Location GetLocationForCountryAndCity(string country, string city)
        {
            foreach(Location location in locations)
            {
                if (location.Country == country && location.City == city)
                {
                    return location;
                }
            }

            return null;
        }
    }
}
