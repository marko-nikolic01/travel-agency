using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> serializer;

        private List<Accommodation> accommodations;

        public AccommodationRepository()
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);
        }

        public AccommodationRepository(List<User> users, List<Location> locations, List<AccommodationPhoto> photos) : this()
        {
            LinkOwners(users);
            LinkLocations(locations);
            LinkPhotos(photos);
        }

        public void LinkOwners(List<User> owners)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (User user in owners)
                {
                    if (accommodation.OwnerId == user.Id)
                    {
                        accommodation.Owner = user;
                        break;
                    }
                }
            }
        }

        public void LinkLocations(List<Location> locations)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (Location location in locations)
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        accommodation.Location = location;
                        break;
                    }
                }
            }
        }

        public void LinkPhotos(List<AccommodationPhoto> photos)
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (AccommodationPhoto photo in photos)
                {
                    if (accommodation.Id == photo.ObjectId)
                    {
                        accommodation.Photos.Add(photo);
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

        public List<Accommodation> GetByOwner(User owner)
        {
            return accommodations.FindAll(c => c.OwnerId == owner.Id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations.Add(accommodation);
            serializer.ToCSV(FilePath, accommodations);
            return accommodation;
        }
    }
}
