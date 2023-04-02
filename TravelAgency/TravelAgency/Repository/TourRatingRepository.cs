using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class TourRatingRepository : IRepository<TourRating>
    {
        private const string FilePath = "../../../Resources/Data/tourRatings.csv";
        private readonly Serializer<TourRating> _serializer;
        private List<TourRating> tourRatings;
        public TourRatingRepository()
        {
            _serializer = new Serializer<TourRating>();
            tourRatings = _serializer.FromCSV(FilePath);
        }

        public List<TourRating> GetAll()
        {
            return tourRatings;
        }

        public TourRating GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int NextId()
        {
            if (tourRatings.Count == 0)
            {
                return 1;
            }
            return tourRatings[tourRatings.Count - 1].Id + 1;
        }

        public TourRating Save(TourRating tourRating)
        {
            tourRating.Id = NextId();
            tourRatings.Add(tourRating);
            _serializer.ToCSV(FilePath, tourRatings);

            return tourRating;
        }

        public void SaveAll(IEnumerable<TourRating> entities)
        {
            throw new NotImplementedException();
        }
        public void Delete(TourRating entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

    }
}
