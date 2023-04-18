using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IAccommodationRatingPhotoRepository
    {
        public List<AccommodationRatingPhoto> GetAll();

        public int NextId();

        public AccommodationRatingPhoto Save(AccommodationRatingPhoto photo);

        public void SaveAll(List<AccommodationRatingPhoto> photos);
    }
}
