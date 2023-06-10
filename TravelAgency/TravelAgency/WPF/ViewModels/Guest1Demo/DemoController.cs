using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class DemoController
    {
        private CancellationTokenSource _demoStopper;
        public MyICommand StopDemoCommand { get; private set; }
        private Guest1MainViewModel _mainViewModel;

        public DemoController(Guest1MainViewModel mainViewModel)
        {
            StopDemoCommand = new MyICommand(OnStopDemo);
            _mainViewModel = mainViewModel;
        }

        private void OnStopDemo()
        {
            _demoStopper.Cancel();
        }

        public async void ExecuteDemo(ViewModelBase returnViewModel)
        {
            _demoStopper = new CancellationTokenSource();
            await Task.Run(() =>
            {
                do
                {
                    Guest1HomeMenuDemoViewModel guest1HomeMenuDemoViewModel = new Guest1HomeMenuDemoViewModel(StopDemoCommand, _demoStopper);
                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel; _mainViewModel.SelectedTab = "HomeDemo";
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) break;
                    ExecuteAccommodationsReservationsDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel; _mainViewModel.SelectedTab = "HomeDemo";
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) break;
                    ExecuteReviewsDemo(); if (_demoStopper.Token.IsCancellationRequested) break;
                    
                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel; _mainViewModel.SelectedTab = "HomeDemo";
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep3(); if (_demoStopper.Token.IsCancellationRequested) break;
                    ExecuteForumsDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel; _mainViewModel.SelectedTab = "HomeDemo";
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep4(); if (_demoStopper.Token.IsCancellationRequested) break;
                    ExecuteNotificationsDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel; _mainViewModel.SelectedTab = "HomeDemo";
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep5(); if (_demoStopper.Token.IsCancellationRequested) break;
                    ExecuteUserProfileDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel; _mainViewModel.SelectedTab = "HomeDemo";
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep6(); if (_demoStopper.Token.IsCancellationRequested) break;
                } while (true);
            });
            _mainViewModel.IsDemoExecuting = false;
            _mainViewModel.CurrentViewModel = returnViewModel;
            _mainViewModel.SelectedTab = _mainViewModel.ReturnSelectedTab;
        }

        private void ExecuteAccommodationsReservationsDemo()
        {
            Guest1AccommodationsReservationsMenuDemoViewModel guest1AccommodationsReservationsMenuDemoViewModel = new Guest1AccommodationsReservationsMenuDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel; _mainViewModel.SelectedTab = "AccommodationsReservationsDemo";
            guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteAccommodationReservationDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
            guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteWhereverWheneverDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
            guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep3(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteAccommodationReservationMoveDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
            guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep4(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteAccommodationReservationMoveRequestsDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
            guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep5(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteReportsDemo();
        }

        public void ExecuteReviewsDemo()
        {
            Guest1ReviewsMenuDemoViewModel guest1ReviewsMenuDemoViewModel = new Guest1ReviewsMenuDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1ReviewsMenuDemoViewModel; _mainViewModel.SelectedTab = "ReviewsDemo";
            guest1ReviewsMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteAccommodationOwnerRatingDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1ReviewsMenuDemoViewModel;
            guest1ReviewsMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteGuestRatingsDemo();
        }

        public void ExecuteForumsDemo()
        {
            Guest1ForumsMenuDemoViewModel guest1ForumsMenuDemoViewModel = new Guest1ForumsMenuDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1ForumsMenuDemoViewModel; _mainViewModel.SelectedTab = "ForumsDemo";
            guest1ForumsMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteOpenForumDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1ForumsMenuDemoViewModel;
            guest1ForumsMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteMyForumsDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            _mainViewModel.CurrentViewModel = guest1ForumsMenuDemoViewModel;
            guest1ForumsMenuDemoViewModel.ExecuteDemoStep3(); if (_demoStopper.Token.IsCancellationRequested) return;
            ExecuteReadWriteForumCommentsDemo();
        }

        private void ExecuteNotificationsDemo()
        {
            Guest1NotificationsDemoViewModel guest1NotificationsDemoViewModel = new Guest1NotificationsDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1NotificationsDemoViewModel; _mainViewModel.SelectedTab = "NotificationsDemo";
            guest1NotificationsDemoViewModel.ExecuteDemo();
        }

        private void ExecuteUserProfileDemo()
        {
            Guest1UserProfileDemoViewModel guest1UserProfileDemoViewModel = new Guest1UserProfileDemoViewModel(StopDemoCommand, _demoStopper, _mainViewModel.Guest);
            _mainViewModel.CurrentViewModel = guest1UserProfileDemoViewModel; _mainViewModel.SelectedTab = "UserProfileDemo";
            guest1UserProfileDemoViewModel.ExecuteDemo();
        }

        private void ExecuteAccommodationReservationDemo()
        {
            Guest1AccommodationSearchDemoViewModel guest1AccommodationSearchDemoViewModel = new Guest1AccommodationSearchDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1AccommodationSearchDemoViewModel;
            guest1AccommodationSearchDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            Guest1AccommodationReservationDemoViewModel guest1AccommodationReservationDemoViewModel = new Guest1AccommodationReservationDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1AccommodationReservationDemoViewModel;
            guest1AccommodationReservationDemoViewModel.ExecuteDemo();
        }

        private void ExecuteWhereverWheneverDemo()
        {
            Guest1WhereverWheneverSearchDemoViewModel guest1WhereverWheneverSearchDemoViewModel = new Guest1WhereverWheneverSearchDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1WhereverWheneverSearchDemoViewModel;
            guest1WhereverWheneverSearchDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            Guest1WhereverWheneverReservationDemoViewModel guest1WhereverWheneverReservationDemoViewModel = new Guest1WhereverWheneverReservationDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1WhereverWheneverReservationDemoViewModel;
            guest1WhereverWheneverReservationDemoViewModel.ExecuteDemo();
        }

        private void ExecuteAccommodationReservationMoveDemo()
        {
            Guest1AccommodationReservationsDemoViewModel Guest1AccommodationReservationsDemoViewModel = new Guest1AccommodationReservationsDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = Guest1AccommodationReservationsDemoViewModel;
            Guest1AccommodationReservationsDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            Guest1AccommodationReservationMoveDemoViewModel guest1AccommodationReservationMoveDemoViewModell = new Guest1AccommodationReservationMoveDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1AccommodationReservationMoveDemoViewModell;
            guest1AccommodationReservationMoveDemoViewModell.ExecuteDemo();
        }

        private void ExecuteAccommodationReservationMoveRequestsDemo()
        {
            Guest1AccommodationReservationMoveRequestsDemoViewModel guest1AccommodationReservationMoveRequestsDemoViewModel = new Guest1AccommodationReservationMoveRequestsDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1AccommodationReservationMoveRequestsDemoViewModel;
            guest1AccommodationReservationMoveRequestsDemoViewModel.ExecuteDemo();
        }

        private void ExecuteReportsDemo()
        {
            Guest1ReportDemoViewModel guest1ReportDemoViewModel = new Guest1ReportDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1ReportDemoViewModel;
            guest1ReportDemoViewModel.ExecuteDemo();
        }

        private void ExecuteAccommodationOwnerRatingDemo()
        {
            Guest1RateableStaysDemoViewModel guest1RateableStaysDemoViewModel = new Guest1RateableStaysDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1RateableStaysDemoViewModel;
            guest1RateableStaysDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            Guest1WriteReviewDemoViewModel guest1WriteReviewDemoViewModel = new Guest1WriteReviewDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1WriteReviewDemoViewModel;
            guest1WriteReviewDemoViewModel.ExecuteDemo();
        }

        private void ExecuteGuestRatingsDemo()
        {
            Guest1ReviewsDemoViewModel guest1ReviewsDemoViewModel = new Guest1ReviewsDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1ReviewsDemoViewModel;
            guest1ReviewsDemoViewModel.ExecuteDemo();
        }

        private void ExecuteOpenForumDemo()
        {
            Guest1ForumLocationSearchDemoViewModel guest1ForumLocationSearchDemoViewModel = new Guest1ForumLocationSearchDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1ForumLocationSearchDemoViewModel;
            guest1ForumLocationSearchDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            Guest1OpenForumDemoViewModel guest1OpenForumDemoViewModel = new Guest1OpenForumDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1OpenForumDemoViewModel;
            guest1OpenForumDemoViewModel.ExecuteDemo();
        }

        private void ExecuteMyForumsDemo()
        {
            Guest1MyForumsDemoViewModel guest1MyForumsDemoViewModel = new Guest1MyForumsDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1MyForumsDemoViewModel;
            guest1MyForumsDemoViewModel.ExecuteDemo();
        }

        private void ExecuteReadWriteForumCommentsDemo()
        {
            Guest1ForumSearchDemoViewModel guest1ForumSearchDemoViewModel = new Guest1ForumSearchDemoViewModel(StopDemoCommand, _demoStopper);
            _mainViewModel.CurrentViewModel = guest1ForumSearchDemoViewModel;
            guest1ForumSearchDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) return;

            Guest1ReadWriteForumDemoViewModel guest1ReadWriteForumDemoViewModel = new Guest1ReadWriteForumDemoViewModel(StopDemoCommand, _demoStopper, _mainViewModel.Guest);
            _mainViewModel.CurrentViewModel = guest1ReadWriteForumDemoViewModel;
            guest1ReadWriteForumDemoViewModel.ExecuteDemo();
        }
    }
}
