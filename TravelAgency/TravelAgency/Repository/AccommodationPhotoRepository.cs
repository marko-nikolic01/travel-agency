using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationPhotoRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationPhotos.csv";

        private readonly Serializer<AccommodationPhoto> _serializer;

        private List<AccommodationPhoto> _accommodationPhotos;

        public AccommodationPhotoRepository()
        {
            _serializer = new Serializer<AccommodationPhoto>();
            _accommodationPhotos = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _accommodationPhotos = _serializer.FromCSV(FilePath);
            if (_accommodationPhotos.Count < 1)
            {
                return 1;
            }
            return _accommodationPhotos.Max(c => c.Id) + 1;
        }

        public AccommodationPhoto Save(AccommodationPhoto image)
        {
            image.Id = NextId();
            _accommodationPhotos = _serializer.FromCSV(FilePath);
            _accommodationPhotos.Add(image);
            _serializer.ToCSV(FilePath, _accommodationPhotos);
            return image;
        }

        public List<AccommodationPhoto> GetAll()
        {
            return _accommodationPhotos;
        }
    }
}
