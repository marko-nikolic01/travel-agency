using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Repositories;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourOccurrenceRepository
    {
        public List<TourOccurrence> GetAll();
        public List<TourOccurrence> GetTodays(int activeGuideId);

        public List<TourOccurrence> GetUpcomings(int activeGuideId);

        public List<TourOccurrence> GetOffered();

        public List<TourOccurrence> GetFinishedOccurrencesForGuide(int guideId);

        public List<TourOccurrence> GetFinishedOccurrencesForGuideByYear(int guideId, int year);
        public List<TourOccurrence> GetOfferedToursByLocation(Location location);

        public TourOccurrence SaveTourOccurrence(TourOccurrence tourOccurrence, User activeGuide);

        public void UpdateTourOccurrence(TourOccurrence tourOccurrence);

        public void Subscribe(IObserver observer);

        public void Unsubscribe(IObserver observer);

        public void NotifyObservers();

        public TourOccurrence GetById(int id);
        public TourOccurrence GetByTourId(int id);

        public int Delete(TourOccurrence tourOccurrence);
        public void UndoDelete(int canceledTour);
    }
}
