using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1AccommodationSearchViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand SearchCommand { get; private set; }
        public MyICommand CancelSearchCommand { get; private set; }

        private AccommodationService _accommodationService;
        private AccommodationSearchService _searchService;
        private LocationService _locationService;
        private SuperOwnerService _superOwnerService;
        private RenovationService _renovationService;

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
                if ((value != _selectedCountry) && Countries.Contains(value))
                {
                    _selectedCountry = value;
                    UpdateLocationsData(true);
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
                if ((value != _selectedCity) && Cities.Contains(value))
                {
                    _selectedCity = value; 
                    
                    UpdateLocationsData(false);
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

        public Guest1AccommodationSearchViewModel(MyICommand<string> navigationCommand)
        {
            NavigationCommand = navigationCommand;
            SearchCommand = new MyICommand(OnSearch);
            CancelSearchCommand = new MyICommand(OnCancelSearch);

            _accommodationService = new AccommodationService();
            _searchService = new AccommodationSearchService();
            _locationService = new LocationService();
            _superOwnerService = new SuperOwnerService();
            _renovationService = new RenovationService();

            InitializeData();
        }

        private void InitializeData()
        {
            _superOwnerService.SetSuperOwners();
            SearchFilter = new AccommodationSearchFilter();
            InitializeAccommodations();
            InitializeLocations();
            InitializeAccommodationTypes();
        }

        private void InitializeAccommodations()
        {
            List<Accommodation> accommodations = _accommodationService.GetAccommodations();
            accommodations = _superOwnerService.SortBySuperOwnersFirst(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);
            _renovationService.SetRenovationStatus(accommodations);
        }

        private void InitializeLocations()
        {
            Countries = _locationService.GetCountries();
            Countries.Insert(0, "-");
            SelectedCountry = Countries[0];
            List<string> tempCities = _locationService.GetCities();
            tempCities.Insert(0, "-");
            Cities = tempCities;
            SelectedCity = Cities[0];
        }

        private void InitializeAccommodationTypes()
        {
            AccommodationTypes = new ObservableCollection<string>();
            AccommodationTypes.Add("-");
            AccommodationTypes.Add("Apartman");
            AccommodationTypes.Add("Kuća");
            AccommodationTypes.Add("Koliba");
        }

        public void OnSearch()
        {
            List<Accommodation> searchedAccommodations = _searchService.Search(SearchFilter);
            searchedAccommodations = _superOwnerService.SortBySuperOwnersFirst(searchedAccommodations);
            Accommodations = new ObservableCollection<Accommodation>(searchedAccommodations);
        }

        public void OnCancelSearch()
        {
            List<Accommodation> accommodations = _searchService.CancelSearch();
            accommodations = _superOwnerService.SortBySuperOwnersFirst(accommodations);
            Accommodations = new ObservableCollection<Accommodation>(accommodations);
            SearchFilter.NameFilter = "";
            SelectedCountry = "-";
            UpdateLocationsData(true);
            SelectedCity = "-";
            SearchFilter.TypeFilter = "-";
            SearchFilter.GuestNumberFilter = 0;
            SearchFilter.DayNumberFilter = 0;
        }

        public void UpdateLocationsData(bool updateCountry)
        {
            if (updateCountry)
            {
                List<string> tempCities;
                if (SelectedCountry != "-")
                {
                    tempCities = _locationService.GetCitiesByCountry(SelectedCountry);
                }
                else
                {
                    tempCities = _locationService.GetCities();
                }
                tempCities.Insert(0, "-");
                Cities = tempCities;
                SelectedCity = "-";
                SearchFilter.CountryFilter = SelectedCountry;
            }
            SearchFilter.CityFilter = SelectedCity;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
