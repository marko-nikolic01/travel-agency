using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TravelAgency.Domain.Models
{
    public class WonVoucherNotification : ISerializable
    {
        public int GuestId { get; set; }
        public int Year { get; set; }
        public bool Seen { get; set; }
        public WonVoucherNotification() { }
        public string[] ToCSV()
        {
            string[] csvValues = { GuestId.ToString(), Year.ToString(), Seen.ToString()};
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            GuestId = int.Parse(values[0]);
            Year = int.Parse(values[1]);
            Seen = bool.Parse(values[2]);
        }

    }
}
