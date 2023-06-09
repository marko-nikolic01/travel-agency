using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels.Guest1Demo;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private SuperGuestService _superGuestService;
        private NotificationService _notificationService;
        private DemoController _demoController;

        public User Guest { get; set; }
        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand StartDemoCommand { get; private set; }

        private Guest1HomeMenuViewModel _guest1HomeMenuViewModel;
        private Guest1AccommodationsReservationsMenuViewModel _guest1AccommodationsReservationsMenuViewModel;
        private Guest1ReviewsMenuViewModel _guest1ReviewsMenuViewModel;
        private Guest1ForumsMenuViewModel _guest1ForumsMenuViewModel;
        private Guest1AccommodationSearchViewModel _guest1AccommodationSearchViewModel;
        private Guest1AccommodationReservationsViewModel _guest1AccommodationReservationsViewModel;
        private Guest1AccommodationReservationMoveRequestsViewModel _guest1AccommodationReservationMoveRequestsViewModel;
        private Guest1RateableStaysViewModel _guest1RateableStaysViewModel;
        private Guest1WhereverWheneverSearchViewModel _guest1WhereverWheneverSearchViewModel;
        private Guest1ForumLocationSearchViewModel _guest1ForumLocationSearchViewModel;
        private Guest1MyForumsViewModel _guest1MyForumsViewModel;
        private Guest1ForumSearchViewModel _guest1ForumSearchViewModel;

        private ViewModelBase _currentViewModel;
        private ViewModelBase _previousViewModel;
        private string _selectedTab;
        private string _currentDateTime;
        private DateTime _date;
        private bool _hasNotifications;

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
                    if (_selectedTab == "Notifications")
                    {
                        HasNotifications = false;
                        _notificationService.MarkAllAsSeen(_notificationService.GetNotificationsByUser(Guest));
                    }
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



        public bool HasNotifications
        {
            get => _hasNotifications;
            set
            {
                if (value != _hasNotifications)
                {
                    _hasNotifications = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1MainViewModel(User guest)
        {
            _superGuestService = new SuperGuestService();
            _notificationService = new NotificationService();
            _demoController = new DemoController(this);

            Guest = guest;
            _superGuestService.CheckSuperGuest(Guest);
            foreach (Notification notification in _notificationService.GetNotificationsByUser(Guest))
            {
                if (!notification.Seen) 
                {
                    HasNotifications = true;
                }
            }

            NavigationCommand = new MyICommand<string>(OnNavigation);
            StartDemoCommand = new MyICommand(OnStartDemo);
            _guest1HomeMenuViewModel = new Guest1HomeMenuViewModel(NavigationCommand, StartDemoCommand);
            _guest1AccommodationsReservationsMenuViewModel = new Guest1AccommodationsReservationsMenuViewModel(NavigationCommand);
            _guest1ReviewsMenuViewModel = new Guest1ReviewsMenuViewModel(NavigationCommand);
            _guest1ForumsMenuViewModel = new Guest1ForumsMenuViewModel(NavigationCommand);

            CurrentViewModel = _guest1HomeMenuViewModel;
            SelectedTab = "Home";
            StartTimer();
        }

        private async void OnStartDemo()
        {
            _demoController.ExecuteDemo(CurrentViewModel);
        }

        public /*async*/ void OnNavigation(string destination)
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
                    /*_canceller = new CancellationTokenSource();
                    await Task.Run(() =>
                    {
                        do
                        {
                            Console.WriteLine("Hello, world");
                            if (_canceller.Token.IsCancellationRequested)
                                break;

                        } while (true);
                    });*/
                    CurrentViewModel = _guest1ForumsMenuViewModel;
                    SelectedTab = "Forums";
                    break;
                case "guest1NotificationsViewModel":
                    CurrentViewModel = new Guest1NotificationsViewModel(NavigationCommand, Guest);
                    SelectedTab = "Notifications";
                    break;
                case "guest1UserProfileViewModel":
                    CurrentViewModel = new Guest1UserProfileViewModel(NavigationCommand, Guest);
                    SelectedTab = "UserProfile";
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
                case "guest1ReviewsViewModel":
                    CurrentViewModel = new Guest1ReviewsViewModel(NavigationCommand, Guest);
                    break;
                case "guest1WhereverWheneverSearchViewModel":
                    _guest1WhereverWheneverSearchViewModel = new Guest1WhereverWheneverSearchViewModel(NavigationCommand);
                    CurrentViewModel = _guest1WhereverWheneverSearchViewModel;
                    break;
                case "guest1WhereverWheneverReservationViewModel":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1WhereverWheneverReservationViewModel(NavigationCommand, Guest, _guest1WhereverWheneverSearchViewModel.SelectedAccommodation, _guest1WhereverWheneverSearchViewModel.LastUsedSearchFilter);
                    break;
                case "guest1ForumLocationSearchViewModel":
                    _guest1ForumLocationSearchViewModel = new Guest1ForumLocationSearchViewModel(NavigationCommand);
                    CurrentViewModel = _guest1ForumLocationSearchViewModel;
                    break;
                case "guest1OpenForumViewModel":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1OpenForumViewModel(NavigationCommand, Guest, _guest1ForumLocationSearchViewModel.SelectedLocation);
                    break;
                case "guest1MyForumsViewModel":
                    _guest1MyForumsViewModel = new Guest1MyForumsViewModel(NavigationCommand, Guest);
                    CurrentViewModel = _guest1MyForumsViewModel;
                    break;
                case "guest1ForumSearchViewModel":
                    _guest1ForumSearchViewModel = new Guest1ForumSearchViewModel(NavigationCommand);
                    CurrentViewModel = _guest1ForumSearchViewModel;
                    break;
                case "guest1ReadForumViewModel1":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1ReadForumViewModel1(NavigationCommand, _guest1ForumSearchViewModel.SelectedForum);
                    break;
                case "guest1ReadForumViewModel2":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1ReadForumViewModel2(NavigationCommand, _guest1MyForumsViewModel.SelectedForum);
                    break;
                case "guest1ReadWriteForumViewModel1":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1ReadWriteForumViewModel1(NavigationCommand, Guest, _guest1ForumSearchViewModel.SelectedForum);
                    break;
                case "guest1ReadWriteForumViewModel2":
                    PreviousViewModel = CurrentViewModel;
                    CurrentViewModel = new Guest1ReadWriteForumViewModel2(NavigationCommand, Guest, _guest1MyForumsViewModel.SelectedForum);
                    break;
                case "guest1ReportViewModel":
                    CurrentViewModel = new Guest1ReportViewModel(NavigationCommand, Guest);
                    break;
                case "previousViewModel":
                    CurrentViewModel = PreviousViewModel;
                    break;
            }
        }

        private void StartTimer()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += LoadCurrentDateTime;
            timer.Start();
        }

        private void LoadCurrentDateTime(object sender, EventArgs e)
        {
            DateTime newTime = DateTime.Now;
            if (_date.Date != newTime.Date) 
            {
                _superGuestService.CheckSuperGuest(Guest);
            }
            _date = newTime;
            CurrentDateTime = _date.ToString("dd/MM/yyyy     hh:mm:ss tt");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
