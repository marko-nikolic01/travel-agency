using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class Tour : Serializer.ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }
        public String Language { get; set; }
        public int MaxGuestNumber { get; set; }
        public int Duration { get; set; }
        public Location Location { get; set; }
        public List<String> KeyPoints { get; set; }
        public List<DateTime> DateTimes { get; set; }
        public List<String> Images { get; set; }

        public Tour(int id, string name, string description, string language, int maxGuestNumber, 
            int duration, Location location, List<string> keyPoints, List<DateTime> dateTimes, List<string> images)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxGuestNumber = maxGuestNumber;
            Duration = duration;
            Location = location;
            KeyPoints = keyPoints;
            DateTimes = dateTimes;
            Images = images;
        }

        public Tour()
        {
            Name = "";
            Description = "";
            Language = "";
            Location = new Location();
            KeyPoints = new List<string>();
            DateTimes = new List<DateTime>();
            Images = new List<string>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Description, Language, MaxGuestNumber.ToString(), Duration.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Description = values[2];
            Language = values[3];
            MaxGuestNumber = int.Parse(values[4]);
            Duration = int.Parse(values[5]);
        }
    }
}
