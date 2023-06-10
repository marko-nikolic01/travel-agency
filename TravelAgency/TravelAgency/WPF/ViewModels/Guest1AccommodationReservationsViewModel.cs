using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1AccommodationReservationsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationReservationService _reservationService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand CancelReservationCommand { get; private set; }

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

        public Guest1AccommodationReservationsViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;
            CancelReservationCommand = new MyICommand(OnCancelReservation);

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

        public void OnCancelReservation()
        {
            string messageBoxText = "Da li ste sigurni da želite da otkažete rezervaciju?\nSmeštaj: " + SelectedReservation.Accommodation.Name +
                "\nLokacija: " + SelectedReservation.Accommodation.Location.City + ", " + SelectedReservation.Accommodation.Location.Country +
                "\nTermin: " + SelectedReservation.DateSpan.StartDate.ToString() + " - " + SelectedReservation.DateSpan.EndDate.ToString();
            string caption = "Otkazivanje rezervacije";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                _reservationService.CancelReservation(SelectedReservation);
                List<AccommodationReservation> reservations = _reservationService.GetByGuest(Guest);
                SortByStartDate(reservations);
                Reservations = new ObservableCollection<AccommodationReservation>(reservations);
            }

            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
