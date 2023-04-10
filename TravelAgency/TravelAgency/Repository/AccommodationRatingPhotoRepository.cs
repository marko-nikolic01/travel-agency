using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationRatingPhotoRepository : IRepository<AccommodationRatingPhoto>
    {
        private const string FilePath = "../../../Resources/Data/accommodationRatingPhotos.csv";

        private readonly Serializer<AccommodationRatingPhoto> serializer;

        private List<AccommodationRatingPhoto> accommodationPhotos;

        public AccommodationRatingPhotoRepository()
        {
            serializer = new Serializer<AccommodationRatingPhoto>();
            accommodationPhotos = serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (accommodationPhotos.Count < 1)
            {
                return 1;
            }
            return accommodationPhotos.Max(c => c.Id) + 1;
        }

        public List<AccommodationRatingPhoto> GetAll()
        {
            return accommodationPhotos;
        }

        public AccommodationRatingPhoto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AccommodationRatingPhoto Save(AccommodationRatingPhoto image)
        {
            image.Id = NextId();
            accommodationPhotos.Add(image);
            serializer.ToCSV(FilePath, accommodationPhotos);
            return image;
        }

        public void SaveAll(IEnumerable<AccommodationRatingPhoto> entities)
        {
            foreach (var entity in entities)
            {
                Save(entity);
            }
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(AccommodationRatingPhoto entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
