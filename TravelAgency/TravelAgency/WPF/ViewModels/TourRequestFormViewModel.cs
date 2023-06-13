using LiveCharts.Maps;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        private string numberOfGuests;
        private string language;
        private string numberOfGuestValid;
        private string languageValid;
        private string minDateValid;
        private string maxDateValid;
        private DateTime maxDate;
        private DateTime minDate;
        public string NumberOfGuestValid
        {
            get { return numberOfGuestValid; }
            set { numberOfGuestValid = value; OnPropertyChanged(); }
        }
        public string LanguageValid
        {
            get { return languageValid; }
            set { if (value != languageValid) { languageValid = value; OnPropertyChanged(); } }
        }
        public string MinDateValid
        {
            get { return minDateValid; }
            set { if (value != minDateValid) { minDateValid = value; OnPropertyChanged(); } }
        }
        public string MaxDateValid
        {
            get { return maxDateValid; }
            set { if (value != maxDateValid) { maxDateValid = value; OnPropertyChanged(); } }
        }

        public string SelectedCountry{
            get => selectedCountry;
            set{ if (value != selectedCountry) { selectedCountry = value; OnPropertyChanged(); SetCitiesComboBox(); } }
        }
        public string SelectedCity{
            get => selectedCity;
            set { if (value != selectedCity) { selectedCity = value; OnPropertyChanged(); } }
        }
        public string Language
        {
            get { return language; }
            set { if (value != language) { language = value; OnPropertyChanged(); IsLanguageValid(); } }
        }
        public DateTime MinDate
        {
            get { return minDate; }
            set { minDate = value; OnPropertyChanged(); IsMinDateValid(); }
        }
        public DateTime MaxDate
        {
            get { return maxDate; }
            set { maxDate = value; OnPropertyChanged(); IsMaxDateValid(); }
        }
        public string Description { get; set; }
        
        public string NumberOfGuests
        {
            get { return numberOfGuests; }
            set { 
                numberOfGuests = value; OnPropertyChanged();
                if (!IsGuestNumValid())
                    NumberOfGuestValid = "Must be positive number";
                else
                    NumberOfGuestValid = "";
                }
        }
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
            NumberOfGuests = "1";
            tourRequestService = new TourRequestService();
            Cities = new ObservableCollection<string>();
            Countries = new ObservableCollection<string>(tourRequestService.getCountries());
            SelectedCountry = Countries[0];
            guestId = id;
            MinDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month+1, DateTime.Now.Date.Day);
            MaxDate = new DateTime(DateTime.Now.Date.Year, DateTime.Now.Date.Month+2, DateTime.Now.Date.Day);
            Language = "";
            UpdateHelpText();
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
           if(IsGuestNumValid() && IsLanguageValid() && IsMinDateValid() && IsMaxDateValid())
                return true;
           else
                return false;
        }
        public bool IsGuestNumValid()
        {
            int result = 0;
            if (int.TryParse(NumberOfGuests, out result))
            {
                if (result < 1)
                    return false;
                else 
                    return true;
            }
            return false;
        }
        public bool IsLanguageValid()
        {
            if (Language == null || Language == "")
            {
                LanguageValid = "Language can't be empty";
                return false;
            }
            else
            {
                LanguageValid = "";
                return true;
            }
        }
        public bool IsMinDateValid()
        {
            DateOnly minDate = DateOnly.FromDateTime(MinDate);
            int deltaDays = minDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
            if(deltaDays < 3)
            {
                IsMaxDateValid();
                MinDateValid = "Must be at least three days from now";
                return false;
            }
            else
            {
                IsMaxDateValid();
                MinDateValid = "";
                return true;
            }
        }
        public bool IsMaxDateValid()
        {
            if(MinDate > MaxDate)
            {
                MaxDateValid = "Must be greater than min date";
                return false;
            }
            else
            {
                MaxDateValid = "";
                return true;
            }
        }
        private void UpdateHelpText()
        {
            string file = @"../../../Resources/HelpTexts/CreateRequestHelp.txt";
            Guest2MainViewModel.HelpText = File.ReadAllText(file);
        }
    }
}
