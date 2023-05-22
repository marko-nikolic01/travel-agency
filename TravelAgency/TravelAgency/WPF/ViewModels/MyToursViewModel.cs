using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class MyToursViewModel : INotifyPropertyChanged
    {
        public List<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public int currentGuestId;
        public string ActiveTourString { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
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
            if (ratingService.IsTourNotRated(currentGuestId, SelectedTourOccurrence.Id))
                return true;
            else
                return false;
        }
    }
}
