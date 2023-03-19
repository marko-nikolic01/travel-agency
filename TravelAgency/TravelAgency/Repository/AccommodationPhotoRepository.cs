using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationPhotoRepository : IRepository<AccommodationPhoto>
    {
        private const string FilePath = "../../../Resources/Data/accommodationPhotos.csv";

        private readonly Serializer<AccommodationPhoto> serializer;

        private List<AccommodationPhoto> accommodationPhotos;

        public AccommodationPhotoRepository()
        {
            serializer = new Serializer<AccommodationPhoto>();
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

        public List<AccommodationPhoto> GetAll()
        {
            return accommodationPhotos;
        }

        List<AccommodationPhoto> IRepository<AccommodationPhoto>.GetAll()
        {
            throw new NotImplementedException();
        }

        public AccommodationPhoto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AccommodationPhoto Save(AccommodationPhoto image)
        {
            image.Id = NextId();
            accommodationPhotos.Add(image);
            serializer.ToCSV(FilePath, accommodationPhotos);
            return image;
        }

        public void SaveAll(IEnumerable<AccommodationPhoto> entities)
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

        public void Delete(AccommodationPhoto entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
