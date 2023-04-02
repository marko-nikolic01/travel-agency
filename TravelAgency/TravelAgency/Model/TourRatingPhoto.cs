using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourRatingPhoto : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int TourRatingId { get; set; }
        public string Link { get; set; }

        public TourRatingPhoto(int tourRatingId, string link)
        {
            TourRatingId = tourRatingId;
            Link = link;
        }

        public TourRatingPhoto()
        {
            Link = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourRatingId.ToString(), Link };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourRatingId = int.Parse(values[1]);
            Link = values[2];
        }
    }
}
