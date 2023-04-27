using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OfferedToursViewModel : INotifyPropertyChanged, IObserver
    {
        private string duration;
        private string language;
        private string city;
        private string country;
        private string guests;
        TourOccurrenceService occurrenceService;
        public static ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public static ObservableCollection<TourOccurrence> FilteredTourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public ButtonCommandNoParameter ReserveTourCommand { get; set; }

        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged();
            }
        }
        public string Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged();
            }
        }
        public string Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }
        public string Guests
        {
            get { return guests; }
            set
            {
                guests = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private int currentGuestId;
        public OfferedToursViewModel(int id)
        {
            occurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(occurrenceService.GetOfferedTours());
            ReserveTourCommand = new ButtonCommandNoParameter(ReserveTour);
            currentGuestId = id;
            occurrenceService.Subscribe(this);            
        }
        private void ReserveTour()
        {
            TourReservationService reservationService = new TourReservationService();
            if (SelectedTourOccurrence == null)
            {
                MessageBox.Show("You must choose a tour.");
            }
            else if (SelectedTourOccurrence.Guests.Count == SelectedTourOccurrence.Tour.MaxGuestNumber)
            {
                /*AlternativeTours alternativeTours = new AlternativeTours(TourOccurrences, SelectedTourOccurrence.Id, SelectedTourOccurrence.Tour.Location, ActiveGuest, TourOccurrenceRepository);
                alternativeTours.Show();*/
            }
            else if (reservationService.IsTourReserved(currentGuestId, SelectedTourOccurrence.Id))
            {
                MessageBox.Show("You already have reservation for this tour.");
            }
            else
            {
                /*TourGuests tourGuests = new TourGuests(SelectedTourOccurrence, ActiveGuest);
                tourGuests.Show();*/
            }
        }
        public void Search()
        {
            if (City != "" || Country != "" || Language != "" || Duration != "" || Guests != "")
            {
                if (!IsValid())
                {
                    MessageBox.Show("Number of guests must be a non negative number");
                    Guests = "";
                    return;
                }
                FilterList(IsTextBoxEmpty(City), IsTextBoxEmpty(Country), IsTextBoxEmpty(duration), IsTextBoxEmpty(language), IsTextBoxEmpty(Guests));
            }
            else
            {
                TourOccurrences = new ObservableCollection<TourOccurrence>(occurrenceService.GetOfferedTours());
            }
        }
        private bool IsValid()
        {
            if (Guests == "")
            {
                return true;
            }
            int res;
            if (int.TryParse(Guests, out res))
            {
                if (res >= 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        private void FilterList(bool tbCityEmpty, bool tbCountryEmpty, bool tbDurEmpty, bool tbLanguageEmpty, bool tbNumOfGuestsEmpty)
        {
            List<TourOccurrence> toursList = new List<TourOccurrence>(occurrenceService.GetOfferedTours());
            int numOfGuests;
            if (!tbNumOfGuestsEmpty)
                numOfGuests = int.Parse(Guests);
            else
                numOfGuests = 0;
            TourOccurrences = (ObservableCollection<TourOccurrence>)toursList.Where(x => (x.Tour.Location.City.ToLower().Contains(City) || tbCityEmpty) &&
                                        (x.Tour.Location.Country.ToLower().Contains(Country) || tbCountryEmpty) &&
                                        (x.Tour.Language.ToLower().Contains(Language) || tbLanguageEmpty) &&
                                        (x.Tour.Duration.ToString().Contains(Duration) || tbDurEmpty) &&
                                        ((x.Tour.MaxGuestNumber - x.Guests.Count) >= numOfGuests));
        }
        private bool IsTextBoxEmpty(string text)
        {
            return text == null;
        }
        public void Update()
        {
            TourOccurrences.Clear();
            TourOccurrences = new ObservableCollection<TourOccurrence>(occurrenceService.GetOfferedTours());
        }
    }
}
