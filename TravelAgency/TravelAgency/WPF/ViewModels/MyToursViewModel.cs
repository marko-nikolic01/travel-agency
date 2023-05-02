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
        private bool dataGridClicked;
        private bool activeTourClicked;
        public bool DataGridHelpClicked
        {
            get { return dataGridClicked; }
            set { dataGridClicked = value; OnPropertyChanged(); }
        }
        public bool ActiveTourHelpClicked
        {
            get { return activeTourClicked; }
            set { activeTourClicked = value; OnPropertyChanged(); }
        }
        public ButtonCommandNoParameter DataGridHelpCommand { get; set; }
        public ButtonCommandNoParameter ActiveTourHelpCommand { get; set; }
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
            DataGridHelpCommand = new ButtonCommandNoParameter(DataGridClick);
            ActiveTourHelpCommand = new ButtonCommandNoParameter(ActiveTourClick);
        }
        private void DataGridClick()
        {
            DataGridHelpClicked = !DataGridHelpClicked;
        }
        private void ActiveTourClick()
        {
            ActiveTourHelpClicked = !ActiveTourHelpClicked;
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
