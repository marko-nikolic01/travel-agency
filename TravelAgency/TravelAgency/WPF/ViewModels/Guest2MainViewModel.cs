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
        public RelayCommand NavigateToSpecialRequestsCommand { get; set; }
        private int currentGuestId;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Guest2MainViewModel(NavigationService navService, int guestId)
        {
            currentGuestId = guestId;
            TourOccurrenceAttendanceService attendanceService = new TourOccurrenceAttendanceService();
            attendanceService.Subscribe(this);
            NavService = navService;
            NavigateToOfferedToursCommand = new RelayCommand(Execute_NavigateToOfferedToursCommand, CanExecute_NavigateCommand);
            NavigateToMyToursCommand = new RelayCommand(Execute_NavigateToMyToursCommand, CanExecute_NavigateCommand);
            NavigateToRequestsCommand = new RelayCommand(Execute_NavigateToRequestsCommand, CanExecute_NavigateCommand);
            NavigateToProfileCommand = new RelayCommand(Execute_NavigateToProfileCommand, CanExecute_NavigateCommand);
            NavigateToNotificationsCommand = new RelayCommand(Execute_NavigateToNotificationsCommand, CanExecute_NavigateCommand);
            NavigateToSpecialRequestsCommand = new RelayCommand(Execute_NavigateToSpecialRequestsCommand, CanExecute_NavigateCommand);
            Update();
        }

        private void Execute_NavigateToSpecialRequestsCommand(object obj)
        {
            Page specialRequests = new SpecialTourRequestsView(currentGuestId);
            NavService.Navigate(specialRequests);
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
            Page requests = new Guest2ProfileView(currentGuestId);
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
            if (NotificationExists())
            {
                NotificationsImageSource = "../../Resources/Images/belled.png";
            }
            else
            {
                NotificationsImageSource = "../../Resources/Images/bell.png";
            }
        }
        private bool NotificationExists()
        {
            TourOccurrenceAttendanceService attendanceService = new TourOccurrenceAttendanceService();
            TourRequestService requestService = new TourRequestService();
            CreatedTourFromStatisticService service = new CreatedTourFromStatisticService();
            if (attendanceService.GetAttendance(currentGuestId) != null || requestService.NewAcceptedRequestExists(currentGuestId) 
                || service.NewTourNotificationExists(currentGuestId))
                return true;
            else
                return false;
        }
    }
}
