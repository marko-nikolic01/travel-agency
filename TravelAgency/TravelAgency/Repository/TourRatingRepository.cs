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

        public TourRatingRepository(TourRatingPhotoRepository tourRatingPhotoRepository)
        {
            _serializer = new Serializer<TourRating>();
            tourRatings = _serializer.FromCSV(FilePath);
            LinkRatingPhoto(tourRatingPhotoRepository);
        }

        private void LinkRatingPhoto(TourRatingPhotoRepository tourRatingPhotoRepository)
        {
            foreach (TourRatingPhoto tourRatingPhoto in tourRatingPhotoRepository.GetAll())
            {
                TourRating tourRating = tourRatings.Find(t => t.Id == tourRatingPhoto.TourRatingId);
                if (tourRating != null)
                {
                    tourRating.PhotoUrls.Add(tourRatingPhoto);
                }
            }
        }

        public List<TourRating> GetAll()
        {
            return tourRatings;
        }

        public TourRating GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TourRating> GetRatingsByTourOccurrenceId(int id)
        {
            List<TourRating> result = new List<TourRating>();
            foreach (TourRating tourRating in tourRatings)
            {
                if (tourRating.TourOccurrenceId == id)
                {
                    result.Add(tourRating);
                }
            }
            return result;
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

        public bool IsTourNotRated(int guestId, int occurrenceId)
        {
            return !tourRatings.Exists(x => x.GuestId == guestId && x.TourOccurrenceId == occurrenceId);
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
        public void UpdateIsValid(TourRating tourRating)
        {
            TourRating oldTourRating = tourRatings.Find(t => t.Id == tourRating.Id);
            oldTourRating.IsValid = tourRating.IsValid;
            _serializer.ToCSV(FilePath, tourRatings);
        }

    }
}
