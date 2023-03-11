using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TravelAgency.Model
{
    public enum AccommodationType { APARTMENT, HOUSE, HUT }
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public int LocationId { get; set; }
        public AccommodationType Type { get; set; }
        public int MaxGuests { get; set; }
        public int MinDays { get; set; }
        public int DaysToCancel { get; set; }

        public User Owner { get; set; }
        public Location Location { get; set; }
        public List<Image> Images { get; set; }

        public Accommodation()
        {
            Id = -1;
            Name = "";
            OwnerId = -1;
            LocationId = -1;
            Type = AccommodationType.APARTMENT;
            MaxGuests = -1;
            MinDays = -1;
            DaysToCancel = -1;

            Images = new List<Image>();
        }

        public Accommodation(int id, string name, int ownerId, int locationId, AccommodationType type, int maxGuests, int minDays, int daysToCancel)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            LocationId = locationId;
            Type = type;
            MaxGuests = maxGuests;
            MinDays = minDays;
            DaysToCancel = daysToCancel;

            Images = new List<Image>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            {
                Id.ToString(),
                Name,
                OwnerId.ToString(),
                LocationId.ToString(),
                Convert.ToInt32(Type).ToString(),
                MaxGuests.ToString(),
                MinDays.ToString(),
                DaysToCancel.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            OwnerId = int.Parse(values[2]);
            LocationId = int.Parse(values[3]);
            Type = (AccommodationType)Convert.ToInt32(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinDays = Convert.ToInt32(values[6]);
            DaysToCancel= Convert.ToInt32(values[7]);
        }
    }
}
