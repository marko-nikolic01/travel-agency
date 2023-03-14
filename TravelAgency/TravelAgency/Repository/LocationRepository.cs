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
    public class LocationRepository
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
        
        public List<Location> GetLocations()
        {
            return locations;
        }

        public Location SaveLocation(Location location)
        {
            location.Id = NextId();
            locations.Add(location);
            _serializer.ToCSV(FilePath, locations);

            return location;
        }
    }
}
