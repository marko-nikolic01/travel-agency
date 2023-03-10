using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class KeyPoint : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourOccurrenceId { get; set; }
        public string Name { get; set; }
        public List<Guest> Guests { get; set; }

        public KeyPoint(int id, string name,  List<Guest> guests, int tourOccurrenceId)
        {
            Id = id;
            Name = name;
            Guests = guests;
            TourOccurrenceId = tourOccurrenceId;
        }

        public KeyPoint()
        {
            Name = "";
            Guests = new List<Guest>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourOccurrenceId.ToString(), Name };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourOccurrenceId = int.Parse(values[1]);
            Name = values[2];
        }
    }
}
