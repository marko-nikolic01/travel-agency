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

        public TourOccurrenceRepository(PhotoRepository photoRepository, LocationRepository locationRepository, TourRepository tourRepository, TourReservationRepository reservationRepository, UserRepository userRepository)
        {
            _serializer = new Serializer<TourOccurrence>();
            tourOccurrences = _serializer.FromCSV(FilePath);
            observers = new List<IObserver>();
            //dodati jos da se ovde stvaraju repozitorijumi, da se ne prosledjuju
            LinkTourLocations(locationRepository, tourRepository);
            LinkTourPhotos(photoRepository, tourRepository);
            LinkTourOccurrences(tourRepository);
            LinkTourGuests(reservationRepository, userRepository);

        }

        private void LinkTourGuests(TourReservationRepository reservationRepository, UserRepository userRepository)
        {
            foreach (TourReservation tourReservation in reservationRepository.GetTourReservations())
            {
                TourOccurrence tourOccurrence = tourOccurrences.Find(x => x.Id == tourReservation.TourOccurrenceId);
                User guest = userRepository.GetUsers().Find(x => x.Id == tourReservation.UserId);
                tourOccurrence.Guests.Add(guest);
            }
        }

        private void LinkTourOccurrences(TourRepository tourRepository)
        {
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                Tour tour = tourRepository.GetAll().Find(t => t.Id == tourOccurrence.TourId);
                if (tour != null)
                {
                    tourOccurrence.Tour = tour;
                }
            }
        }

        private static void LinkTourPhotos(PhotoRepository photoRepository, TourRepository tourRepository)
        {
            foreach (Photo photo in photoRepository.GetAll())
            {
                Tour tour = tourRepository.GetAll().Find(t => t.Id == photo.TourId);
                if (tour != null)
                {
                    tour.Photos.Add(photo);
                }
            }
        }

        private static void LinkTourLocations(LocationRepository locationRepository, TourRepository tourRepository)
        {
            foreach (var tour in tourRepository.GetAll())
            {
                Location location = locationRepository.GetAll().Find(l => l.Id == tour.LocationId);
                if (location != null)
                {
                    tour.Location = location;
                }
            }
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
