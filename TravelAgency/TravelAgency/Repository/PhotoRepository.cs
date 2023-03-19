using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class PhotoRepository : IRepository<Photo>
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

        public Photo GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Photo Save(Photo photo)
        {
            photo.Id = NextId();
            photos.Add(photo);
            _serializer.ToCSV(FilePath, photos);
            return photo;
        }

        public void SaveAll(IEnumerable<Photo> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Photo entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
