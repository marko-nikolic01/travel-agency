using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationReservationRepository : IRepository<AccommodationReservation>
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository(AccommodationRepository accommodationRepository, UserRepository userRepository)
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);

            foreach (AccommodationReservation accommodationReservation in _accommodationReservations)
            {
                foreach (Accommodation accommodation in accommodationRepository.GetAll())
                {
                    if (accommodationReservation.AccomodationId == accommodation.Id)
                    {
                        accommodationReservation.Accommodation = accommodation;
                    }
                }

                foreach (User user in userRepository.GetUsers())
                {
                    if (accommodationReservation.GuestId == user.Id)
                    {
                        accommodationReservation.Guest = user;
                    }
                }
            }
        }

        public void Delete(AccommodationReservation accommodationReservation)
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

        public IEnumerable<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }

        public AccommodationReservation GetById(int id)
        {
            foreach (AccommodationReservation accommodationReservation in _accommodationReservations)
            {
                if (accommodationReservation.Id == id)
                {
                    return accommodationReservation; ;
                }
            }
            return null;
        }

        public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }

        public AccommodationReservation Save(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations = _serializer.FromCSV(FilePath);
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
            return accommodationReservation;
        }

        public void SaveAll(IEnumerable<AccommodationReservation> entities)
        {
            throw new NotImplementedException();
        }

        public List<AccommodationReservation> GetUnrated(IEnumerable<AccommodationGuestRating> ratings)
        {
            List<AccommodationReservation> unrated = new List<AccommodationReservation>(_accommodationReservations);
            foreach (var accommodationReservation in _accommodationReservations)
            {
                foreach (var rating in ratings)
                {
                    if (accommodationReservation.Id == rating.AccommodationReservationId)
                    {
                        unrated.Remove(accommodationReservation);
                    }
                }
            }

            return unrated;
        }
    }
}
