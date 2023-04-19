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
    public class TourRatingPhotoRepository : ITourRatingPhotoRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRatingPhotos.csv";
        private readonly Serializer<TourRatingPhoto> _serializer;
        private List<TourRatingPhoto> photos;

        public TourRatingPhotoRepository()
        {
            _serializer = new Serializer<TourRatingPhoto>();
            photos = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (photos.Count == 0)
            {
                return 1;
            }
            return photos[photos.Count - 1].Id + 1;
        }

        public List<TourRatingPhoto> GetAll()
        {
            return photos;
        }

        public TourRatingPhoto Save(TourRatingPhoto photo)
        {
            photo.Id = NextId();
            photos.Add(photo);
            _serializer.ToCSV(FilePath, photos);
            return photo;
        }

    }
}
