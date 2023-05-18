using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRequestBookingViewModel : INotifyPropertyChanged
    {
        private int numberOfGuests;
        public int NumberOfGuests
        {
            get { return numberOfGuests; }
            set 
            { 
                numberOfGuests = value;
                OnPropertyChanged();
            }
        }
        private string language;
        public string Language
        {
            get { return language; }
            set 
            { 
                language = value;
                OnPropertyChanged();
            }
        }
        private string selectedCountry;
        public string SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                if (value.Equals(Countries[0]))
                {
                    Cities.Clear();
                    Cities.Add("< city >");
                    IsCountrySelected = false;
                }
                else
                {
                    Cities.Clear();
                    Cities.Add("< city >");
                    foreach (var city in TourRequestService.GetUniqueCitiesByCountryForPendings(value))
                    {
                        Cities.Add(city);
                    }
                    IsCountrySelected = true;
                }
                SelectedCity = Cities[0];
                selectedCountry = value;
                OnPropertyChanged();
            }
        }
        public List<string> Countries { get; set; }
        private ObservableCollection<TourRequest> tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get { return tourRequests; }
            set
            {
                if (value != tourRequests)
                {
                    tourRequests = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<string> cities;
        public ObservableCollection<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged();
            }
        }
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged();
            }
        }
        private bool isCountrySelected;
        public bool IsCountrySelected
        {
            get { return isCountrySelected; }
            set
            {
                isCountrySelected = value;
                OnPropertyChanged();
            }
        }
        private string startDate;
        public string StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged();
            }
        }
        private string endDate;
        public string EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public User ActiveGuide { get; set; }
        public TourRequest SelectedRequest { get; set; }
        public ButtonCommandNoParameter BookCommand { get; set; }
        public ButtonCommandNoParameter FilterCommand { get; set; }
        public ButtonCommandNoParameter RemoveFilterCommand { get; set; }
        public TourRequestService TourRequestService { get; set; }
        public LocationService LocationService { get; set; }
        public NavigationService NavigationService { get; set; }
        public TourRequestBookingViewModel(int activeGuideId, NavigationService navService)
        {
            NavigationService = navService;
            ActiveGuide = new UserService().GetById(activeGuideId);
            TourRequestService = new TourRequestService();
            TourRequests = new ObservableCollection<TourRequest>(TourRequestService.GetPendings());
            FilterCommand = new ButtonCommandNoParameter(Filter);
            RemoveFilterCommand = new ButtonCommandNoParameter(RemoveFilter);
            BookCommand = new ButtonCommandNoParameter(ShowRequest);
            Cities = new ObservableCollection<string>() { "< city >" };
            SelectedCity = Cities[0];
            Countries = new List<string>() { "< country >" };
            SelectedCountry = Countries[0];
            LocationService = new LocationService();
            Countries.AddRange(TourRequestService.GetUniqueCountriesForPendings());
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Filter()
        {
            List<TourRequest> tourRequests = new List<TourRequest>(TourRequestService.GetPendings());
            if (Language != "" && Language != null)
            {
                tourRequests = tourRequests.Where(t => t.Language.ToLower().Trim().Contains(Language.ToLower().Trim())).ToList();
            }
            if (NumberOfGuests > 0 && NumberOfGuests != null)
            {
                tourRequests = tourRequests.Where(t => t.GuestNumber <= NumberOfGuests).ToList();
            }
            if (SelectedCountry != Countries[0])
            {
                tourRequests = tourRequests.Where(t => t.Location.Country.Contains(SelectedCountry)).ToList();
            }
            if (SelectedCity != Cities[0])
            {
                tourRequests = tourRequests.Where(t => t.Location.City.Contains(SelectedCity)).ToList();
            }
            if (StartDate != null)
            {
                DateTime date = DateTime.ParseExact(StartDate, "G", new CultureInfo("en-US"));
                string dateTime = date.ToString("dd-MM-yyyy");
                tourRequests = tourRequests.Where(t => t.MaxDate >= DateOnly.ParseExact(dateTime, "dd-MM-yyyy")).ToList();
            }
            if (EndDate != null)
            {
                DateTime date = DateTime.ParseExact(EndDate, "G", new CultureInfo("en-US"));
                string dateTime = date.ToString("dd-MM-yyyy");
                tourRequests = tourRequests.Where(t => t.MinDate <= DateOnly.ParseExact(dateTime, "dd-MM-yyyy")).ToList();
            }
            TourRequests = new ObservableCollection<TourRequest>(tourRequests);
        }
        private void RemoveFilter()
        {
            NumberOfGuests = 0;
            SelectedCountry = Countries[0];
            SelectedCity = Cities[0];
            StartDate = null;
            EndDate = null;
            Language = "";
            TourRequests = new ObservableCollection<TourRequest>(TourRequestService.GetPendings());
        }
        private void ShowRequest()
        {
            Page acceptTourRequest = new AcceptTourRequestDialogue(SelectedRequest, ActiveGuide.Id, NavigationService);
            NavigationService.Navigate(acceptTourRequest);
        }


    }
}
