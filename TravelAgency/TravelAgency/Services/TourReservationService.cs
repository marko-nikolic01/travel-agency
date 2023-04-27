using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class TourReservationService
    {
        public ITourReservationRepository ITourReservationRepository { get; set; }
        public TourReservationService() 
        {
            ITourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            //LinkTourGuests(ITourReservationRepository, IUserRepository);
        }
        public void SaveTourReservation(TourReservation tourReservation)
        {
            ITourReservationRepository.Save(tourReservation);
        }
        public bool IsTourReserved(int guestId, int tourOccurrenceId)
        {
            return ITourReservationRepository.IsTourReserved(guestId, tourOccurrenceId);
        }
    }
}
