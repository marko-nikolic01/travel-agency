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
                    
                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel;
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) break;
                    
                    {
                        Guest1AccommodationsReservationsMenuDemoViewModel guest1AccommodationsReservationsMenuDemoViewModel = new Guest1AccommodationsReservationsMenuDemoViewModel(StopDemoCommand, _demoStopper);
                        /*
                        _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
                        guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) break;

                        {
                            Guest1AccommodationSearchDemoViewModel guest1AccommodationSearchDemoViewModel = new Guest1AccommodationSearchDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1AccommodationSearchDemoViewModel;
                            guest1AccommodationSearchDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                            Guest1AccommodationReservationDemoViewModel guest1AccommodationReservationDemoViewModel = new Guest1AccommodationReservationDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1AccommodationReservationDemoViewModel;
                            guest1AccommodationReservationDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;
                        }

                        _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
                        guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) break;

                        {
                            Guest1WhereverWheneverSearchDemoViewModel guest1WhereverWheneverSearchDemoViewModel = new Guest1WhereverWheneverSearchDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1WhereverWheneverSearchDemoViewModel;
                            guest1WhereverWheneverSearchDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                            Guest1WhereverWheneverReservationDemoViewModel guest1WhereverWheneverReservationDemoViewModel = new Guest1WhereverWheneverReservationDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1WhereverWheneverReservationDemoViewModel;
                            guest1WhereverWheneverReservationDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;
                        }
                        */
                        _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
                        guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep3(); if (_demoStopper.Token.IsCancellationRequested) break;

                        {/*
                            Guest1AccommodationReservationsDemoViewModel Guest1AccommodationReservationsDemoViewModel = new Guest1AccommodationReservationsDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = Guest1AccommodationReservationsDemoViewModel;
                            Guest1AccommodationReservationsDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;

                            Guest1AccommodationReservationMoveDemoViewModel guest1AccommodationReservationMoveDemoViewModell = new Guest1AccommodationReservationMoveDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1AccommodationReservationMoveDemoViewModell;
                            guest1AccommodationReservationMoveDemoViewModell.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;*/
                        }

                        _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
                        guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep4(); if (_demoStopper.Token.IsCancellationRequested) break;

                        {/*
                            Guest1AccommodationReservationMoveRequestsDemoViewModel guest1AccommodationReservationMoveRequestsDemoViewModel = new Guest1AccommodationReservationMoveRequestsDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1AccommodationReservationMoveRequestsDemoViewModel;
                            guest1AccommodationReservationMoveRequestsDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;*/
                        }

                        _mainViewModel.CurrentViewModel = guest1AccommodationsReservationsMenuDemoViewModel;
                        guest1AccommodationsReservationsMenuDemoViewModel.ExecuteDemoStep5(); if (_demoStopper.Token.IsCancellationRequested) break;

                        {
                            Guest1ReportDemoViewModel guest1ReportDemoViewModel = new Guest1ReportDemoViewModel(StopDemoCommand, _demoStopper);
                            _mainViewModel.CurrentViewModel = guest1ReportDemoViewModel;
                            guest1ReportDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;
                        }
                    }
                    /*
                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel;
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) break;

                    {
                        Guest1ReviewsMenuDemoViewModel guest1ReviewsMenuDemoViewModel = new Guest1ReviewsMenuDemoViewModel(StopDemoCommand, _demoStopper);

                        _mainViewModel.CurrentViewModel = guest1ReviewsMenuDemoViewModel;
                        guest1ReviewsMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) break;

                        _mainViewModel.CurrentViewModel = guest1ReviewsMenuDemoViewModel;
                        guest1ReviewsMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) break;
                    }

                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel;
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep3(); if (_demoStopper.Token.IsCancellationRequested) break;

                    {
                        Guest1ForumsMenuDemoViewModel guest1ForumsMenuDemoViewModel = new Guest1ForumsMenuDemoViewModel(StopDemoCommand, _demoStopper);

                        _mainViewModel.CurrentViewModel = guest1ForumsMenuDemoViewModel;
                        guest1ForumsMenuDemoViewModel.ExecuteDemoStep1(); if (_demoStopper.Token.IsCancellationRequested) break;

                        _mainViewModel.CurrentViewModel = guest1ForumsMenuDemoViewModel;
                        guest1ForumsMenuDemoViewModel.ExecuteDemoStep2(); if (_demoStopper.Token.IsCancellationRequested) break;

                        _mainViewModel.CurrentViewModel = guest1ForumsMenuDemoViewModel;
                        guest1ForumsMenuDemoViewModel.ExecuteDemoStep3(); if (_demoStopper.Token.IsCancellationRequested) break;
                    }
                    
                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel;
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep4(); if (_demoStopper.Token.IsCancellationRequested) break;

                    {
                        Guest1NotificationsDemoViewModel guest1NotificationsDemoViewModel = new Guest1NotificationsDemoViewModel(StopDemoCommand, _demoStopper);

                        _mainViewModel.CurrentViewModel = guest1NotificationsDemoViewModel;
                        guest1NotificationsDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;
                    }
                    
                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel;
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep5(); if (_demoStopper.Token.IsCancellationRequested) break;

                    {
                        Guest1UserProfileDemoViewModel guest1UserProfileDemoViewModel = new Guest1UserProfileDemoViewModel(StopDemoCommand, _demoStopper, _mainViewModel.Guest);

                        _mainViewModel.CurrentViewModel = guest1UserProfileDemoViewModel;
                        guest1UserProfileDemoViewModel.ExecuteDemo(); if (_demoStopper.Token.IsCancellationRequested) break;
                    }

                    _mainViewModel.CurrentViewModel = guest1HomeMenuDemoViewModel;
                    guest1HomeMenuDemoViewModel.ExecuteDemoStep6(); if (_demoStopper.Token.IsCancellationRequested) break;
                    */
                } while (true);
            });
            _mainViewModel.CurrentViewModel = returnViewModel;
        }
    }
}
