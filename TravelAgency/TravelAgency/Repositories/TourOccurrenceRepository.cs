using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourOccurrenceRepository : ISubject, ITourOccurrenceRepository
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

        public TourOccurrenceRepository(TourRepository tourRepository)
        {
            _serializer = new Serializer<TourOccurrence>();
            tourOccurrences = _serializer.FromCSV(FilePath);
            LinkTourOccurrences(tourRepository);
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
        public List<TourOccurrence> GetTodays(int activeGuideId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date.Equals(DateTime.Now.Date) && tourOccurrence.GuideId == activeGuideId && tourOccurrence.IsDeleted == false)
                {
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetUpcomings(int activeGuideId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date > DateTime.Now.Date && tourOccurrence.GuideId == activeGuideId && tourOccurrence.IsDeleted == false)
                {
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetOffered()
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences)
            {
                if (tourOccurrence.DateTime.Date >= DateTime.Now.Date && tourOccurrence.CurrentState != CurrentState.Ended && tourOccurrence.IsDeleted == false)
                {
                    tourOccurrence.MakeDetailedRowString();
                    result.Add(tourOccurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetFinishedOccurrencesForGuide(int guideId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence occurrence in tourOccurrences)
            {
                if (occurrence.CurrentState == CurrentState.Ended && occurrence.GuideId == guideId)
                {
                    result.Add(occurrence);
                }
            }
            return result;
        }

        public List<TourOccurrence> GetFinishedOccurrencesForGuideByYear(int guideId, int year)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence occurrence in tourOccurrences)
            {
                if (occurrence.CurrentState == CurrentState.Ended && occurrence.GuideId == guideId && occurrence.DateTime.Year == year)
                {
                    result.Add(occurrence);
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
            oldTourOccurrence.FreeSpots = tourOccurrence.FreeSpots;
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
            TourOccurrence tourOccurrence = tourOccurrences.Find(t => t.Id == id);
            return tourOccurrence;
        }

        public void Delete(TourOccurrence tourOccurrence)
        {
            TourOccurrence oldTourOccurrence = tourOccurrences.Find(t => t.Id == tourOccurrence.Id);
            if (oldTourOccurrence == null)
            {
                return;
            }
            oldTourOccurrence.IsDeleted = true;
            _serializer.ToCSV(FilePath, tourOccurrences);
            NotifyObservers();
        }

    }
}
