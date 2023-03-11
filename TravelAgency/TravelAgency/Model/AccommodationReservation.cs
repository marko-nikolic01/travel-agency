using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int AccomodationId { get; set; }
        public DateOnly ReservationDate { get; set; }

        public AccommodationReservation()
        {
            Id = -1;
            AccomodationId = -1;
            ReservationDate = new DateOnly();
        }

        public AccommodationReservation(int id, int accommodationId, DateOnly reservationDate)
        {
            Id = id;
            AccomodationId = accommodationId;
            ReservationDate = reservationDate;
        }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }
    }
}
