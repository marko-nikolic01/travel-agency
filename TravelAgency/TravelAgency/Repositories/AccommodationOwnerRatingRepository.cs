using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class AccommodationOwnerRatingRepository : IAccommodationOwnerRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationOwnerRatings.csv";
        private readonly Serializer<AccommodationOwnerRating> serializer;
        private List<AccommodationOwnerRating> accommodationOwnerRatings;

        public AccommodationOwnerRatingRepository()
        {
            serializer = new Serializer<AccommodationOwnerRating>();
            accommodationOwnerRatings = serializer.FromCSV(FilePath);
        }

        public void LinkReservations(List<AccommodationReservation> reservations)
        {
            foreach (var accommodationOwnerRating in accommodationOwnerRatings)
            {
                foreach (var accommodationReservation in reservations)
                {
                    if (accommodationOwnerRating.AccommodationReservationId == accommodationReservation.Id)
                    {
                        accommodationOwnerRating.AccommodationReservation = accommodationReservation;
                    }
                }
            }
        }

        public void LinkRenovationRecommendations(List<RenovationRecommendation> recommendations)
        {
            foreach (var accommodationOwnerRating in accommodationOwnerRatings)
            {
                foreach (var recommendation in recommendations)
                {
                    if (accommodationOwnerRating.RenovationReccommendationId == recommendation.Id)
                    {
                        accommodationOwnerRating.RenovationRecommendation = recommendation;
                    }
                }
            }
        }

        public List<AccommodationOwnerRating> GetAll()
        {
            return accommodationOwnerRatings;
        }

        public AccommodationOwnerRating GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<AccommodationOwnerRating> GetByOwner(User owner)
        {
            return accommodationOwnerRatings.FindAll(c => c.AccommodationReservation.Accommodation.OwnerId == owner.Id);
        }

        public List<AccommodationOwnerRating> GetRatingsVisibleToOwner(User user, IEnumerable<AccommodationGuestRating> guestRatings)
        {
            List<AccommodationOwnerRating> ownerRatings = new List<AccommodationOwnerRating>();

            foreach (var guestRating in guestRatings)
            {
                foreach (var accommodationOwnerRating in accommodationOwnerRatings)
                {
                    if (accommodationOwnerRating.AccommodationReservationId == guestRating.AccommodationReservationId && user.Id == accommodationOwnerRating.AccommodationReservation.Accommodation.OwnerId)
                    {
                        ownerRatings.Add(accommodationOwnerRating);
                        break;
                    }
                }
            }

            return ownerRatings;
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

        public void SaveAll()
        {
            serializer.ToCSV(FilePath, accommodationOwnerRatings);
        }
    }
}
