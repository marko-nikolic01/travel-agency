using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationCancellation : ISerializable
    {
        public int Id { get; set; }
        public int AccomodationReservationId { get; set; }
        public DateOnly CancellationDate { get; set; }

        public AccommodationCancellation()
        {
            Id = -1;
            AccomodationReservationId = -1;
            CancellationDate = new DateOnly();
        }

        public AccommodationCancellation(int id, int accomodationReservationId, DateOnly cancellationDate)
        {
            Id = id;
            AccomodationReservationId = accomodationReservationId;
            CancellationDate = cancellationDate;
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
