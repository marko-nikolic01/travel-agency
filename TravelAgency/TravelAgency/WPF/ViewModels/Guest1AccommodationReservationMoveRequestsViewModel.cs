using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1AccommodationReservationMoveRequestsViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationReservationMoveService _moveService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand CancelReservationCommand { get; private set; }

        public User Guest { get; set; }
        private ObservableCollection<AccommodationReservationMoveRequest> _moveRequests;
        private AccommodationReservationMoveRequest _selectedMoveRequest;

        public ObservableCollection<AccommodationReservationMoveRequest> MoveRequests
        {
            get => _moveRequests;
            set
            {
                if (value != _moveRequests)
                {
                    _moveRequests = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReservationMoveRequest SelectedMoveRequest
        {
            get => _selectedMoveRequest;
            set
            {
                if (value != _selectedMoveRequest)
                {
                    _selectedMoveRequest = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1AccommodationReservationMoveRequestsViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;

            _moveService = new AccommodationReservationMoveService();

            Guest = guest;
            InitializeData();
        }

        private void InitializeData()
        {
            InitializeMoveRequests();
        }

        private void InitializeMoveRequests()
        {
            List<AccommodationReservationMoveRequest> moveRequests =_moveService.GetRequestsByGuest(Guest);
            SortByStartDate(moveRequests);
            MoveRequests = new ObservableCollection<AccommodationReservationMoveRequest>(moveRequests);
        }

        private void SortByStartDate(List<AccommodationReservationMoveRequest> moveRequests)
        {
            for (int i = 0; i < moveRequests.Count() - 1; i++)
            {
                for (int j = 0; j < moveRequests.Count() - i - 1; j++)
                {
                    if (((moveRequests[j].DateSpan.StartDate.CompareTo(moveRequests[j + 1].DateSpan.StartDate) < 0) && (moveRequests[j].DateSpan.StartDate.CompareTo(moveRequests[j + 1].Reservation.DateSpan.StartDate) < 0) ||
                        ((moveRequests[j].Reservation.DateSpan.StartDate.CompareTo(moveRequests[j + 1].DateSpan.StartDate) < 0) && (moveRequests[j].Reservation.DateSpan.StartDate.CompareTo(moveRequests[j + 1].Reservation.DateSpan.StartDate) < 0))))
                    {
                        AccommodationReservationMoveRequest swaper = moveRequests[j];
                        moveRequests[j] = moveRequests[j + 1];
                        moveRequests[j + 1] = swaper;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
