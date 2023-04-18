using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetTourReservations();

        public void SaveTourReservation(TourReservation tourReservation);

        public bool IsTourReserved(int guestId, int tourOccurrenceId);
    }
}
