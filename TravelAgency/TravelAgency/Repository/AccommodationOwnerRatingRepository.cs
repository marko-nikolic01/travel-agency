using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationOwnerRatingRepository : IRepository<AccommodationOwnerRating>
    {
        private const string FilePath = "../../../Resources/Data/accommodationOwnerRatings.csv";
        private readonly Serializer<AccommodationOwnerRating> serializer;
        private List<AccommodationOwnerRating> accommodationOwnerRatings;

        public AccommodationOwnerRatingRepository(IEnumerable<AccommodationReservation> accommodationReservations)
        {
            serializer = new Serializer<AccommodationOwnerRating>();
            accommodationOwnerRatings = serializer.FromCSV(FilePath);

            foreach (var accommodationOwnerRating in accommodationOwnerRatings)
            {
                foreach (var accommodationReservation in accommodationReservations)
                {
                    if (accommodationOwnerRating.AccommodationReservationId == accommodationReservation.Id)
                    {
                        accommodationOwnerRating.AccommodationReservation = accommodationReservation;
                    }
                }
            }
        }

        public void Delete(AccommodationOwnerRating entity)
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

        public List<AccommodationOwnerRating> GetAll()
        {
            return accommodationOwnerRatings;
        }

        public AccommodationOwnerRating GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int NextId()
        {
            if (accommodationOwnerRatings.Count < 1)
            {
                return 1;
            }
            return accommodationOwnerRatings.Max(c => c.Id) + 1;
        }

        public AccommodationOwnerRating Save(AccommodationOwnerRating entity)
        {
            entity.Id = NextId();
            accommodationOwnerRatings.Add(entity);
            serializer.ToCSV(FilePath, accommodationOwnerRatings);
            return entity;
        }

        public void SaveAll(IEnumerable<AccommodationOwnerRating> entities)
        {
            throw new NotImplementedException();
        }
    }
}
