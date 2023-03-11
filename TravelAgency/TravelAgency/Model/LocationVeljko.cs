using System.Xml.Linq;
using System;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Location : ISerializable
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public Location()
        {
            Id = -1;
            Country = "";
            City = "";
        }

        public Location(int id, string country, string city)
        {
            Id = id;
            Country = country;
            City = city;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Country,
                City
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Country = values[1];
            City = values[2];
        }
    }
}
