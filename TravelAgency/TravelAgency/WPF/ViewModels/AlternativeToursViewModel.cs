using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class AlternativeToursViewModel
    {
        public string AlternativeTour { get; set; }
        public List<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private string city;
        private string country;
        public int CurrentGuestId;
        public AlternativeToursViewModel(TourOccurrence tourOccurrence, int guestId) 
        {
            CurrentGuestId = guestId;
            TourReservationService reservationService = new TourReservationService();
            TourOccurrences = reservationService.GetAlternativeTours(tourOccurrence, guestId);
            city = tourOccurrence.Tour.Location.City;
            country = tourOccurrence.Tour.Location.Country;
            BuildAlternativeTourString();
        }
        private void BuildAlternativeTourString()
        {
            if(TourOccurrences.Count == 0)
            {
                AlternativeTour = "There is no alternative tours in " + country + ", " +city+ " at the moment";
            }
            else
            {
                AlternativeTour = "You can reserve this alternative tours in " + country + ", " + city;
            }
        }
        public bool CanReserve()
        {
            return SelectedTourOccurrence != null;
        }
    }
}
