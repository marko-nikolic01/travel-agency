using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourReservation: Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourOccurrenceId { get; set; }
        public int UserId { get; set; }
        public TourReservation() { }

        public TourReservation(int tourOccurrenceId, int userId)
        {
            TourOccurrenceId = tourOccurrenceId;
            UserId = userId;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourOccurrenceId.ToString(), UserId.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourOccurrenceId = int.Parse(values[1]);
            UserId = int.Parse(values[2]);
        }
    }
}
