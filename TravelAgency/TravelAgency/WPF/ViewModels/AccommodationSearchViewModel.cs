using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class AccommodationSearchViewModel : INotifyPropertyChanged
    {
        private AccommodationService _accommodationService;
        private AccommodationSearchService _searchService;
        private LocationService _locationService;
        private SuperOwnerService _superOwnerService;

        //private User _guest;
        private ObservableCollection<Accommodation> _accommodations;
        private Accommodation _selectedAccommodation;
        private List<string> _countries;
        private List<string> _cities;
        private string _selectedCountry;
        private string _selectedCity;
        private ObservableCollection<string> _accommodationTypes;
        public AccommodationSearchFilter SearchFilter { get; set; }

        public ObservableCollection<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {
                    _accommodations = value;
                    OnPropertyChanged();
                }
            }
        }

        public Accommodation SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                if (value != _selectedAccommodation)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> Countries
        {
            get => _countries;
            set
            {
                if (value != _countries)
                {
                    _countries = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (value != _selectedCountry)
                {
                    _selectedCountry = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> Cities
        {
            get => _cities;
            set
            {
                if (value != _cities)
                {
                    _cities = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (value != _selectedCity)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> AccommodationTypes
        {
            get => _accommodationTypes;
            set
            {
                if (value != _accommodationTypes)
                {
                    _accommodationTypes = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationSearchViewModel(/*User guest*/)
        {
            _accommodationService = new AccommodationService();
            _searchService = new AccommodationSearchService();
            _locationService = new LocationService();
            _superOwnerService = new SuperOwnerService();

            InitializeData();
            //guest = guest;
            SearchFilter = new AccommodationSearchFilter();
        }

        private void InitializeData()
        {
            _superOwnerService.SetSuperOwners();
            InitializeAccommodations();
            InitializeLocations();
            InitializeAccommodationTypes();
        }

        private void InitializeAccommodations()
        {
            List<Accommodation> accommodations = _accommodationService.GetAccommodations();
            accommodations = _superOwnerService.SortBySuperOwnersFirst(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);
        }

        private void InitializeLocations()
        {
            Countries = _locationService.GetCountries();
            Countries.Insert(0, "Not specified");
            SelectedCountry = Countries[0];
            Cities = _locationService.GetCities();
            Cities.Insert(0, "Not specified");
            SelectedCity = Cities[0];
        }

        private void InitializeAccommodationTypes()
        {
            AccommodationTypes = new ObservableCollection<string>();
            AccommodationTypes.Add("Not specified");
            AccommodationTypes.Add("Appartment");
            AccommodationTypes.Add("House");
            AccommodationTypes.Add("Hut");
        }

        public void UpdateLocationsData(bool updateCountry)
        {
            if (updateCountry)
            {
                if (SelectedCountry != "Not specified")
                {
                    Cities = _locationService.GetCitiesByCountry(SelectedCountry);
                }
                else
                {
                    Cities = _locationService.GetCities();
                }
                Cities.Insert(0, "Not specified");
                SelectedCity = Cities[0];
                SearchFilter.CountryFilter = SelectedCountry;
            }
            SearchFilter.CityFilter = SelectedCity;
        }

        public void Search()
        {
            List<Accommodation> searchedAccommodations = _searchService.Search(SearchFilter);
            searchedAccommodations = _superOwnerService.SortBySuperOwnersFirst(searchedAccommodations);
            Accommodations = new ObservableCollection<Accommodation>(searchedAccommodations);
        }

        public void CancelSearch()
        {
            List<Accommodation> accommodations = _searchService.CancelSearch();
            accommodations = _superOwnerService.SortBySuperOwnersFirst(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);
            SearchFilter.NameFilter = "";
            SelectedCountry = "Not specified";
            UpdateLocationsData(true);
            SearchFilter.TypeFilter = "Not specified";
            SearchFilter.GuestNumberFilter = 0;
            SearchFilter.DayNumberFilter = 0;

        }

        public event PropertyChangedEventHandler? PropertyChanged; 
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
