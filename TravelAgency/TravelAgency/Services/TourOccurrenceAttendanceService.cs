using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
