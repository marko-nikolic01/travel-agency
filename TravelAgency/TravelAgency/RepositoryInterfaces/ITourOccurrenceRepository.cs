using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Repository;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface ITourOccurrenceRepository
    {
        public List<TourOccurrence> GetAll();
        public List<TourOccurrence> GetTodays(int activeGuideId);

        public List<TourOccurrence> GetUpcomings(int activeGuideId);

        public List<TourOccurrence> GetOffered();

        public List<TourOccurrence> GetFinishedOccurrencesForGuide(int guideId);

        public List<TourOccurrence> GetFinishedOccurrencesForGuideByYear(int guideId, int year);

        public TourOccurrence SaveTourOccurrence(TourOccurrence tourOccurrence, User activeGuide);

        public void UpdateTourOccurrence(TourOccurrence tourOccurrence);

        public void Subscribe(IObserver observer);

        public void Unsubscribe(IObserver observer);

        public void NotifyObservers();

        public TourOccurrence GetById(int id);

        public void Delete(TourOccurrence tourOccurrence);
    }
}
