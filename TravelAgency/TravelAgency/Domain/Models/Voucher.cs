using System;
using System.Globalization;

namespace TravelAgency.Domain.Models
{
    public class Voucher : Serializer.ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int GuideId { get; set; }
        public string VoucherString { get; set; }
        public bool IsUsed { get; set; }
        public DateTime Deadline { get; set; }
        public int TourOccurrenceId { get; set; }
        public int CanceledTourOccurrenceId { get; set; }
        public Voucher()
        {
        }

        public Voucher(int id, int guestId, int guideId, DateTime dateTime, int canceledTourOccurrenceId)
        {
            Id = id;
            GuestId = guestId;
            GuideId = guideId;
            IsUsed = false;
            Deadline = dateTime;
            TourOccurrenceId = -1;
            CanceledTourOccurrenceId = canceledTourOccurrenceId;
        }

        public void BuildVoucherString()
        {
            VoucherString = Id + ". Deadline - " + Deadline.ToString("dd-MM-yyyy");
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), GuideId.ToString(), IsUsed.ToString(), Deadline.ToString("dd-MM-yyyy HH-mm"), TourOccurrenceId.ToString(), CanceledTourOccurrenceId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            GuideId = int.Parse(values[2]);
            IsUsed = bool.Parse(values[3]);
            Deadline = DateTime.ParseExact(values[4], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
            TourOccurrenceId = int.Parse(values[5]);
            CanceledTourOccurrenceId = int.Parse(values[6]);
        }
    }
}
