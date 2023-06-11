using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class SuperGuide : ISerializable
    {
        public int Id { get; set; }
        public User Guide { get; set; }
        public int GuidesId { get; set; }
        public DateTime EndDate { get; set; }

        public SuperGuide()
        {
            Id = -1;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuidesId.ToString(),
                EndDate.ToString("dd-MM-yyyy HH-mm")
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuidesId = Convert.ToInt32(values[1]);
            EndDate = DateTime.ParseExact(values[2], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
        }
    }
}
