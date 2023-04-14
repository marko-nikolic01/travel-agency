using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelAgency.Repository;
using TravelAgency.Services;

namespace TravelAgency.ViewModel
{
    public class TourRequestFormViewModel : INotifyPropertyChanged
    {
        private string selectedCountry;
        private string selectedCity;
        public string SelectedCountry {
            get => selectedCountry;
            set
            {
                if (value != selectedCountry)
                {
                    selectedCountry = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedCity {
            get => selectedCity;
            set
            {
                if (value != selectedCity)
                {
                    selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Language { get; set; }
        public DateTime MinDate { get; set; }
        public string Description { get; set; }
        public DateTime MaxDate { get; set; }
        public string NumberOfGuests { get; set; }
        private int guestId;
        private LocationRepository locationRepository;
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
            locationRepository = new LocationRepository();
            tourRequestService = new TourRequestService();
            Countries = new ObservableCollection<string>(tourRequestService.getCountries());
            SelectedCountry = Countries[0];
            Cities = new ObservableCollection<string>();
            guestId = id;
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

        public bool SubmitRequest()
        {
            return tourRequestService.SaveRequest(SelectedCountry, SelectedCity, Language, NumberOfGuests, MinDate, MaxDate, Description, guestId);
        }
    }
}
