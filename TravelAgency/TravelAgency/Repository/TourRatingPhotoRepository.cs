using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class TourRatingPhotoRepository : IRepository<TourRatingPhoto>
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

        public TourRatingPhoto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public TourRatingPhoto Save(TourRatingPhoto photo)
        {
            photo.Id = NextId();
            photos.Add(photo);
            _serializer.ToCSV(FilePath, photos);
            return photo;
        }

        public void SaveAll(IEnumerable<TourRatingPhoto> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TourRatingPhoto entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
