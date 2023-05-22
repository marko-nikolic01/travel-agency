using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1AccommodationReservationMoveViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        private AccommodationReservationMoveService _reservationMoveService;
        private AccommodationDateFinderService _dateFinderService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand FindAvailableDatesCommand { get; private set; }
        public MyICommand MakeMoveRequestCommand { get; private set; }
        public MyICommand PreviousPhotoCommand { get; private set; }
        public MyICommand NextPhotoCommand { get; private set; }

        private AccommodationReservationMoveRequest _moveRequest;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private ObservableCollection<DateSpan> _availableDateSpans;
        private DateSpan _selectedDateSpan;
        private List<BitmapImage> _photos;
        private BitmapImage _selectedPhoto;
        private int _currentPhotoIndex;
        private bool _shouldValidate;
        private bool _foundDates;
        private DateTime _tomorrow { get; set; }

        public AccommodationReservationMoveRequest MoveRequest
        {
            get => _moveRequest;
            set
            {
                if (value != _moveRequest)
                {
                    _moveRequest = value;
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
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<DateSpan> AvailableDateSpans
        {
            get => _availableDateSpans;
            set
            {
                if (value != _availableDateSpans)
                {
                    _availableDateSpans = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateSpan SelectedDateSpan
        {
            get => _selectedDateSpan;
            set
            {
                if (value != _selectedDateSpan)
                {
                    _selectedDateSpan = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<BitmapImage> Photos
        {
            get => _photos;
            set
            {
                if (value != _photos)
                {
                    _photos = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage SelectedPhoto
        {
            get => _selectedPhoto;
            set
            {
                if (value != _selectedPhoto)
                {
                    _selectedPhoto = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool FoundDates
        {
            get => _foundDates;
            set
            {
                if (value != _foundDates)
                {
                    _foundDates = value;
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

        public Guest1AccommodationReservationMoveViewModel(MyICommand<string> navigationCommand, AccommodationReservation reservation)
        {
            _reservationMoveService = new AccommodationReservationMoveService();
            _dateFinderService = new AccommodationDateFinderService();

            NavigationCommand = navigationCommand;
            FindAvailableDatesCommand = new MyICommand(OnFindAvailableDates);
            MakeMoveRequestCommand = new MyICommand(OnMakeMoveRequest);
            PreviousPhotoCommand = new MyICommand(OnGetPreviousPhoto);
            NextPhotoCommand = new MyICommand(OnGetNextPhoto);

            MoveRequest = new AccommodationReservationMoveRequest(reservation);

            InitializeData();
        }

        private void InitializeData()
        {
            InitializePhotos();
            InitializeDateSpanData();
            _shouldValidate = true;
            FoundDates = false;
            Tomorrow = DateTime.Now.Date.AddDays(1);
        }


        private void InitializePhotos()
        {
            Photos = new List<BitmapImage>();
            foreach (AccommodationPhoto photo in MoveRequest.Reservation.Accommodation.Photos)
            {
                Uri uri = new Uri(photo.Path, UriKind.RelativeOrAbsolute);
                BitmapImage image = new BitmapImage(uri);
                Photos.Add(image);
            }
            SelectedPhoto = Photos[0];
            _currentPhotoIndex = 0;
        }

        private void InitializeDateSpanData()
        {
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
            AvailableDateSpans = new ObservableCollection<DateSpan>();
        }

        public void OnGetNextPhoto()
        {
            if (++_currentPhotoIndex > (Photos.Count() - 1))
            {
                _currentPhotoIndex = 0;
            }
            SelectedPhoto = Photos[_currentPhotoIndex];
        }

        public void OnGetPreviousPhoto()
        {
            if (--_currentPhotoIndex < 0)
            {
                _currentPhotoIndex = Photos.Count() - 1;
            }
            SelectedPhoto = Photos[_currentPhotoIndex];
        }

        public void OnFindAvailableDates()
        {
            if (this.IsValid)
            {
                AvailableDateSpans = new ObservableCollection<DateSpan>(_dateFinderService.FindDatesForReservationMoveRequest(FirstDate, LastDate, MoveRequest.Reservation));

                MoveRequest.DateSpan = null;
                FoundDates = true;
            }
        }

        public void OnMakeMoveRequest()
        {
            if (MoveRequest.IsValid)
            {
                _reservationMoveService.CreateMoveRequest(MoveRequest);
                NavigationCommand.Execute("previousViewModel");
            }
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
                        return "* First date must be a future date";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
                    }
                    else if (dateSpanLength < MoveRequest.Reservation.DateSpan.DayCount)
                    {
                        return "*Date span can't be shorter than specified number of days";
                    }

                }
                else if (columnName == "LastDate")
                {
                    bool isFutureDate = LastDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* Last date must be a future date";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*Last date can't be before first date";
                    }
                    else if (dateSpanLength < MoveRequest.Reservation.DateSpan.DayCount)
                    {
                        return "*Date span can't be shorter than specified number of days";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = {"FirstDate", "LastDate" };

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
