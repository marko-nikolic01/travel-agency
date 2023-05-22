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

namespace TravelAgency.WPF.ViewModels
{
    public class wAccommodationReservationsViewModel: INotifyPropertyChanged
    {
        private AccommodationReservationService _reservationService;

        public User Guest { get; set; }
        private ObservableCollection<AccommodationReservation> _reservations;
        private AccommodationReservation _selectedReservation;

        public ObservableCollection<AccommodationReservation> Reservations
        {
            get => _reservations;
            set
            {
                if (value != _reservations)
                {
                    _reservations = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                if (value != _selectedReservation)
                {
                    _selectedReservation = value;
                    OnPropertyChanged();
                }
            }
        }

        public wAccommodationReservationsViewModel(User guest)
        {
            _reservationService = new AccommodationReservationService();

            Guest = guest;
            InitializeData();
        }

        private void InitializeData()
        {
            InitializeReservations();
        }

        private void InitializeReservations()
        {
            List<AccommodationReservation> reservations = _reservationService.GetByGuest(Guest);
            SortByStartDate(reservations);
            Reservations = new ObservableCollection<AccommodationReservation>(reservations);
        }

        private void SortByStartDate(List<AccommodationReservation> reservations)
        {
            for (int i = 0; i < reservations.Count() - 1; i++)
            {
                for (int j = 0; j < reservations.Count() - i - 1; j++) 
                {
                    if (reservations[j].DateSpan.StartDate.CompareTo(reservations[j + 1].DateSpan.StartDate) < 0)
                    {
                        AccommodationReservation swaper = reservations[j];
                        reservations[j] = reservations[j + 1];
                        reservations[j + 1] = swaper;
                    }
                }
            }
        }

        public void CancelReservation()
        {
            _reservationService.CancelReservation(SelectedReservation);
            List<AccommodationReservation> reservations = _reservationService.GetByGuest(Guest);
            SortByStartDate(reservations);
            Reservations = new ObservableCollection<AccommodationReservation>(reservations);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
