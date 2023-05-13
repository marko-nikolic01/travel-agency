using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public User Guest { get; set; }
        public MyICommand<string> NavigationCommand { get; private set; }
        private Guest1HomeMenuViewModel guest1HomeMenuViewModel;
        private Guest1AccommodationsReservationsMenuViewModel guest1AccommodationsReservationsMenuViewModel;
        private Guest1ReviewsMenuViewModel guest1ReviewsMenuViewModel;
        private Guest1ForumsMenuViewModel guest1ForumsMenuViewModel;
        private Guest1AccommodationSearchViewModel guest1AccommodationSearchViewModel;
        private Guest1AccommodationReservationsViewModel guest1AccommodationReservationsViewModel;
        private Guest1AccommodationReservationMoveRequestsViewModel guest1AccommodationReservationMoveRequestsViewModel;
        private ViewModelBase currentViewModel;
        private ViewModelBase previousViewModel;
        private string selectedTab;

        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                if (value != currentViewModel)
                {
                    currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ViewModelBase PreviousViewModel
        {
            get => previousViewModel;
            set
            {
                if (value != previousViewModel)
                {
                    previousViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedTab
        {
            get => selectedTab;
            set
            {
                if (value != selectedTab)
                {
                    selectedTab = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1MainViewModel(User guest)
        {
            Guest = guest;
            NavigationCommand = new MyICommand<string>(OnNavigation);
            guest1HomeMenuViewModel = new Guest1HomeMenuViewModel(NavigationCommand);
            guest1AccommodationsReservationsMenuViewModel = new Guest1AccommodationsReservationsMenuViewModel(NavigationCommand);
            guest1ReviewsMenuViewModel = new Guest1ReviewsMenuViewModel(NavigationCommand);
            guest1ForumsMenuViewModel = new Guest1ForumsMenuViewModel(NavigationCommand);
            CurrentViewModel = guest1HomeMenuViewModel;
            SelectedTab = "Home";


        }

        public void OnNavigation(string destination)
        {
            switch (destination)
            {
                case "guest1HomeMenuViewModel":
                    CurrentViewModel = guest1HomeMenuViewModel;
                    SelectedTab = "Home";
                    break;
                case "guest1AccommodationsReservationsMenuViewModel":
                    CurrentViewModel = guest1AccommodationsReservationsMenuViewModel;
                    SelectedTab = "AccommodationsReservations";
                    break;
                case "guest1ReviewsMenuViewModel":
                    CurrentViewModel = guest1ReviewsMenuViewModel;
                    SelectedTab = "Reviews";
                    break;
                case "guest1ForumsMenuViewModel":
                    CurrentViewModel = guest1ForumsMenuViewModel;
                    SelectedTab = "Forums";
                    break;
                case "guest1AccommodationSearchViewModel":
                    guest1AccommodationSearchViewModel = new Guest1AccommodationSearchViewModel(NavigationCommand);
                    CurrentViewModel = guest1AccommodationSearchViewModel;
                    break;
                case "guest1AccommodationReservationViewModel":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1AccommodationReservationViewModel(NavigationCommand, Guest, guest1AccommodationSearchViewModel.SelectedAccommodation);
                    break;
                case "guest1AccommodationReservationsViewModel":
                    guest1AccommodationReservationsViewModel = new Guest1AccommodationReservationsViewModel(NavigationCommand, Guest);
                    CurrentViewModel = guest1AccommodationReservationsViewModel;
                    break;
                case "guest1AccommodationReservationMoveViewModel":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1AccommodationReservationMoveViewModel(NavigationCommand, guest1AccommodationReservationsViewModel.SelectedReservation);
                    break;
                case "guest1AccommodationReservationMoveRequestsViewModel":
                    guest1AccommodationReservationMoveRequestsViewModel = new Guest1AccommodationReservationMoveRequestsViewModel(NavigationCommand, Guest);
                    CurrentViewModel = guest1AccommodationReservationMoveRequestsViewModel;
                    break;
                case "previousViewModel":
                    CurrentViewModel = PreviousViewModel;
                    break;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
