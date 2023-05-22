using System;
using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;

namespace TravelAgency.Services
{
    public class TourOccurrenceAttendanceService
    {
        public ITourOccurrenceAttendanceRepository IAttendanceRepository { get; set; }
        public IUserRepository IUserRepository { get;set; }
        public IKeyPointRepository IKeyPointRepository { get; set; }
        public TourOccurrenceAttendanceService()
        {
            IAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            IKeyPointRepository = Injector.Injector.CreateInstance<IKeyPointRepository>();
        }
        public int GetGuestsNumberByTour(int id)
        {
            return IAttendanceRepository.GetCountForTour(id);
        }
        public int GetGuestsUnder18(int id)
        {
            int result = 0;
            var guests = IAttendanceRepository.GetGuestsByTourOccurrenceId(id);
            foreach (var guestId in guests)
            {
                var guest = IUserRepository.GetUsers().Find(g => g.Id == guestId);
                if (guest != null)
                {
                    if (DateTime.Now.Date.Year - guest.BirthDay.Year < 18)
                    {
                        result++;
                    }
                    else if ((DateTime.Now.Year == guest.BirthDay.AddYears(18).Year && DateTime.Now.Month < guest.BirthDay.Month))
                    {
                        result++;
                    }
                    else if ((DateTime.Now.Year == guest.BirthDay.AddYears(18).Year) && DateTime.Now.Month == guest.BirthDay.Month && DateTime.Now.Day < guest.BirthDay.Day)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public int GetGuest18to50(int id)
        {
            var guests = IAttendanceRepository.GetGuestsByTourOccurrenceId(id);
            return guests.Count - GetGuestsUnder18(id) - GetGuestsAbove50(id);
        }
        public int GetGuestsAbove50(int id)
        {
            int result = 0;
            var guests = IAttendanceRepository.GetGuestsByTourOccurrenceId(id);
            foreach (var guestId in guests)
            {
                var guest = IUserRepository.GetUsers().Find(g => g.Id == guestId);
                if (guest != null)
                {
                    if (DateTime.Now.Year - guest.BirthDay.Year > 50)
                    {
                        result++;
                    }
                    else if ((DateTime.Now.Year == guest.BirthDay.AddYears(50).Year && DateTime.Now.Month > guest.BirthDay.Month))
                    {
                        result++;
                    }
                    else if ((DateTime.Now.Year == guest.BirthDay.AddYears(50).Year) && DateTime.Now.Month == guest.BirthDay.Month && DateTime.Now.Day > guest.BirthDay.Day)
                    {
                        result++;
                    }
                }
            }
            return result;
        }


        public TourOccurrenceAttendance GetAttendance(int guestId)
        {
            foreach (TourOccurrenceAttendance tourOccurrenceAttendance in IAttendanceRepository.GetAll())
            {
                if (tourOccurrenceAttendance.GuestId == guestId && tourOccurrenceAttendance.ResponseStatus == ResponseStatus.NotAnsweredYet
                    && tourOccurrenceAttendance.KeyPointId != -1)
                {
                    return tourOccurrenceAttendance;
                }
            }
            return null;
        }

        public void SaveAnswer(bool accepted, TourOccurrenceAttendance attendance)
        {
            if (accepted)
            {
                attendance.ResponseStatus = ResponseStatus.Accepted;
            }
            else
            {
                attendance.ResponseStatus = ResponseStatus.Declined;
            }
            IAttendanceRepository.UpdateTourOccurrenceAttendaces(attendance);

        }
        public List<TourOccurrenceAttendance> GetByTourOccurrenceId(int id)
        {
            return IAttendanceRepository.GetByTourOccurrenceId(id);
        }

        public void SaveOrUpdate(TourOccurrenceAttendance tourOccurrenceAttendance)
        {
            IAttendanceRepository.SaveOrUpdate(tourOccurrenceAttendance);
        }

        public string GetArrivalKeyPoint(int occurrenceId, int guestId)
        {
            TourOccurrenceAttendance attendance = IAttendanceRepository.GetByTourOccurrenceIdAndGuestId(occurrenceId, guestId);
            KeyPoint keyPoint = IKeyPointRepository.GetById(attendance.KeyPointId);
            return keyPoint.Name;
        }
        public int GetNumberOfGuestsAttendances(int guestId)
        {
            int number = 0;
            List<TourOccurrenceAttendance> tourOccurrenceAttendances = IAttendanceRepository.GetByGuestId(guestId);
            foreach(TourOccurrenceAttendance attendance in tourOccurrenceAttendances)
            {
                if(attendance.ResponseStatus == ResponseStatus.Accepted)
                    number++;
            }    
            return number;
        }
        public void Subscribe(IObserver observer)
        {
            IAttendanceRepository.Subscribe(observer);
        }
        public void NotifyObservers()
        {
            IAttendanceRepository.NotifyObservers();
        }
    }
}
