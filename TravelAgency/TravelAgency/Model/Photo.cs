using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class Photo : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string Link { get; set; }

        public Photo(int id, int tourId, string link)
        {
            Id = id;
            TourId = tourId;
            Link = link;
        }

        public Photo()
        {
            Link = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), Link };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourId = int.Parse(values[1]);
            Link = values[2];
        }
    }
}
