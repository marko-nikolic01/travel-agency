using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.Services
{
    public class TourOccurrenceAttendanceService
    {
        public TourOccurrenceAttendanceService()
        {
        }
        public int GetGuestsNumberByTour(int id)
        {
            TourOccurrenceAttendanceRepository attendanceRepository = new TourOccurrenceAttendanceRepository();
            return attendanceRepository.GetCountForTour(id);
        }
        public int GetGuestsUnder18(int id)
        {
            int result = 0;
            TourOccurrenceAttendanceRepository attendanceRepository = new TourOccurrenceAttendanceRepository();
            UserRepository userRepository = new UserRepository();
            var guests = attendanceRepository.GetGuestsByTourOccurrenceId(id);
            foreach(var guestId in guests)
            {
                var guest = userRepository.GetUsers().Find(g => g.Id == guestId);
                if(guest != null)
                {
                    if(DateTime.Now.Year - guest.BirthDay.Year < 18)
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
            TourOccurrenceAttendanceRepository attendanceRepository = new TourOccurrenceAttendanceRepository();
            UserRepository userRepository = new UserRepository();
            var guests = attendanceRepository.GetGuestsByTourOccurrenceId(id);
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
            TourOccurrenceAttendanceRepository attendanceRepository = new TourOccurrenceAttendanceRepository();
            UserRepository userRepository = new UserRepository();
            var guests = attendanceRepository.GetGuestsByTourOccurrenceId(id);
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
    }
}
