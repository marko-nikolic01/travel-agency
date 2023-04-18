using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        public List<TourRating> GetAll();
        public List<TourRating> GetRatingsByTourOccurrenceId(int id);
        public int NextId();
        public TourRating Save(TourRating tourRating);
        public bool IsTourNotRated(int guestId, int occurrenceId);
        public void UpdateIsValid(TourRating tourRating);
    }
}
