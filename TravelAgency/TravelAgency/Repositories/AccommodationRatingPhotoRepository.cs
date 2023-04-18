using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class AccommodationRatingPhotoRepository : IAccommodationRatingPhotoRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRatingPhotos.csv";

        private readonly Serializer<AccommodationRatingPhoto> serializer;

        private List<AccommodationRatingPhoto> accommodationPhotos;

        public AccommodationRatingPhotoRepository()
        {
            serializer = new Serializer<AccommodationRatingPhoto>();
            accommodationPhotos = serializer.FromCSV(FilePath);
        }

        public List<AccommodationRatingPhoto> GetAll()
        {
            return accommodationPhotos;
        }

        public int NextId()
        {
            if (accommodationPhotos.Count < 1)
            {
                return 1;
            }
            return accommodationPhotos.Max(c => c.Id) + 1;
        }

        public AccommodationRatingPhoto Save(AccommodationRatingPhoto photo)
        {
            photo.Id = NextId();
            accommodationPhotos.Add(photo);
            serializer.ToCSV(FilePath, accommodationPhotos);
            return photo;
        }

        public void SaveAll(List<AccommodationRatingPhoto> photos)
        {
            foreach (AccommodationRatingPhoto photo in photos)
            {
                Save(photo);
            }
        }
    }
}
