using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourOccurrence
    {
        int Id { get; set; }
        int TourId { get; set; }
        Tour Tour { get; set; }
        public DateTime DateTime { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
    }
}
