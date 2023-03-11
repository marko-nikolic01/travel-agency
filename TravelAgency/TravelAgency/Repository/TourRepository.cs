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
    public class TourRepository : ISubject
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";
        private readonly Serializer<Tour> _serializer;
        private List<Tour> tours;
        private List<IObserver> observers;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            tours = _serializer.FromCSV(FilePath);
            observers = new List<IObserver>();
        }

        private int GetNewId()
        {
            if(tours.Count == 0)
            {
                return 1;
            }
            return tours[tours.Count - 1].Id +1;
        }


        public List<Tour> GetTours()
        {
            return tours;
        }
        public void SaveTours(Tour tour)
        {
            tour.Id = GetNewId();
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
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
