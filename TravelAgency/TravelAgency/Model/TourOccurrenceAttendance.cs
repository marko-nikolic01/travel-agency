using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourOccurrenceAttendance : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourOccurrenceId { get; set; }
        public int KeyPointId { get; set; }
        public int UserId { get; set; }

        public TourOccurrenceAttendance(int tourOccurrenceId, int keyPointId, int userId)
        {
            TourOccurrenceId = tourOccurrenceId;
            KeyPointId = keyPointId;
            UserId = userId;
        }

        public TourOccurrenceAttendance()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourOccurrenceId.ToString(), KeyPointId.ToString(), UserId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourOccurrenceId = int.Parse(values[1]);
            KeyPointId = int.Parse(values[2]);
            UserId = int.Parse(values[3]);
        }
    }
}
