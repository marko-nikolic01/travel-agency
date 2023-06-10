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
    public class Guest1ReportViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand ReportReservationsCommand { get; private set; }
        public MyICommand ReportCanceledReservationsCommand { get; private set; }

        private AccommodationReservationService _reservationService;
        private PDFReportService _reportService;

        public User Guest;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private bool _shouldValidate;
        private string _count;
        private string _canceledCount;
        private List<AccommodationReservation> _reservations;
        private List<AccommodationReservation> _canceledReservations;
        private bool _valid;

        private DateTime _tomorrow { get; set; }

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
                    UpdateReservationData();
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
                    UpdateReservationData();
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

        public string Count
        {
            get => _count;
            set
            {
                if (value != _count)
                {
                    _count = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CanceledCount
        {
            get => _canceledCount;
            set
            {
                if (value != _canceledCount)
                {
                    _canceledCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Valid
        {
            get => _valid;
            set
            {
                if (value != _valid)
                {
                    _valid = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1ReportViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;
            ReportReservationsCommand = new MyICommand(OnReportReservations);
            ReportCanceledReservationsCommand = new MyICommand(OnReportCanceledReservations);

            _reservationService = new AccommodationReservationService();
            _reportService = new PDFReportService();

            Guest = guest;
            _reservations = new List<AccommodationReservation>();
            _canceledReservations = new List<AccommodationReservation>();

            InitializeData();
        }

        private void OnReportReservations()
        {
            if (this.IsValid)
            {
                _reportService.WriteAccommodationReservationReport(Guest, _reservations, FirstDate, LastDate);
            }
        }

        private void OnReportCanceledReservations()
        {
            if (this.IsValid)
            {
                _reportService.WriteCanceledAccommodationReservationReport(Guest, _canceledReservations, FirstDate, LastDate);
            }
        }

        private void InitializeData()
        {
            Tomorrow = DateTime.Now.Date.AddDays(1);
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
            _shouldValidate = true;
            UpdateReservationData();
        }

        private void UpdateReservationData()
        {
            _reservations = _reservationService.GetByGuest(Guest);
            _canceledReservations = _reservationService.GetCanceledByGuest(Guest);
            FindReservationsInDateSpan();
            SortReservations(_reservations);
            SortReservations(_canceledReservations);
            if (this.IsValid)
            {
                if (_reservations.Count() >= 0)
                {
                    Count = (_reservations.Count()).ToString();
                }
                if (_canceledReservations.Count() >= 0)
                {
                    CanceledCount = (_canceledReservations.Count()).ToString();
                }
                Valid = true;
                return;
            }
            Count = "";
            CanceledCount = "";
            Valid = false;
        }

        private void SortReservations(List<AccommodationReservation> reservations)
        {
            for (int i = 0; i < reservations.Count() - 1; i++)
            {
                for (int j = 0; j < reservations.Count() - i - 1; j++)
                {
                    if (reservations[j].DateSpan.StartDate.CompareTo(reservations[j + 1].DateSpan.StartDate) > 0)
                    {
                        AccommodationReservation swaper = reservations[j];
                        reservations[j] = reservations[j + 1];
                        reservations[j + 1] = swaper;
                    }
                }
            }
        }

        private void FindReservationsInDateSpan()
        {
            List<AccommodationReservation> newReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _reservations)
            {
                if (reservation.DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(FirstDate)) >= 0
                    && reservation.DateSpan.EndDate.CompareTo(DateOnly.FromDateTime(LastDate)) <= 0)
                {
                    newReservations.Add(reservation);
                }
            }
            _reservations = newReservations;

            List<AccommodationReservation> newCanceledReservations = new List<AccommodationReservation>();
            foreach (AccommodationReservation reservation in _canceledReservations)
            {
                if (reservation.DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(FirstDate)) >= 0
                    && reservation.DateSpan.EndDate.CompareTo(DateOnly.FromDateTime(LastDate)) <= 0)
                {
                    newCanceledReservations.Add(reservation);
                }
            }
            _canceledReservations = newCanceledReservations;
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
                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "* Početni datum ne može biti posle krajnjeg datuma";
                    }

                }
                else if (columnName == "LastDate")
                {
                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "* Krajnji datum ne može biti pre početnog datuma";
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
