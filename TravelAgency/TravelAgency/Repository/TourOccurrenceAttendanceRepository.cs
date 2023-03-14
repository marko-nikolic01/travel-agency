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
    public class TourOccurrenceAttendanceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourOccurrenceAttendances.csv";
        private readonly Serializer<TourOccurrenceAttendance> _serializer;
        private List<TourOccurrenceAttendance> tourOccurrenceAttendances;

        public TourOccurrenceAttendanceRepository()
        {
            _serializer = new Serializer<TourOccurrenceAttendance>();
            tourOccurrenceAttendances = _serializer.FromCSV(FilePath);
        }

        private int GetNewId()
        {
            if (tourOccurrenceAttendances.Count == 0)
            {
                return 1;
            }
            return tourOccurrenceAttendances[tourOccurrenceAttendances.Count - 1].Id + 1;
        }


        public List<TourOccurrenceAttendance> GetTourOccurrenceAttendances()
        {
            return tourOccurrenceAttendances;
        }
        public void SaveTourOccurrenceAttendaces(TourOccurrenceAttendance tourOccurrenceAttendance)
        {
            tourOccurrenceAttendance.Id = GetNewId();
            tourOccurrenceAttendances.Add(tourOccurrenceAttendance);
            _serializer.ToCSV(FilePath, tourOccurrenceAttendances);
        }
    }
}
