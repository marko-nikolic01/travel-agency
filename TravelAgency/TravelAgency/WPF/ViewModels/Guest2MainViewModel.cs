using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest2MainViewModel: IObserver, INotifyPropertyChanged
    {
        private string imageSource;
        private static string helpText;
        private bool comboBoxOpen;
        public bool ComboBoxOpen
        {
            get => comboBoxOpen;
            set { if (value != comboBoxOpen) { comboBoxOpen = value; OnPropertyChanged(); } }
        }
        public string NotificationsImageSource
        {
            get => imageSource;
            set { if (value != imageSource) { imageSource = value; OnPropertyChanged(); } }
        }
        //pomoc za bajndovanje na staticko nadjeno na stackoverflow
        public static string HelpText
        {
            get => helpText;
            set
            {
                if (helpText == value)
                    return;

                helpText = value;
                StaticPropertyChanged?.Invoke(null, HelpTextPropertyEventArgs);
            }
        }
        private static readonly PropertyChangedEventArgs HelpTextPropertyEventArgs = new PropertyChangedEventArgs(nameof(HelpText));
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        public NavigationService NavService { get; set; }
        public RelayCommand NavigateToOfferedToursCommand { get; set; }
        public RelayCommand NavigateToMyToursCommand { get; set; }
        public RelayCommand NavigateToRequestsCommand { get; set; }
        public RelayCommand NavigateToProfileCommand { get; set; }
        public RelayCommand NavigateToNotificationsCommand { get; set; }
        public RelayCommand NavigateToSpecialRequestsCommand { get; set; }
        private int currentGuestId;
        VoucherService voucherService;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Guest2MainViewModel(NavigationService navService, int guestId)
        {
            currentGuestId = guestId;
            voucherService = new VoucherService();
            // ili samo za jednog umesto za sve
            voucherService.CheckIfGuestsWonVouchers();
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
            ComboBoxOpen = false;
            Page specialRequests = new SpecialTourRequestsView(currentGuestId, NavService);
            NavService.Navigate(specialRequests);
            UpdateHelpText("SpecialRequestsHelp");
        }
        private void Execute_NavigateToOfferedToursCommand(object obj)
        {
            Page OfferedTours = new OfferedToursView(currentGuestId, false);
            NavService.Navigate(OfferedTours);
            UpdateHelpText("OfferedToursHelp");
        }
        private void Execute_NavigateToMyToursCommand(object obj)
        {
            Page myTours = new MyTours(currentGuestId);
            NavService.Navigate(myTours);
            UpdateHelpText("MyToursHelp");
        }
        private void Execute_NavigateToRequestsCommand(object obj)
        {
            ComboBoxOpen = false;
            Page requests = new TourRequestView(currentGuestId, NavService);
            NavService.Navigate(requests);
            UpdateHelpText("RequestsHelp");
        }
        private void Execute_NavigateToProfileCommand(object obj)
        {
            ComboBoxOpen = false;
            Page requests = new Guest2ProfileView(currentGuestId);
            NavService.Navigate(requests);
            UpdateHelpText("ProfileHelp");
        }
        private void Execute_NavigateToNotificationsCommand(object obj)
        {
            Page notifications = new Guest2NotificationsView(currentGuestId);
            NavService.Navigate(notifications);
            UpdateHelpText("NotificationsHelp");
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
        public void UpdateHelpText(string filename)
        {
            string file = @"../../../Resources/HelpTexts/"+filename+".txt";
            HelpText = File.ReadAllText(file);
        }
        private bool NotificationExists()
        {
            TourOccurrenceAttendanceService attendanceService = new TourOccurrenceAttendanceService();
            TourRequestService requestService = new TourRequestService();
            CreatedTourFromStatisticService service = new CreatedTourFromStatisticService();
            if (attendanceService.GetAttendance(currentGuestId) != null || requestService.NewAcceptedRequestExists(currentGuestId) 
                || service.NewTourNotificationExists(currentGuestId) || NotificationForVoucherExist())
                return true;
            else
                return false;
        }
        private bool NotificationForVoucherExist()
        {
            WonVoucherNotification notification = voucherService.GetVoucherNotification(currentGuestId);
            if (notification != null)
                if (!notification.Seen)
                    return true;
            return false;
        }
    }
}
