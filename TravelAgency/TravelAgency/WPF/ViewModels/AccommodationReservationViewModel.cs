using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class AccommodationReservationViewModel : INotifyPropertyChanged
    {
        private AccommodationReservationService _reservationService;
        private ReservationDateFinderService _dateFinderService;

        private User _guest;
        private Accommodation _accommodation;
        private AccommodationReservation _reservation;
        private int _dayNumber;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private ObservableCollection<DateSpan> _availableDateSpans;
        private DateSpan _selectedDateSpan;
        private List<BitmapImage> _photos;
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

        public AccommodationReservationViewModel()
        {
            _reservationService = new AccommodationReservationService();
            _dateFinderService = new ReservationDateFinderService();

            //InitializeData();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
