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
    public class AccommodationRepository : IRepository<Accommodation>
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> serializer;

        private List<Accommodation> accommodations;

        public AccommodationRepository(UserRepository userRepository, LocationRepository locationRepository, AccommodationPhotoRepository imageRepository)
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);

            foreach (Accommodation accommodation in accommodations)
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
                        accommodation.Photos.Add(image);
                    }
                }
            }
        }

        public int NextId()
        {
            if (accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public List<Accommodation> GetByUser(User user)
        {
            return accommodations.FindAll(c => c.OwnerId == user.Id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        IEnumerable<Accommodation> IRepository<Accommodation>.GetAll()
        {
            throw new NotImplementedException();
        }

        public Accommodation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations.Add(accommodation);
            serializer.ToCSV(FilePath, accommodations);
            return accommodation;
        }

        public void SaveAll(IEnumerable<Accommodation> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Accommodation entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
