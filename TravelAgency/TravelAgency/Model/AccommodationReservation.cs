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
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public AccommodationReservation()
        {
            Id = -1;
            AccomodationId = -1;
            StartDate = new DateOnly();
            EndDate = new DateOnly();
        }

        public AccommodationReservation(int id, int accommodationId, DateOnly reservationStartDate, DateOnly reservationEndDate)
        {
            Id = id;
            AccomodationId = accommodationId;
            StartDate = reservationStartDate;
            EndDate = reservationEndDate;
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
