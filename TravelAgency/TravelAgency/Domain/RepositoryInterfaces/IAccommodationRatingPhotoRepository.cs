using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationRatingPhotoRepository
    {
        public List<AccommodationRatingPhoto> GetAll();

        public int NextId();

        public AccommodationRatingPhoto Save(AccommodationRatingPhoto photo);

        public void SaveAll(List<AccommodationRatingPhoto> photos);
    }
}
