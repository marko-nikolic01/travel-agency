using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        public AccommodationRepository(UserRepository userRepository, LocationRepository locationRepository, AccommodationPhotoRepository imageRepository)
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);

            foreach (Accommodation accommodation in _accommodations)
            {
                foreach (User user in userRepository.GetUsers())
                {
                    if (accommodation.OwnerId == user.Id)
                    {
                        accommodation.Owner = user;
                        break;
                    }
                }
                foreach (Location location in locationRepository.GetLocations())
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        accommodation.Location = location;
                        break;
                    }
                }

                foreach (AccommodationPhoto image in imageRepository.GetAll())
                {
                    if (accommodation.Id == image.ObjectId)
                    {
                        accommodation.Images.Add(image);
                    }
                }
            }
        }

        public int NextId()
        {
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }
        
        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
            return accommodation;
        }

        public List<Accommodation> GetByUser(User user)
        {
            return _accommodations.FindAll(c => c.OwnerId == user.Id);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }
    }
}
