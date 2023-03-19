using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationGuestRatingRepository : IRepository<AccommodationGuestRating>
    {
        private const string FilePath = "../../../Resources/Data/accommodationGuestRatings.csv";
        private readonly Serializer<AccommodationGuestRating> _serializer;
        private List<AccommodationGuestRating> _accommodationGuestRatings;

        public AccommodationGuestRatingRepository(IEnumerable<AccommodationReservation> accommodationReservations)
        {
            _serializer = new Serializer<AccommodationGuestRating>();
            _accommodationGuestRatings = _serializer.FromCSV(FilePath);

            foreach (var accommodationGuestRating in _accommodationGuestRatings)
            {
                foreach (var accommodationReservation in accommodationReservations)
                {
                    if (accommodationGuestRating.AccommodationReservationId == accommodationReservation.Id)
                    {
                        accommodationGuestRating.AccommodationReservation = accommodationReservation;
                    }
                }
            }
        }

        public void Delete(AccommodationGuestRating entity)
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

        public List<AccommodationGuestRating> GetAll()
        {
            return _accommodationGuestRatings;
        }

        public AccommodationGuestRating GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int NextId()
        {
            if (_accommodationGuestRatings.Count < 1)
            {
                return 1;
            }
            return _accommodationGuestRatings.Max(c => c.Id) + 1;
        }

        public AccommodationGuestRating Save(AccommodationGuestRating entity)
        {
            entity.Id = NextId();
            _accommodationGuestRatings.Add(entity);
            _serializer.ToCSV(FilePath, _accommodationGuestRatings);
            return entity;
        }

        public void SaveAll(IEnumerable<AccommodationGuestRating> entities)
        {
            throw new NotImplementedException();
        }
    }
}
