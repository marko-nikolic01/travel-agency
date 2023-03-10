using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class PhotoRepository
    {
        private const string FilePath = "../../../Resources/Data/photos.csv";
        private readonly Serializer<Photo> _serializer;
        private List<Photo> photos;

        public PhotoRepository()
        {
            _serializer = new Serializer<Photo>();
            photos = _serializer.FromCSV(FilePath);
        }

        private int GetNewId()
        {
            if (photos.Count == 0)
            {
                return 1;
            }
            return photos[photos.Count - 1].Id + 1;
        }


        public List<Photo> GetPhotos()
        {
            return photos;
        }
        public void SavePhotos(Photo photo)
        {
            photo.Id = GetNewId();
            photos.Add(photo);
            _serializer.ToCSV(FilePath, photos);
        }
    }
}
