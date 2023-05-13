using LiveCharts.Maps;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRequestFormViewModel : INotifyPropertyChanged
    {
        private string selectedCountry;
        private string selectedCity;
        private bool dateHelpClicked;
        private bool guestNumHelpClicked;
        private bool descriptionHelpClicked;
        public bool DateHelpClicked
        {
            get { return dateHelpClicked; }
            set { dateHelpClicked = value; OnPropertyChanged(); }
        }
        public bool GuestNumHelpClicked
        {
            get { return guestNumHelpClicked; }
            set { guestNumHelpClicked = value; OnPropertyChanged(); }
        }
        public bool DescriptionHelpClicked
        {
            get { return descriptionHelpClicked; }
            set { descriptionHelpClicked = value; OnPropertyChanged(); }
        }
        public ButtonCommandNoParameter GuestNumHelpCommand { get; set; }
        public ButtonCommandNoParameter DateHelpCommand { get; set; }
        public ButtonCommandNoParameter DescriptionHelpCommand { get; set; }
        public string SelectedCountry{
            get => selectedCountry;
            set{ if (value != selectedCountry) { selectedCountry = value; OnPropertyChanged(); } }
        }
        public string SelectedCity{
            get => selectedCity;
            set { if (value != selectedCity) { selectedCity = value; OnPropertyChanged(); } }
        }
        public string Language { get; set; }
        public DateTime MinDate { get; set; }
        public string Description { get; set; }
        public DateTime MaxDate { get; set; }
        public string NumberOfGuests { get; set; }
        public int guestId;
        public TourRequest TourRequest;
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private TourRequestService tourRequestService;
        public TourRequestFormViewModel(int id)
        {
            tourRequestService = new TourRequestService();
            Countries = new ObservableCollection<string>(tourRequestService.getCountries());
            SelectedCountry = Countries[0];
            Cities = new ObservableCollection<string>();
            guestId = id;
            MinDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day + 3);
            MaxDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month, DateTime.Now.Date.Day + 4);
            Language = "";
            GuestNumHelpCommand = new ButtonCommandNoParameter(GuestNumClick);
            DateHelpCommand = new ButtonCommandNoParameter(DateClick);
            DescriptionHelpCommand = new ButtonCommandNoParameter(DescriptionClick);
        }
        private void GuestNumClick()
        {
            GuestNumHelpClicked = !GuestNumHelpClicked;
        }
        private void DateClick()
        {
            DateHelpClicked = !DateHelpClicked;
        }
        private void DescriptionClick()
        {
            DescriptionHelpClicked = !DescriptionHelpClicked;
        }
        public void SetCitiesComboBox()
        {
            Cities.Clear();
            foreach (var city in tourRequestService.getCities(SelectedCountry))
            {
                Cities.Add(city);
            }
            SelectedCity = Cities[0];
        }

        public bool SubmitRequest(int specialTourRequestId = -1)
        {
            return tourRequestService.SaveRequest(SelectedCountry, SelectedCity, Language, NumberOfGuests, DateOnly.FromDateTime(MinDate), DateOnly.FromDateTime(MaxDate), Description, guestId, specialTourRequestId);
        }

        public bool Valid()
        {
            DateOnly minDate = DateOnly.FromDateTime(MinDate);
            int deltaDays = minDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
            int result = 0;
            if (int.TryParse(NumberOfGuests, out result))
            {
                if(result < 1)
                    return false;
            }
            else
                return false;
            if (Language != "" && deltaDays > 2 && MaxDate > MinDate)
            {
                return true;
            }
            return false;
        }
    }
}
