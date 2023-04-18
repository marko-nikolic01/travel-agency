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
        public AccommodationReservationRepository accommodationReservationRepository { get; set; }

        public User LoggedInUser { get; set; }
        public AccommodationReservationMoveRequest SelectedMoveRequest { get; set; }

        public AccommodationReservationMoveRequestManagingWindow(User loggedInUser, AccommodationReservationRepository accommodationReservationRepository, AccommodationReservationMoveRequestRepository accommodationReservationMoveRequestRepository, AccommodationReservationMoveRequest selectedMoveRequest)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = loggedInUser;
            this.accommodationReservationMoveRequestRepository = accommodationReservationMoveRequestRepository;
            this.accommodationReservationRepository = accommodationReservationRepository;
            SelectedMoveRequest = selectedMoveRequest;

            SetAvailability();
        }

        private void SetAvailability()
        {
            var isAvailable = accommodationReservationRepository.CanResevationBeMoved(SelectedMoveRequest);

            if (isAvailable)
            {
                AvailabilityTextBlock.Text = "Available";
            }
            else
            {
                AvailabilityTextBlock.Text = "Reserved";
            }
        }

        private void AcceptMoveRequest_Click(object sender, RoutedEventArgs e)
        {
            accommodationReservationMoveRequestRepository.Delete(SelectedMoveRequest);
            SelectedMoveRequest.Status = AccommodationReservationMoveRequestStatus.ACCEPTED;
            accommodationReservationMoveRequestRepository.Save(SelectedMoveRequest);
            OwnerMain.AccommodationReservationMoveRequests.Remove(SelectedMoveRequest);

            accommodationReservationRepository.UpdateDateSpan(SelectedMoveRequest.Reservation, SelectedMoveRequest.DateSpan);

            accommodationReservationRepository.DeleteOverlappingReservations(SelectedMoveRequest.Reservation);

            Close();
        }

        private void RejectMoveRequest_Click(object sender, RoutedEventArgs e)
        {
            if (ExplanationTextBox.Text == string.Empty)
            {
                MessageBox.Show("Enter explanation!");
                return;
            }
            else
            {
                accommodationReservationMoveRequestRepository.Delete(SelectedMoveRequest);
                SelectedMoveRequest.Status = AccommodationReservationMoveRequestStatus.REJECTED;
                SelectedMoveRequest.RejectionExplanation = ExplanationTextBox.Text;
                accommodationReservationMoveRequestRepository.Save(SelectedMoveRequest);
                OwnerMain.AccommodationReservationMoveRequests.Remove(SelectedMoveRequest);
                Close();
            }
        }
    }
}
