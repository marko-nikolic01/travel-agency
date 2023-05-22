using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Repositories
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourReservations.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> tourReservations;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            tourReservations = _serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return tourReservations;
        }
        public List<TourReservation> GetAllForGuest(int guestId)
        {
            List<TourReservation> result = new List<TourReservation>();
            foreach(TourReservation reservation in tourReservations)
            {
                if(reservation.UserId == guestId)
                    result.Add(reservation);
            }
            return result;
        }
        private int GetNewId()
        {
            if (tourReservations.Count == 0)
            {
                return 1;
            }
            return tourReservations[tourReservations.Count - 1].Id + 1;
        }

        public void Save(TourReservation tourReservation)
        {
            tourReservation.Id = GetNewId();
            tourReservations.Add(tourReservation);
            _serializer.ToCSV(FilePath, tourReservations);
        }
        public bool IsTourReserved(int guestId, int tourOccurrenceId)
        {
            return tourReservations.Exists(x => x.UserId == guestId && x.TourOccurrenceId == tourOccurrenceId);
        }
    }
}
