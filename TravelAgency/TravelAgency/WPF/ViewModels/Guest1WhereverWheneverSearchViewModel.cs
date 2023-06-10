using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1WhereverWheneverSearchViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand SearchCommand { get; private set; }
        public MyICommand CancelSearchCommand { get; private set; }

        private WhereverWheneverService _whereverWheneverService;
        private AccommodationService _accommodationService;
        private SuperOwnerService _superOwnerService;
        private RenovationService _renovationService;

        private ObservableCollection<Accommodation> _accommodations;
        private Accommodation _selectedAccommodation;
        public WhereverWheneverSearchFilter SearchFilter { get; set; }
        public WhereverWheneverSearchFilter LastUsedSearchFilter { get; set; }
        private int _dayNumber;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private bool _shouldValidate;
        private DateTime _tomorrow { get; set; }

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

        public int DayNumber
        {
            get => _dayNumber;
            set
            {
                if (value != _dayNumber)
                {
                    _dayNumber = value;
                    TriggerValidationMessage();
                    SearchFilter.DayNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                if (value != _firstDate)
                {
                    _firstDate = value;
                    if (_shouldValidate)
                    {
                        TriggerValidationMessage();
                    }
                    SearchFilter.FirstDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime LastDate
        {
            get => _lastDate;
            set
            {
                if (value != _lastDate)
                {
                    _lastDate = value;
                    if (_shouldValidate)
                    {
                        TriggerValidationMessage();
                    }
                    SearchFilter.LastDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Tomorrow
        {
            get => _tomorrow;
            set
            {
                if (value != _tomorrow)
                {
                    _tomorrow = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1WhereverWheneverSearchViewModel(MyICommand<string> navigationCommand)
        {
            NavigationCommand = navigationCommand;
            SearchCommand = new MyICommand(OnSearch);
            CancelSearchCommand = new MyICommand(OnCancelSearch);

            _whereverWheneverService = new WhereverWheneverService();
            _accommodationService = new AccommodationService();
            _superOwnerService = new SuperOwnerService();
            _renovationService = new RenovationService();

            InitializeData();
        }

        private void InitializeData()
        {
            _superOwnerService.SetSuperOwners();
            SearchFilter = new WhereverWheneverSearchFilter();
            DayNumber = 1;
            Tomorrow = DateTime.Now.Date.AddDays(1);
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
            _shouldValidate = true;
            InitializeAccommodations();
        }

        private void InitializeAccommodations()
        {
            List<Accommodation> allAccommodations = _accommodationService.GetAccommodations();
            _renovationService.SetRenovationStatus(allAccommodations);
            Accommodations = new ObservableCollection<Accommodation>();
        }

        public void OnSearch()
        {
            List<Accommodation> searchedAccommodations = _whereverWheneverService.SearchAccommodationsByFilter(SearchFilter);
            if (searchedAccommodations != null)
            {
                LastUsedSearchFilter = new WhereverWheneverSearchFilter(SearchFilter);
                searchedAccommodations = _superOwnerService.SortBySuperOwnersFirst(searchedAccommodations);
                Accommodations = new ObservableCollection<Accommodation>(searchedAccommodations);
            }
        }

        public void OnCancelSearch()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            DayNumber = 1;
            SearchFilter.GuestNumber = 1;
            SearchFilter.SearchInsideDateSpan = false;
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
        }

        public void TriggerValidationMessage()
        {
            _shouldValidate = false;
            FirstDate = FirstDate.AddDays(1);
            FirstDate = FirstDate.AddDays(-1);
            LastDate = LastDate.AddDays(1);
            LastDate = LastDate.AddDays(-1);
            _shouldValidate = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {       
                if (columnName == "FirstDate")
                {
                    bool isFutureDate = FirstDate.CompareTo(DateTime.Now) > 0;

                    if (!isFutureDate)
                    {
                        return "* Početni datum mora biti u budućnosti";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "* Početni datum ne može biti posle krajnjeg datuma";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "* Opseg datuma je kraći od broja dana";
                    }

                }
                else if (columnName == "LastDate")
                {
                    bool isFutureDate = LastDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* Krajnji datum mora biti u budućnosti";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "* Krajnji datum ne može biti pre početnog datuma";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "* Opseg datuma je kraći od broja dana";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "FirstDate", "LastDate" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
