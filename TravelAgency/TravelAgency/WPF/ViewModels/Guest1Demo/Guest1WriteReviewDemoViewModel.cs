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
    public class Guest1WriteReviewDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        private AccommodationOwnerRating _rating;
        private RenovationRecommendation _renovationRecommendation;
        private bool _writeRenovationRecommendation;

        public AccommodationOwnerRating Rating
        {
            get => _rating;
            set
            {
                if (value != _rating)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        public RenovationRecommendation RenovationRecommendation
        {
            get => _renovationRecommendation;
            set
            {
                if (value != _renovationRecommendation)
                {
                    _renovationRecommendation = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool WriteRenovationRecommendation
        {
            get => _writeRenovationRecommendation;
            set
            {
                if (value != _writeRenovationRecommendation)
                {
                    _writeRenovationRecommendation = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1WriteReviewDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            Visibility = true;
            string text = "Pisanje recenzije: Unosimo sve ocene i komentare.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Rating.AccommodationCleanliness = 1; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Rating.AccommodationComfort = 2; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Rating.AccommodationLocation = 3; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Rating.OwnerCorrectness = 4; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Rating.OwnerResponsiveness = 5; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Rating.Comment = "Komentar..."; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pisanje preporuke za renoviranje: Po potrebi popunjavamo preporuku za renoviranje.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            WriteRenovationRecommendation = true; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            RenovationRecommendation.Description = "Opis..."; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            RenovationRecommendation.UrgencyLevel = UrgencyLevel.LEVEL3; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Na kraju pritiskom na dugme \"Pošalji recenziju\" završavamo sa pisanjem recenzije.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        private void InitializeData()
        {
            _writeRenovationRecommendation = false;
            Rating = new AccommodationOwnerRating();
            RenovationRecommendation = new RenovationRecommendation(Rating);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
