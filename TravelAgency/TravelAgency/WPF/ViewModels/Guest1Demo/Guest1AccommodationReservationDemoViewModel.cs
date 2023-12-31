﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1AccommodationReservationDemoViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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

        public Guest1AccommodationReservationDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            DayNumber= 2; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
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
            text = "Rezervacija: Biramo broj gostiju, zatim datum i rezervišemo smeštaj pritiskom na dugme \"Rezerviši\".";
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
                        return "* Broj dana ne može biti negativan";
                    }
                    else if (DayNumber == 0)
                    {
                        return "* Broj dana je obavezan";
                    }
                    else if (DayNumber < 1)
                    {
                        return "* Broj dana je manji od dozvoljenog";
                    }
                }
                else if (columnName == "FirstDate")
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
                    else if (dateSpanLength < 2)
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
                    else if (dateSpanLength < 2)
                    {
                        return "* Opseg datuma je kraći od broja dana";
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
