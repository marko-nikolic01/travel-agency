using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourOccurrence : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public Tour Tour { get; set; }
        public DateTime DateTime { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public List<Guest> Guests { get; set; }

        public TourOccurrence(int id, int tourId, Tour tour, DateTime dateTime, List<KeyPoint> keyPoints)
        {
            Id = id;
            TourId = tourId;
            Tour = tour;
            DateTime = dateTime;
            KeyPoints = keyPoints;
        }

        public TourOccurrence()
        {
            KeyPoints = new List<KeyPoint>();
            Guests = new List<Guest>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), DateTime.ToString("dd-MM-yyyy hh-mm")};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            DateTime = DateTime.ParseExact(values[2], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
        }
    }
}
