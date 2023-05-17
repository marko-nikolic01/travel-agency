using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1WriteReviewViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationOwnerRatingService _ratingService;

        public MyICommand<string> NavigationCommand { get; private set; }

        public User Guest { get; set; }
        private ObservableCollection<AccommodationReservation> _stays;
        private AccommodationReservation _selectedStay;
        private DateTime _currentDateTime;

        public ObservableCollection<AccommodationReservation> Stays
        {
            get => _stays;
            set
            {
                if (value != _stays)
                {
                    _stays = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReservation SelectedStay
        {
            get => _selectedStay;
            set
            {
                if (value != _selectedStay)
                {
                    _selectedStay = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                if (value != _currentDateTime)
                {
                    if (_currentDateTime.Date != value.Date)
                    {
                        InitializeData();
                    }
                    _currentDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1WriteReviewViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;

            _ratingService = new AccommodationOwnerRatingService();

            Guest = guest;
            InitializeData();
            StartTimer();
        }

        private void InitializeData()
        {
            InitializeStays();
        }

        private void InitializeStays()
        {
            List<AccommodationReservation> stays = _ratingService.GetUnratedReservationsByGuest(Guest);
            SortByStartDate(stays);
            Stays = new ObservableCollection<AccommodationReservation>(stays);
        }

        private void SortByStartDate(List<AccommodationReservation> reservations)
        {
            for (int i = 0; i < reservations.Count() - 1; i++)
            {
                for (int j = 0; j < reservations.Count() - i - 1; j++)
                {
                    if (reservations[j].DateSpan.EndDate.CompareTo(reservations[j + 1].DateSpan.EndDate) > 0)
                    {
                        AccommodationReservation swaper = reservations[j];
                        reservations[j] = reservations[j + 1];
                        reservations[j + 1] = swaper;
                    }
                }
            }
        }

        private void StartTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += LoadCurrentDateTime;
            timer.Start();
        }

        private void LoadCurrentDateTime(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
