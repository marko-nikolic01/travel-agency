using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourReservationRepository
    {
        public List<TourReservation> GetAll();
        public List<TourReservation> GetAllForGuest(int guestId);

        public void Save(TourReservation tourReservation);

        public bool IsTourReserved(int guestId, int tourOccurrenceId);
    }
}
