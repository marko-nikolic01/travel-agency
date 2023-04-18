using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private const string FilePath = "../../../Resources/Data/photos.csv";
        private readonly Serializer<Photo> _serializer;
        private List<Photo> photos;

        public PhotoRepository()
        {
            _serializer = new Serializer<Photo>();
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

        public List<Photo> GetAll()
        {
            return photos;
        }

        public Photo Save(Photo photo)
        {
            photo.Id = NextId();
            photos.Add(photo);
            _serializer.ToCSV(FilePath, photos);
            return photo;
        }
        public List<Photo> GetByTour(int id)
        {
            List<Photo> result = new List<Photo>();
            foreach (Photo p in photos)
            {
                if (p.TourId == id)
                {
                    result.Add(p);
                }
            }
            return result;
        }
    }
}
