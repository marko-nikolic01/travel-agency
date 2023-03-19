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
    public class TourOccurrenceRepository : ISubject, IRepository<TourOccurrence>
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

        public int NextId()
        {
            if (tourOccurrences.Count == 0)
            {
                return 1;
            }
            return tourOccurrences[tourOccurrences.Count - 1].Id + 1;
        }

        public List<TourOccurrence> GetAll()
        {
            return tourOccurrences;
        }
        public List<TourOccurrence> GetTodays(User activeGuide)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date.Equals(DateTime.Now.Date) && tourOccurrence.GuideId == activeGuide.Id)
                {
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }
        public TourOccurrence SaveTourOccurrence(TourOccurrence tourOccurrence, User activeGuide)
        {
            tourOccurrence.Id = NextId();
            tourOccurrence.GuideId = activeGuide.Id;
            tourOccurrences.Add(tourOccurrence);
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
            return tourOccurrence;
        }

        public void UpdateTourOccurrence(TourOccurrence tourOccurrence)
        {
            TourOccurrence oldTourOccurrence = tourOccurrences.Find(t => t.Id == tourOccurrence.Id);
            oldTourOccurrence.CurrentState = tourOccurrence.CurrentState;
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

        public TourOccurrence GetById(int id)
        {
            throw new NotImplementedException();
        }

        public TourOccurrence Save(TourOccurrence entity)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<TourOccurrence> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TourOccurrence entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
