﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourOccurrenceAttendanceRepository : ITourOccurrenceAttendanceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourOccurrenceAttendances.csv";
        private readonly Serializer<TourOccurrenceAttendance> _serializer;
        private List<TourOccurrenceAttendance> tourOccurrenceAttendances;
        private List<IObserver> observers;
        public TourOccurrenceAttendanceRepository()
        {
            _serializer = new Serializer<TourOccurrenceAttendance>();
            tourOccurrenceAttendances = _serializer.FromCSV(FilePath);
            observers = new List<IObserver>();
        }

        public int NextId()
        {
            if (tourOccurrenceAttendances.Count == 0)
            {
                return 1;
            }
            return tourOccurrenceAttendances[tourOccurrenceAttendances.Count - 1].Id + 1;
        }

        public List<TourOccurrenceAttendance> GetAll()
        {
            return tourOccurrenceAttendances;
        }
        public TourOccurrenceAttendance Save(TourOccurrenceAttendance tourOccurrenceAttendance)
        {
            tourOccurrenceAttendance.Id = NextId();
            tourOccurrenceAttendances.Add(tourOccurrenceAttendance);
            _serializer.ToCSV(FilePath, tourOccurrenceAttendances);
            return tourOccurrenceAttendance;
        }

        public void UpdateTourOccurrenceAttendaces(TourOccurrenceAttendance tourOccurrenceAttendance)
        {
            TourOccurrenceAttendance oldTourOccurrenceAttendance = tourOccurrenceAttendances.Find(t => t.Id == tourOccurrenceAttendance.Id);
            oldTourOccurrenceAttendance.ResponseStatus = tourOccurrenceAttendance.ResponseStatus;
            _serializer.ToCSV(FilePath, tourOccurrenceAttendances);
        }

        public List<TourOccurrenceAttendance> GetByTourOccurrenceId(int id)
        {
            List<TourOccurrenceAttendance> result = new List<TourOccurrenceAttendance>();
            foreach (TourOccurrenceAttendance tourOccurrenceAttendance in tourOccurrenceAttendances)
            {
                if (tourOccurrenceAttendance.TourOccurrenceId == id)
                {
                    result.Add(tourOccurrenceAttendance);
                }
            }
            return result;
        }

        public List<TourOccurrenceAttendance> GetByGuestId(int id)
        {
            List<TourOccurrenceAttendance> result = new List<TourOccurrenceAttendance>();
            foreach (TourOccurrenceAttendance tourOccurrenceAttendance in tourOccurrenceAttendances)
            {
                if (tourOccurrenceAttendance.GuestId == id)
                {
                    result.Add(tourOccurrenceAttendance);
                }
            }
            return result;
        }

        public List<int> GetGuestsByTourOccurrenceId(int id)
        {
            List<int> result = new List<int>();
            foreach (TourOccurrenceAttendance tourOccurrenceAttendance in tourOccurrenceAttendances)
            {
                if (tourOccurrenceAttendance.TourOccurrenceId == id && tourOccurrenceAttendance.ResponseStatus == ResponseStatus.Accepted)
                {
                    result.Add(tourOccurrenceAttendance.GuestId);
                }
            }
            return result;
        }

        public TourOccurrenceAttendance GetByTourOccurrenceIdAndGuestId(int TourOccurrenceId, int GuestId)
        {
            foreach (TourOccurrenceAttendance tourOccurrenceAttendance in tourOccurrenceAttendances)
            {
                if (tourOccurrenceAttendance.TourOccurrenceId == TourOccurrenceId && tourOccurrenceAttendance.GuestId == GuestId)
                {
                    return tourOccurrenceAttendance;
                }
            }
            return null;
        }

        public void SaveOrUpdate(TourOccurrenceAttendance attendance)
        {
            TourOccurrenceAttendance oldAttendance = tourOccurrenceAttendances.Find(k => k.TourOccurrenceId == attendance.TourOccurrenceId && k.GuestId == attendance.GuestId);
            if (oldAttendance != null)
            {
                oldAttendance.KeyPointId = attendance.KeyPointId;
            }
            else
            {
                attendance.Id = NextId();
                tourOccurrenceAttendances.Add(attendance);
            }
            _serializer.ToCSV(FilePath, tourOccurrenceAttendances);
        }

        public int GetCountForTour(int id)
        {
            int count = 0;
            foreach (TourOccurrenceAttendance attendance in tourOccurrenceAttendances)
            {
                if (attendance.TourOccurrenceId == id && attendance.ResponseStatus == ResponseStatus.Accepted)
                {
                    count++;
                }
            }
            return count;
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
