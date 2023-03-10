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
        List<string> Images { get; set; }

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
            throw new NotImplementedException();
        }

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
