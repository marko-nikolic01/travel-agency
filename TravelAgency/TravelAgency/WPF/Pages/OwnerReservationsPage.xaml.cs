using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerReservationsPage.xaml
    /// </summary>
    public partial class OwnerReservationsPage : Page
    {
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationReservationService accommodationReservationService;
        private AccommodationReservationMoveService accommodationReservationMoveService;

        public ObservableCollection<AccommodationReservation> AccommodationReservations { get; set; }
        public ObservableCollection<AccommodationReservationMoveRequestWithAvailabilityDTO> MoveRequests { get; set; }

        public OwnerReservationsPage()
        {
            InitializeComponent();
            DataContext = this;

            userService = new UserService();
            accommodationReservationService = new AccommodationReservationService();
            accommodationReservationMoveService = new AccommodationReservationMoveService();

            LoggedInUser = userService.GetLoggedInUser();

            AccommodationReservations = new ObservableCollection<AccommodationReservation>(accommodationReservationService.GetActiveByOwner(LoggedInUser));
            MoveRequests = new ObservableCollection<AccommodationReservationMoveRequestWithAvailabilityDTO>(accommodationReservationMoveService.GetMoveRequestsWithAvailability(LoggedInUser));
        }
    }
}
