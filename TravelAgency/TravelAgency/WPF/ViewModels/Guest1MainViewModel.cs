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
                    CurrentViewModel = new Guest1AccommodationSearchViewModel(NavigationCommand);
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
