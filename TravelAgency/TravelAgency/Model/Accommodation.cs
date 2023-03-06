using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public enum AccommodationType { APARTMENT, HOUSE, HUT }

    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDays { get; set; }
        public int DaysToCancel { get; set; }

        public Location Location { get; set; }
        public List<string> Images { get; set; }
        public List<AccommodationReservation> Reservations { get; set; }
        public List<AccommodationCancellation> Cancellations { get; set; }

        public Accommodation()
        {
            Id = -1;
            Name = "";
            LocationId = -1;
            Type = AccommodationType.APARTMENT;
            MaxGuests = -1;
            MinDays = -1;
            DaysToCancel = -1;

            Location = new Location();
            Images = new List<string>();
        }

        public Accommodation(int id, string name, int locationId, AccommodationType type, int maxGuests, int minDays, int daysToCancel)
        {
            Id = id;
            Name = name;
            LocationId = locationId;
            Type = type;
            MaxGuests = maxGuests;
            MinDays = minDays;
            DaysToCancel = daysToCancel;

            Location = new Location();
            Images = new List<string>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name, 
                LocationId.ToString(),
                Type.ToString(),
                MaxGuests.ToString(),
                MinDays.ToString(),
                DaysToCancel.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = int.Parse(values[1]);
            LocationId = int.Parse(values[2]);
            Type = int.Parse(values[3]);
            MaxGuests = int.Parse(values[4]);
            MinDays = int.Parse(values[5]);
            DaysToCancel = int.Parse(values[6]);
        }
    }
}
