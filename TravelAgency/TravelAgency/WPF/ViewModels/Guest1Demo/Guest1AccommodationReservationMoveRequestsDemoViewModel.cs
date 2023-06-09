using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1AccommodationReservationMoveRequestsDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        private ObservableCollection<AccommodationReservationMoveRequest> _moveRequests;

        public ObservableCollection<AccommodationReservationMoveRequest> MoveRequests
        {
            get => _moveRequests;
            set
            {
                if (value != _moveRequests)
                {
                    _moveRequests = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1AccommodationReservationMoveRequestsDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            string text = "Zahtevi za izmenu rezervacija: Ovde možete pogledati svoje zahteve za izmenu rezervacija i njihov status.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000);
        }

        private void InitializeData()
        {
            InitializeMoveRequests();
        }

        private void InitializeMoveRequests()
        {
            MoveRequests = new ObservableCollection<AccommodationReservationMoveRequest>();
            MoveRequests.Add(new AccommodationReservationMoveRequest());
            MoveRequests.Add(new AccommodationReservationMoveRequest());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
