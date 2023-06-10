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
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    class Guest1AccommodationReservationsDemoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;

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

        private ObservableCollection<AccommodationReservation> _reservations;

        public ObservableCollection<AccommodationReservation> Reservations
        {
            get => _reservations;
            set
            {
                if (value != _reservations)
                {
                    _reservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1AccommodationReservationsDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;

            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            string text = "Otkazivanje rezervacija: Otkazujemo rezervaciju pritiskom na dugme \"Otkaži\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnCancelReservation();

            text = "Izmena rezervacije: Nastavljamo na izmenu rezervacije pritiskom na dugme \"Izmeni\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000);
        }

        private void InitializeData()
        {
            InitializeReservations();
        }

        private void InitializeReservations()
        {
            Reservations = new ObservableCollection<AccommodationReservation>();
            AccommodationReservation reservation = new AccommodationReservation();
            reservation.DateSpan = new DateSpan(DateOnly.FromDateTime(new DateTime(3000, 1, 1)), DateOnly.FromDateTime(new DateTime(3000, 1, 2)));
            Reservations.Add(reservation);
            reservation = new AccommodationReservation();
            reservation.DateSpan = new DateSpan(DateOnly.FromDateTime(new DateTime(3000, 1, 1)), DateOnly.FromDateTime(new DateTime(3000, 1, 2)));
            Reservations.Add(reservation);
        }

        public void OnCancelReservation()
        {
            Reservations = new ObservableCollection<AccommodationReservation>();
            AccommodationReservation reservation = new AccommodationReservation();
            reservation.DateSpan = new DateSpan(DateOnly.FromDateTime(new DateTime(3000, 1, 1)), DateOnly.FromDateTime(new DateTime(3000, 1, 2)));
            Reservations.Add(reservation);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
