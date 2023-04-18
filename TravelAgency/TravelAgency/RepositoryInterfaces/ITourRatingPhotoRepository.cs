using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface ITourRatingPhotoRepository
    {
        public List<TourRatingPhoto> GetAll();

        public TourRatingPhoto Save(TourRatingPhoto photo);
    }
}
