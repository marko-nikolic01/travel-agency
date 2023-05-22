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

namespace TravelAgency.WPF.ViewModels
{
    public class wAccommodationReservationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private AccommodationReservationService _reservationService;
        private AccommodationDateFinderService _dateFinderService;

        public User Guest { get; set; }
        private Accommodation _accommodation;
        private AccommodationReservation _reservation;
        private int _dayNumber;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private ObservableCollection<DateSpan> _availableDateSpans;
        private DateSpan _selectedDateSpan;
        private List<BitmapImage> _photos;
        private BitmapImage _selectedPhoto;
        private int _currentPhotoIndex;
        private bool _shouldValidate;

        public Accommodation Accommodation
        {
            get => _accommodation;
            set
            {
                if (value != _accommodation)
                {
                    _accommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReservation Reservation
        {
            get => _reservation;
            set
            {
                if (value != _reservation)
                {
                    _reservation = value;
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

        public wAccommodationReservationViewModel(User guest, Accommodation accommodation)
        {
            _reservationService = new AccommodationReservationService();
            _dateFinderService = new AccommodationDateFinderService();

            Guest = guest;
            Accommodation = accommodation;
            Reservation = new AccommodationReservation(accommodation.Id, accommodation, Guest.Id, Guest);

            InitializeData();
        }

        private void InitializeData()
        {
            InitializePhotos();
            InitializeDateSpanData();
            _shouldValidate = true;
        }


        private void InitializePhotos()
        {
            Photos = new List<BitmapImage>();
            foreach (AccommodationPhoto photo in Accommodation.Photos)
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
            DayNumber = Accommodation.MinDays;
            FirstDate = DateTime.Now.Date;
            LastDate = DateTime.Now.Date;
            AvailableDateSpans = new ObservableCollection<DateSpan>();
        }

        public void GetNextPhoto()
        {
            if (++_currentPhotoIndex > (Photos.Count() - 1))
            {
                _currentPhotoIndex = 0;
            }
            SelectedPhoto = Photos[_currentPhotoIndex];
        }

        public void GetPreviousPhoto()
        {
            if (--_currentPhotoIndex < 0)
            {
                _currentPhotoIndex = Photos.Count() - 1;
            }
            SelectedPhoto = Photos[_currentPhotoIndex];
        }

        public bool FindAvailableDates()
        {
            if (this.IsValid)
            {
                _dateFinderService.SetReservationLength(DayNumber);
                AvailableDateSpans = new ObservableCollection<DateSpan>(_dateFinderService.FindAvailableDatesInsideDateRange(FirstDate, LastDate, Accommodation));

                if (AvailableDateSpans.Count == 0)
                {
                    AvailableDateSpans = new ObservableCollection<DateSpan>(_dateFinderService.FindAvailableDatesOutsideDateRange(FirstDate, LastDate, Accommodation));
                    System.Windows.MessageBox.Show("There aren't any dates available in the specified date span! Pick one of our suggestions or adjust your search.");
                }

                Reservation.DateSpan = null;
                Reservation.NumberOfGuests = 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MakeReservation()
        {
            if (Reservation.IsValid)
            {
                _reservationService.CreateReservation(Reservation);
                return true;
            }
            return false;
        }

        public void TriggerValidationMessage(bool shouldValidateFirst, bool shouldValidateLast)
        {
            if (shouldValidateFirst)
            {
                FirstDate = FirstDate.AddDays(1);
                FirstDate = FirstDate.AddDays(-1);
            }

            if (shouldValidateLast)
            {
                LastDate = LastDate.AddDays(1);
                LastDate = LastDate.AddDays(-1);
            }
        }


        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DayNumber")
                {
                    if (DayNumber < 0)
                    {
                        return "* Number of days can't be negative";
                    }
                    else if (DayNumber == 0)
                    {
                        return "* Number of days is required";
                    }
                    else if (DayNumber < Accommodation.MinDays)
                    {
                        return "* Number of guests is smaller than allowed";
                    }
                }
                else if (columnName == "FirstDate")
                {
                    bool isFutureDate = FirstDate.CompareTo(DateTime.Now) > 0;

                    if (!isFutureDate)
                    {
                        return "* First date must be a future date";
                    }

                    double dateSpanLength = (LastDate - FirstDate).TotalDays + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified\nnumber of days";
                    }

                }
                else if (columnName == "LastDate")
                {
                    bool isFutureDate = LastDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* Last date must be a future date";
                    }

                    double dateSpanLength = (LastDate - FirstDate).TotalDays + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*Last date can't be before first date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified\nnumber of days";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "DayNumber", "FirstDate", "LastDate" };

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
