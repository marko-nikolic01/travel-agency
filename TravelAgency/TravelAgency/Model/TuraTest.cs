using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TuraTest
    {
        public string Location { get; set; }
        public int Duration { get; set; }

        public TuraTest()
        {
            Location = string.Empty;
            Duration = 0;
        }

        public TuraTest(string location, int duration)
        {
            Location = location;
            Duration = duration;
        }
    }
}
