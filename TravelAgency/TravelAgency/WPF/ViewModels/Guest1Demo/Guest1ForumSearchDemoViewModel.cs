using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1ForumSearchDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        private List<string> _countries;
        private List<string> _cities;
        private string _selectedCountry;
        private string _selectedCity;
        private ObservableCollection<Forum> _forums;

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

        public Guest1ForumSearchDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
            _locationService = new LocationService();
            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            Visibility = true;
            string text = "Pretraga foruma: Popunjavamo parametre pretrage.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SelectedCountry = "Serbia"; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SelectedCity = "Novi Sad"; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pretraga foruma: Pretragu obavljamo pritiskom na dugme \"Pretraži\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnSearch();

            text = "Pretraga foruma: Pretragu otkazujemo pritiskom na dugme \"Otkaži pretragu\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnCancelSearch();

            Visibility = false;
            text = "Čitanje i pisanje komentara: Biramo forum i pritiskom na dugme \"Čitaj i komentariši/Čitaj\" ulazimo u forum gde možemo učestvovati u diskusiji sa drugim korisnicima.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000);
        }

        private void InitializeData()
        {
            InitializeForums();
            InitializeLocations();
        }

        private void InitializeForums()
        {
            List<Forum> forums = new List<Forum>();
            Forum forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Serbia";
            forum.Location.City = "Novi Sad";
            forums.Add(forum);
            forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Croatia";
            forum.Location.City = "Zagreb";
            forum.Close();
            forums.Add(forum);
            Forums = new ObservableCollection<Forum>(forums);
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

        public void OnSearch()
        {
            Forums = new ObservableCollection<Forum>();
            Forum forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Serbia";
            forum.Location.City = "Novi Sad";
            Forums.Add(forum);
        }

        public void OnCancelSearch()
        {
            Forums = new ObservableCollection<Forum>();
            Forum forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Serbia";
            forum.Location.City = "Novi Sad";
            Forums.Add(forum);
            forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Croatia";
            forum.Location.City = "Zagreb";
            forum.Close();
            Forums.Add(forum);
            SelectedCountry = "-";
            UpdateLocationsData(true);
            SelectedCity = "-";
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
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
