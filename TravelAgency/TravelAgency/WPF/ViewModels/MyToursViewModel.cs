using System.Collections.Generic;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class MyToursViewModel
    {
        public List<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public int currentGuestId;
        public string ActiveTourString { get; set; }
        public MyToursViewModel(int guestId)
        {
            TourOccurrenceService tourOccurrenceService = new TourOccurrenceService();
            currentGuestId = guestId;
            TourOccurrences = tourOccurrenceService.GetFinishedOccurrencesForGuest(currentGuestId);
            ActiveTourString = tourOccurrenceService.GetActiveTour(currentGuestId);
        }
        public bool CanTourBeRated()
        {
            TourRatingService ratingService = new TourRatingService();
            if (SelectedTourOccurrence != null)
            {
                if (ratingService.IsTourNotRated(currentGuestId, SelectedTourOccurrence.Id))
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
