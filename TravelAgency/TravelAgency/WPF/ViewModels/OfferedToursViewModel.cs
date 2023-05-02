using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OfferedToursViewModel : INotifyPropertyChanged, IObserver
    {
        private string duration;
        private string language;
        private string city;
        private string country;
        private string guests;
        private bool helpClicked;
        private bool dataGridClicked;
        private ObservableCollection<TourOccurrence> occurrences;
        TourOccurrenceService occurrenceService;
        public ObservableCollection<TourOccurrence> TourOccurrences
        {
            get { return occurrences; }
            set { occurrences = value; OnPropertyChanged(); }
        }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public ButtonCommandNoParameter SearchHelpCommand { get; set; }
        public ButtonCommandNoParameter DataGridHelpCommand { get; set; }
        public bool SearchHelpClicked
        {
            get { return helpClicked; }
            set { helpClicked = value; OnPropertyChanged(); }
        }
        public bool DataGridHelpClicked
        {
            get { return dataGridClicked; }
            set { dataGridClicked = value; OnPropertyChanged(); }
        }
        public string Country{
            get { return country; }
            set { country = value; OnPropertyChanged(); }
        }
        public string City{
            get { return city; }
            set { city = value; OnPropertyChanged(); }
        }
        public string Language{
            get { return language; }
            set { language = value; OnPropertyChanged(); }
        }
        public string Duration{
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }
        public string Guests{
            get { return guests; }
            set { guests = value; OnPropertyChanged(); }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public int currentGuestId;
        public OfferedToursViewModel(int id)
        {
            occurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(occurrenceService.GetOfferedTours());
            currentGuestId = id;
            occurrenceService.Subscribe(this);
            SearchHelpClicked = false;
            SearchHelpCommand = new ButtonCommandNoParameter(HelpClick);
            DataGridHelpCommand = new ButtonCommandNoParameter(DataGridClick);
        }
        private void HelpClick()
        {
            SearchHelpClicked = !SearchHelpClicked;
        }
        private void DataGridClick()
        {
            DataGridHelpClicked = !DataGridHelpClicked;
        }
        public bool CanTourBeReserved()
        {
            TourReservationService reservationService = new TourReservationService();
            if (SelectedTourOccurrence == null)
            {
                MessageBox.Show("You must choose a tour.");
                return false;
            }
            else if (SelectedTourOccurrence.Guests.Count == SelectedTourOccurrence.Tour.MaxGuestNumber)
            {
                /*AlternativeTours alternativeTours = new AlternativeTours(TourOccurrences, SelectedTourOccurrence.Id, SelectedTourOccurrence.Tour.Location, ActiveGuest, TourOccurrenceRepository);
                alternativeTours.Show();*/
                return false;
            }
            else if (reservationService.IsTourReserved(currentGuestId, SelectedTourOccurrence.Id))
            {
                MessageBox.Show("You already have reservation for this tour.");
                return false;
            }
            else
                return true;
        }
        public void Search()
        {
            ConvertFromNullToEmptyString();
            if (IsTextBoxEmpty(City) || IsTextBoxEmpty(Country) || IsTextBoxEmpty(Language) || IsTextBoxEmpty(Duration) || IsTextBoxEmpty(Guests))
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
            toursList = toursList.Where(x => (x.Tour.Location.City.ToLower().Contains(City) || tbCityEmpty) &&
                                        (x.Tour.Location.Country.ToLower().Contains(Country) || tbCountryEmpty) &&
                                        (x.Tour.Language.ToLower().Contains(Language) || tbLanguageEmpty) &&
                                        (x.Tour.Duration.ToString().Contains(Duration) || tbDurEmpty) &&
                                        ((x.Tour.MaxGuestNumber - x.Guests.Count) >= numOfGuests)).ToList();
            TourOccurrences = new ObservableCollection<TourOccurrence>(toursList);
        }
        private bool IsTextBoxEmpty(string text)
        {
            return text == "";
        }
        private void ConvertFromNullToEmptyString()
        {
            if (City == null)
                City = "";
            if (Country == null)
                Country = "";
            if (Language == null)
                Language = "";
            if (Duration == null)
                Duration = "";
            if (Guests == null)
                guests = "";
        }
        public void Update()
        {
            TourOccurrences.Clear();
            TourOccurrences = new ObservableCollection<TourOccurrence>(occurrenceService.GetOfferedTours());
        }
    }
}
