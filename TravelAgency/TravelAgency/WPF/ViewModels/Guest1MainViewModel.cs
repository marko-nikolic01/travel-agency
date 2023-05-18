using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public User Guest { get; set; }
        public MyICommand<string> NavigationCommand { get; private set; }
        private Guest1HomeMenuViewModel _guest1HomeMenuViewModel;
        private Guest1AccommodationsReservationsMenuViewModel _guest1AccommodationsReservationsMenuViewModel;
        private Guest1ReviewsMenuViewModel _guest1ReviewsMenuViewModel;
        private Guest1ForumsMenuViewModel _guest1ForumsMenuViewModel;
        private Guest1AccommodationSearchViewModel _guest1AccommodationSearchViewModel;
        private Guest1AccommodationReservationsViewModel _guest1AccommodationReservationsViewModel;
        private Guest1AccommodationReservationMoveRequestsViewModel _guest1AccommodationReservationMoveRequestsViewModel;
        private Guest1RateableStaysViewModel _guest1RateableStaysViewModel;
        private ViewModelBase _currentViewModel;
        private ViewModelBase _previousViewModel;
        private string _selectedTab;
        private string _currentDateTime;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (value != _currentViewModel)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewModelBase PreviousViewModel
        {
            get => _previousViewModel;
            set
            {
                if (value != _previousViewModel)
                {
                    _previousViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (value != _selectedTab)
                {
                    _selectedTab = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                if (value != _currentDateTime)
                {
                    _currentDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1MainViewModel(User guest)
        {
            Guest = guest;
            NavigationCommand = new MyICommand<string>(OnNavigation);
            _guest1HomeMenuViewModel = new Guest1HomeMenuViewModel(NavigationCommand);
            _guest1AccommodationsReservationsMenuViewModel = new Guest1AccommodationsReservationsMenuViewModel(NavigationCommand);
            _guest1ReviewsMenuViewModel = new Guest1ReviewsMenuViewModel(NavigationCommand);
            _guest1ForumsMenuViewModel = new Guest1ForumsMenuViewModel(NavigationCommand);
            CurrentViewModel = _guest1HomeMenuViewModel;
            SelectedTab = "Home";
            StartTimer();
        }

        public void OnNavigation(string destination)
        {
            switch (destination)
            {
                case "guest1HomeMenuViewModel":
                    CurrentViewModel = _guest1HomeMenuViewModel;
                    SelectedTab = "Home";
                    break;
                case "guest1AccommodationsReservationsMenuViewModel":
                    CurrentViewModel = _guest1AccommodationsReservationsMenuViewModel;
                    SelectedTab = "AccommodationsReservations";
                    break;
                case "guest1ReviewsMenuViewModel":
                    CurrentViewModel = _guest1ReviewsMenuViewModel;
                    SelectedTab = "Reviews";
                    break;
                case "guest1ForumsMenuViewModel":
                    CurrentViewModel = _guest1ForumsMenuViewModel;
                    SelectedTab = "Forums";
                    break;
                case "guest1AccommodationSearchViewModel":
                    _guest1AccommodationSearchViewModel = new Guest1AccommodationSearchViewModel(NavigationCommand);
                    CurrentViewModel = _guest1AccommodationSearchViewModel;
                    break;
                case "guest1AccommodationReservationViewModel":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1AccommodationReservationViewModel(NavigationCommand, Guest, _guest1AccommodationSearchViewModel.SelectedAccommodation);
                    break;
                case "guest1AccommodationReservationsViewModel":
                    _guest1AccommodationReservationsViewModel = new Guest1AccommodationReservationsViewModel(NavigationCommand, Guest);
                    CurrentViewModel = _guest1AccommodationReservationsViewModel;
                    break;
                case "guest1AccommodationReservationMoveViewModel":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1AccommodationReservationMoveViewModel(NavigationCommand, _guest1AccommodationReservationsViewModel.SelectedReservation);
                    break;
                case "guest1AccommodationReservationMoveRequestsViewModel":
                    _guest1AccommodationReservationMoveRequestsViewModel = new Guest1AccommodationReservationMoveRequestsViewModel(NavigationCommand, Guest);
                    CurrentViewModel = _guest1AccommodationReservationMoveRequestsViewModel;
                    break;
                case "guest1RateableStaysViewModel":
                    _guest1RateableStaysViewModel = new Guest1RateableStaysViewModel(NavigationCommand, Guest);
                    CurrentViewModel = _guest1RateableStaysViewModel;
                    break;
                case "guest1WriteReviewViewModel":
                    CurrentViewModel = new Guest1WriteReviewViewModel(NavigationCommand, _guest1RateableStaysViewModel.SelectedStay);
                    break;
                case "previousViewModel":
                    CurrentViewModel = PreviousViewModel;
                    break;
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
            CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy     hh:mm:ss tt");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
