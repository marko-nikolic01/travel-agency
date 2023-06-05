using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1ForumSearchViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand SearchCommand { get; private set; }
        public MyICommand CancelSearchCommand { get; private set; }

        private ForumService _forumService;
        private LocationService _locationService;

        private List<string> _countries;
        private List<string> _cities;
        private string _selectedCountry;
        private string _selectedCity;
        private ObservableCollection<Forum> _forums;
        private Forum _selectedForum;

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

        public ObservableCollection<Forum> Forums
        {
            get => _forums;
            set
            {
                if (value != _forums)
                {
                    _forums = value;
                    OnPropertyChanged();
                }
            }
        }

        public Forum SelectedForum
        {
            get => _selectedForum;
            set
            {
                if (value != _selectedForum)
                {
                    _selectedForum = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1ForumSearchViewModel(MyICommand<string> navigationCommand)
        {
            NavigationCommand = navigationCommand;
            SearchCommand = new MyICommand(OnSearch);
            CancelSearchCommand = new MyICommand(OnCancelSearch);

            _forumService = new ForumService();
            _locationService = new LocationService();

            InitializeData();
        }

        private void InitializeData()
        {
            InitializeForums();
            InitializeLocations();
        }

        private void InitializeForums()
        {
            List<Forum> forums = _forumService.GetForums();
            forums.Reverse();
            Forums = new ObservableCollection<Forum>(forums);
        }

        private void InitializeLocations()
        {   
            Countries = _locationService.GetCountries();
            Countries.Insert(0, "Not specified");
            SelectedCountry = Countries[0];
            List<string> tempCities = _locationService.GetCities();
            tempCities.Insert(0, "Not specified");
            Cities = tempCities;
            SelectedCity = Cities[0];
        }

        public void OnSearch()
        {
            if ((SelectedCountry == "Not specified") && (SelectedCity == "Not specified"))
            {
                List<Forum> forums = _forumService.GetForums();
                Forums = new ObservableCollection<Forum>(forums);
            }
            else if ((SelectedCountry != "Not specified") && (SelectedCity == "Not specified"))
            {
                List<Forum> forums = _forumService.GetForumsByCountry(SelectedCountry);
                Forums = new ObservableCollection<Forum>(forums);
            }
            else if ((SelectedCountry == "Not specified") && (SelectedCity != "Not specified"))
            {
                List<Forum> forums = _forumService.GetForumsByCity(SelectedCity);
                Forums = new ObservableCollection<Forum>(forums);
            }
            else
            {
                List<Forum> forums = _forumService.GetForumsByCountryAndCity(SelectedCountry, SelectedCity);
                Forums = new ObservableCollection<Forum>(forums);
            }
        }

        public void OnCancelSearch()
        {
            List<Forum> forums = _forumService.GetForums();
            Forums = new ObservableCollection<Forum>(forums);
            SelectedCountry = "Not specified";
            UpdateLocationsData(true);
            SelectedCity = "Not specified";
        }

        public void UpdateLocationsData(bool updateCountry)
        {
            if (updateCountry)
            {
                List<string> tempCities;
                if (SelectedCountry != "Not specified")
                {
                    tempCities = _locationService.GetCitiesByCountry(SelectedCountry);
                }
                else
                {
                    tempCities = _locationService.GetCities();
                }
                tempCities.Insert(0, "Not specified");
                Cities = tempCities;
                SelectedCity = "Not specified";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
