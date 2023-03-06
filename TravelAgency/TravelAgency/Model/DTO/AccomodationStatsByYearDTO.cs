using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model.DTO
{
    public class AccomodationStatsByYearDTO
    {
        public int Year { get; set; }
        public int ReservationsCanceled { get; set; }
        public int ReservationsPostponed { get; set; }
        public int RenovationReccomendations { get; set; }

        public AccomodationStatsByYearDTO()
        {
            Year = 0;
            ReservationsCanceled = 0;
            ReservationsPostponed = 0;
            RenovationReccomendations = 0;
        }

        public AccomodationStatsByYearDTO(int year, int reservationsCanceled, int reservationsPostponed, int renovationReccomendations)
        {
            Year = year;
            ReservationsCanceled = reservationsCanceled;
            ReservationsPostponed = reservationsPostponed;
            RenovationReccomendations = renovationReccomendations;
        }
    }
}
