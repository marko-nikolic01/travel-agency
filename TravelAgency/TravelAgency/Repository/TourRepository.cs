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
    public class TourRepository : ISubject, IRepository<Tour>
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

        public int NextId()
        {
            if(tours.Count == 0)
            {
                return 1;
            }
            return tours[tours.Count - 1].Id +1;
        }

        public List<Tour> GetAll()
        {
            return tours;
        }

        public Tour Save(Tour tour)
        {
            tour.Id = NextId();
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
            NotifyObservers();
            return tour;
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

        public Tour GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveAll(IEnumerable<Tour> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Tour entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
