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
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public TourReservationService() 
        {
            ITourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
        }
        public void SaveTourReservation(TourReservation tourReservation)
        {
            ITourReservationRepository.Save(tourReservation);
        }
        public bool IsTourReserved(int guestId, int tourOccurrenceId)
        {
            return ITourReservationRepository.IsTourReserved(guestId, tourOccurrenceId);
        }
        public List<TourOccurrence> GetAlternativeTours(TourOccurrence occurrence, int guestId)
        {
            
            List<TourOccurrence> result = ITourOccurrenceRepository.GetOfferedToursByLocation(occurrence.Tour.Location);
            result.Remove(occurrence);
            result = RemoveReservedTours(result, guestId);
            return result;
        }
        private List<TourOccurrence> RemoveReservedTours(List<TourOccurrence> occurrences, int guestId)
        {
            TourOccurrence tourOccurrence = null;
            foreach (TourReservation reservation in ITourReservationRepository.GetAll())
            {
                if (reservation.UserId == guestId && (tourOccurrence = occurrences.Find(t => t.Id == reservation.TourOccurrenceId)) != null)
                {
                    occurrences.Remove(tourOccurrence);
                }
            }
            return occurrences;
        }
    }
}
