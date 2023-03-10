using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class Image
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string Link { get; set; }
    }
}
