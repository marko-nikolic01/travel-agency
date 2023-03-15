using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccomodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        public int NumberOfGuests { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }


        public AccommodationReservation()
        {
            Id = -1;
            AccomodationId = -1;
            GuestId = -1;
            NumberOfGuests = -1;
            StartDate = new DateOnly();
            EndDate = new DateOnly();
        }

        public AccommodationReservation(int id, int accommodationId, int guestId, int numberOfGuests, DateOnly startDate, DateOnly endDate)
        {
            Id = id;
            AccomodationId = accommodationId;
            GuestId = guestId;
            NumberOfGuests = numberOfGuests;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccomodationId.ToString(),
                GuestId.ToString(),
                NumberOfGuests.ToString(),
                StartDate.ToString(),
                EndDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccomodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            NumberOfGuests = Convert.ToInt32(values[3]);
            StartDate = DateOnly.Parse(values[4]);
            EndDate = DateOnly.Parse(values[5]);
        }
    }
}
