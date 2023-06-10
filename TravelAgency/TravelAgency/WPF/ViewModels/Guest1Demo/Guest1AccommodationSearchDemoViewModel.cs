using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1AccommodationSearchDemoViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;
        private bool _visibility;

        public DemoInstruction Instruction
        {
            get => _instruction;
            set
            {
                if (value != _instruction)
                {
                    _instruction = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Visibility
        {
            get => _visibility;
            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private LocationService _locationService;

        private ObservableCollection<Accommodation> _accommodations;
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

        public Guest1AccommodationSearchDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
            _locationService = new LocationService();
            Visibility = false;

            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            Visibility = true;
            string text = "Pretraga smeštaja: Unosimo parametre pretrage.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text);    Delay(3000);    if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.NameFilter = "Smeštaj";    Delay(3000);    if (_demoStopper.Token.IsCancellationRequested) return;
            SelectedCountry = "Serbia"; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SelectedCity = "Novi Sad"; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.TypeFilter = "Appartment"; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.GuestNumberFilter = 1; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.DayNumberFilter = 1; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pretraga smeštaja: Pretragu obavljamo pritiskom na dugme \"Pretraži\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnSearch();

            text = "Pretraga smeštaja: Pretragu otkazujemo pritiskom na dugme \"Otkaži pretragu\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnCancelSearch();

            Visibility = false;
            text = "Rezervacija smeštaja: Biramo smeštaj i nastavljamo na rezervaciju pritiskom na dugme \"Rezerviši\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000);
        }

        private void InitializeData()
        {
            SearchFilter = new AccommodationSearchFilter();
            InitializeAccommodations();
            InitializeLocations();
            InitializeAccommodationTypes();
        }

        private void InitializeAccommodations()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            Accommodation accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Novi Sad";
            accommdation.Location.Country = "Serbia";
            Accommodations.Add(accommdation);
            accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Zagreb";
            accommdation.Location.Country = "Croatia";
            Accommodations.Add(accommdation);
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

        private void InitializeAccommodationTypes()
        {
            AccommodationTypes = new ObservableCollection<string>();
            AccommodationTypes.Add("Not specified");
            AccommodationTypes.Add("Appartment");
            AccommodationTypes.Add("House");
            AccommodationTypes.Add("Hut");
        }

        public void OnSearch()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            Accommodation accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Novi Sad";
            accommdation.Location.Country = "Serbia";
            Accommodations.Add(accommdation);
        }

        public void OnCancelSearch()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            Accommodation accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Novi Sad";
            accommdation.Location.Country = "Serbia";
            Accommodations.Add(accommdation);
            accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Zagreb";
            accommdation.Location.Country = "Croatia";
            Accommodations.Add(accommdation);
            SearchFilter.NameFilter = "";
            SelectedCountry = "Not specified";
            UpdateLocationsData(true);
            SelectedCity = "Not specified";
            SearchFilter.TypeFilter = "Not specified";
            SearchFilter.GuestNumberFilter = 0;
            SearchFilter.DayNumberFilter = 0;
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
