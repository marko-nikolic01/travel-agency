using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public enum ResponseStatus { NotAnsweredYet, Accepted, Declined}
    public class TourOccurrenceAttendance : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourOccurrenceId { get; set; }
        public int KeyPointId { get; set; }
        public int GuestId { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public TourOccurrenceAttendance(int tourOccurrenceId, int keyPointId, int guestId)
        {
            TourOccurrenceId = tourOccurrenceId;
            KeyPointId = keyPointId;
            GuestId = guestId;
            ResponseStatus = ResponseStatus.NotAnsweredYet;
        }

        public TourOccurrenceAttendance()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourOccurrenceId.ToString(), KeyPointId.ToString(), GuestId.ToString(), ((int)ResponseStatus).ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourOccurrenceId = int.Parse(values[1]);
            KeyPointId = int.Parse(values[2]);
            GuestId = int.Parse(values[3]);
            ResponseStatus = (ResponseStatus)Convert.ToInt32(values[4]);
        }
    }
}
