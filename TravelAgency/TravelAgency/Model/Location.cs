using System;
using TravelAgency.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TravelAgency.Model
{
    public class Location : Serializer.ISerializable
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string FullName { get; set; }

        public Location(int id, string city, string country, string fullName)
        {
            Id = id;
            City = city;
            Country = country;
            FullName = fullName;
        }

        public Location()
        {
            Country = "";
            City = "";
            FullName = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), City, Country };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
        }
    }
}
