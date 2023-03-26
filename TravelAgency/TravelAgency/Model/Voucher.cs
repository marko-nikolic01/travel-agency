using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace TravelAgency.Model
{
    public class Voucher : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int GuideId { get; set; }
        public bool IsUsed { get; set; }

        public Voucher()
        {
        }

        public Voucher(int id, int guestId, int guideId)
        {
            Id = id;
            GuestId = guestId;
            GuideId = guideId;
            IsUsed = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), GuideId.ToString(), IsUsed.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            GuideId = int.Parse(values[2]);
            IsUsed = bool.Parse(values[3]);
        }
    }
}
