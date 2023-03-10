using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class TourOccurrenceRepository : ISubject
    {
        private const string FilePath = "../../../Resources/Data/tourOccurrences.csv";
        private readonly Serializer<TourOccurrence> _serializer;
        private List<TourOccurrence> tourOccurrences;
        private List<IObserver> observers;

        public TourOccurrenceRepository()
        {
            _serializer = new Serializer<TourOccurrence>();
            tourOccurrences = _serializer.FromCSV(FilePath);
            observers = new List<IObserver>();
        }

        private int GetNewId()
        {
            if (tourOccurrences.Count == 0)
            {
                return 1;
            }
            return tourOccurrences[tourOccurrences.Count - 1].Id + 1;
        }


        public List<TourOccurrence> GetTourOccurrences()
        {
            return tourOccurrences;
        }
        public void SaveTourOccurrences(TourOccurrence tourOccurrence)
        {
            tourOccurrence.Id = GetNewId();
            tourOccurrences.Add(tourOccurrence);
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }
}
