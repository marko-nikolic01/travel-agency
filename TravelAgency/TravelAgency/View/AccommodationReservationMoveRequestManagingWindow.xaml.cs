using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationMoveRequestManagingWindow.xaml
    /// </summary>
    public partial class AccommodationReservationMoveRequestManagingWindow : Window
    {
        public AccommodationReservationMoveRequestRepository accommodationReservationMoveRequestRepository { get; set; }

        public User LoggedInUser { get; set; }
        public AccommodationReservationMoveRequest SelectedMoveRequest { get; set; }

        public AccommodationReservationMoveRequestManagingWindow(User loggedInUser, AccommodationReservationMoveRequestRepository accommodationReservationMoveRequestRepository, AccommodationReservationMoveRequest selectedMoveRequest)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = loggedInUser;
            this.accommodationReservationMoveRequestRepository = accommodationReservationMoveRequestRepository;
            SelectedMoveRequest = selectedMoveRequest;
        }
    }
}
