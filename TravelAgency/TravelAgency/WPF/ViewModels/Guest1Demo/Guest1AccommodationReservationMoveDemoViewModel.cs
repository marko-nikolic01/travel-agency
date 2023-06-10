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
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    class Guest1AccommodationReservationMoveDemoViewModel: ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {

        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;
        private bool _visibility1;
        private bool _visibility2;


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

        public bool Visibility1
        {
            get => _visibility1;
            set
            {
                if (value != _visibility1)
                {
                    _visibility1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Visibility2
        {
            get => _visibility2;
            set
            {
                if (value != _visibility2)
                {
                    _visibility2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationReservation _reservation;
        private int _dayNumber;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private ObservableCollection<DateSpan> _availableDateSpans;
        private bool _shouldValidate;
        private bool _foundDates;
        private DateTime _tomorrow { get; set; }

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
                    TriggerValidationMessage();
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

        public Guest1AccommodationReservationMoveDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
            Reservation = new AccommodationReservation();
            Accommodation accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Novi Sad";
            accommdation.Location.Country = "Serbia";
            accommdation.MinDays = 1;
            accommdation.MaxGuests = 1;
            Reservation.Accommodation = accommdation;

            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            Visibility1 = true;
            Visibility2 = false;
            string text = "Pronalaženje datuma: Unosimo parametre pretrage.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            FirstDate = new DateTime(3000, 1, 1); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            LastDate = new DateTime(3000, 1, 2); ; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pronalaženje datuma: Pronalazimo datume pritiskom na dugme \"Pronađi datume\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            DateSpan dateSpan = new DateSpan(DateOnly.FromDateTime(FirstDate), DateOnly.FromDateTime(LastDate));
            AvailableDateSpans = new ObservableCollection<DateSpan>();
            AvailableDateSpans.Add(dateSpan);
            FoundDates = true;

            Visibility1 = false;
            Visibility2 = true;
            text = "Pomeranje rezervacije: Biramo datum i zatražite izmenu rezervacije pritiskom na dugme \"Zatraži izmenu\".";
            Reservation.NumberOfGuests = 1;
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000);
        }

        private void InitializeData()
        {
            InitializeDateSpanData();
            Reservation.NumberOfGuests = 1;
            _shouldValidate = true;
            FoundDates = false;
            Tomorrow = DateTime.Now.Date.AddDays(1);
        }

        private void InitializeDateSpanData()
        {
            DayNumber = 1;
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
            AvailableDateSpans = new ObservableCollection<DateSpan>();
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
                    else if (DayNumber < 1)
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

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
                    }
                    else if (dateSpanLength < DayNumber)
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
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified number of days";
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
