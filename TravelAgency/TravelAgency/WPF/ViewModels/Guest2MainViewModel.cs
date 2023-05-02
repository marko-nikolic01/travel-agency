using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Observer;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest2MainViewModel: IObserver, INotifyPropertyChanged
    {
        private string imageSource;
        public string NotificationsImageSource
        {
            get => imageSource;
            set { if (value != imageSource) { imageSource = value; OnPropertyChanged(); } }
        }
        public NavigationService NavService { get; set; }
        public RelayCommand NavigateToOfferedToursCommand { get; set; }
        public RelayCommand NavigateToMyToursCommand { get; set; }
        public RelayCommand NavigateToRequestsCommand { get; set; }
        public RelayCommand NavigateToProfileCommand { get; set; }
        public RelayCommand NavigateToNotificationsCommand { get; set; }
        private int currentGuestId;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Guest2MainViewModel(NavigationService navService, int guestId)
        {
            TourOccurrenceAttendanceService attendanceService = new TourOccurrenceAttendanceService();
            Update();
            attendanceService.Subscribe(this);
            NavService = navService;
            NavigateToOfferedToursCommand = new RelayCommand(Execute_NavigateToOfferedToursCommand, CanExecute_NavigateCommand);
            NavigateToMyToursCommand = new RelayCommand(Execute_NavigateToMyToursCommand, CanExecute_NavigateCommand);
            NavigateToRequestsCommand = new RelayCommand(Execute_NavigateToRequestsCommand, CanExecute_NavigateCommand);
            NavigateToProfileCommand = new RelayCommand(Execute_NavigateToProfileCommand, CanExecute_NavigateCommand);
            NavigateToNotificationsCommand = new RelayCommand(Execute_NavigateToNotificationsCommand, CanExecute_NavigateCommand);
            currentGuestId = guestId;
        }
 
        private void Execute_NavigateToOfferedToursCommand(object obj)
        {
            Page OfferedTours = new OfferedToursView(currentGuestId);
            NavService.Navigate(OfferedTours);
        }
        private void Execute_NavigateToMyToursCommand(object obj)
        {
            Page myTours = new MyTours(currentGuestId);
            NavService.Navigate(myTours);
        }
        private void Execute_NavigateToRequestsCommand(object obj)
        {
            Page requests = new TourRequestView(currentGuestId);
            NavService.Navigate(requests);
        }
        private void Execute_NavigateToProfileCommand(object obj)
        {
            Page requests = new Guest2ProfileView();
            NavService.Navigate(requests);
        }
        private void Execute_NavigateToNotificationsCommand(object obj)
        {
            Page notifications = new Guest2NotificationsView(currentGuestId);
            NavService.Navigate(notifications);
        }
        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }

        public void Update()
        {
            TourOccurrenceAttendanceService attendanceService = new TourOccurrenceAttendanceService();
            if (attendanceService.GetAttendance(currentGuestId) != null)
            {
                NotificationsImageSource = "../../Resources/Images/IconBelled.png";
            }
            else
            {
                NotificationsImageSource = "../../Resources/Images/IconBell.png";
            }
        }
    }
}
