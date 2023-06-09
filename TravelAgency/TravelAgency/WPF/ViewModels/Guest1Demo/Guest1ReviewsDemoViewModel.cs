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
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1ReviewsDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        private ObservableCollection<AccommodationGuestRating> _ratings;

        public ObservableCollection<AccommodationGuestRating> Ratings
        {
            get => _ratings;
            set
            {
                if (value != _ratings)
                {
                    _ratings = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1ReviewsDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            string text = "Recenzije: Ovde možete pogledaati kako su Vas ocenili vlasnici smeštajeva u kojima ste boravili.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        private void InitializeData()
        {
            InitializeMoveRequests();
        }

        private void InitializeMoveRequests()
        {
            Ratings = new ObservableCollection<AccommodationGuestRating>();
            Ratings.Add(new AccommodationGuestRating());
            Ratings.Add(new AccommodationGuestRating());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
