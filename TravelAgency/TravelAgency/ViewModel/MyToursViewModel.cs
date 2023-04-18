using System.Collections.Generic;
using TravelAgency.Model;
using TravelAgency.Services;

namespace TravelAgency.ViewModel
{
    public class MyToursViewModel
    {
        public List<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public int currentGuestId;
        public string ActiveTourString {get; set; }
        public MyToursViewModel(int guestId)
        {
            TourOccurrenceService tourOccurrenceService = new TourOccurrenceService();
            currentGuestId = guestId;
            TourOccurrences = tourOccurrenceService.GetFinishedOccurrencesForGuest(currentGuestId);
            ActiveTourString = tourOccurrenceService.GetActiveTour(currentGuestId);
        }
    }
}
