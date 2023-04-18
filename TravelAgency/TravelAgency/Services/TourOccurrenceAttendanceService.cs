using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class TourOccurrenceAttendanceService
    {
        public ITourOccurrenceAttendanceRepository IAttendanceRepository { get; set; }
        public TourOccurrenceAttendanceService()
        {
            IAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
        }
        public int GetGuestsNumberByTour(int id)
        {
            return IAttendanceRepository.GetCountForTour(id);
        }
        public int GetGuestsUnder18(int id)
        {
            int result = 0;
            UserRepository userRepository = new UserRepository();
            var guests = IAttendanceRepository.GetGuestsByTourOccurrenceId(id);
            foreach (var guestId in guests)
            {
                var guest = userRepository.GetUsers().Find(g => g.Id == guestId);
                if (guest != null)
                {
                    if (DateTime.Now.Year - guest.BirthDay.Year < 18)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public int GetGuest18to50(int id)
        {
            int result = 0;
            UserRepository userRepository = new UserRepository();
            var guests = IAttendanceRepository.GetGuestsByTourOccurrenceId(id);
            foreach (var guestId in guests)
            {
                var guest = userRepository.GetUsers().Find(g => g.Id == guestId);
                if (guest != null)
                {
                    if (DateTime.Now.Year - guest.BirthDay.Year > 18 && DateTime.Now.Year - guest.BirthDay.Year < 50)
                    {
                        result++;
                    }
                }
            }
            return result;
        }
        public int GetGuestsAbove50(int id)
        {
            int result = 0;
            UserRepository userRepository = new UserRepository();
            var guests = IAttendanceRepository.GetGuestsByTourOccurrenceId(id);
            foreach (var guestId in guests)
            {
                var guest = userRepository.GetUsers().Find(g => g.Id == guestId);
                if (guest != null)
                {
                    if (DateTime.Now.Year - guest.BirthDay.Year > 50)
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
    }
}
