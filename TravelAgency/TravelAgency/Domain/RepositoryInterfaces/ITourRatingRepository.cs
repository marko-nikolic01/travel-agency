using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourRatingRepository
    {
        public List<TourRating> GetAll();
        public List<TourRating> GetRatingsByTourOccurrenceId(int id);
        public int GetRatingsNumberByGuestId(int id);
        public int NextId();
        public TourRating Save(TourRating tourRating);
        public bool IsTourNotRated(int guestId, int occurrenceId);
        public void UpdateIsValid(TourRating tourRating);
    }
}
