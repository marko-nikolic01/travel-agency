using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
    public class Guest1WhereverWheneverReservationDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        private AccommodationReservation _reservation;
        private ObservableCollection<DateSpan> _availableDateSpans;

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

        public Guest1WhereverWheneverReservationDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            Visibility = true;
            string text = "Rezervacija: Biramo datum i rezervišemo smeštaj pritiskom na dugme \"Rezerviši\".";
            Reservation.NumberOfGuests = 1;
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000);
        }

        private void InitializeData()
        {
            InitializeDateSpanData();
        }

        private void InitializeDateSpanData()
        {
            AvailableDateSpans = new ObservableCollection<DateSpan>();
            DateSpan dateSpan = new DateSpan(DateOnly.FromDateTime(new DateTime(3000, 1, 1)), DateOnly.FromDateTime(new DateTime(3000, 1, 2)));
            AvailableDateSpans.Add(dateSpan);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
