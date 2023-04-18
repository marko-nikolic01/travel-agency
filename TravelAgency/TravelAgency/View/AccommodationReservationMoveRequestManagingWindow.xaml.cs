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
using TravelAgency.Services;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationMoveRequestManagingWindow.xaml
    /// </summary>
    public partial class AccommodationReservationMoveRequestManagingWindow : Window
    {
        public User LoggedInUser { get; set; }
        public AccommodationReservationMoveRequest SelectedMoveRequest { get; set; }

        public AccommodationReservationMoveService moveReqestService { get; set; }

        public AccommodationReservationMoveRequestManagingWindow(User loggedInUser, AccommodationReservationMoveRequest selectedMoveRequest)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = loggedInUser;
            SelectedMoveRequest = selectedMoveRequest;

            moveReqestService = new AccommodationReservationMoveService();

            SetAvailability();
        }

        private void SetAvailability()
        {
            if (moveReqestService.CanResevationBeMoved(SelectedMoveRequest))
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
            moveReqestService.AcceptMoveRequest(SelectedMoveRequest);

            OwnerMain.AccommodationReservationMoveRequests.Remove(SelectedMoveRequest);

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
                moveReqestService.RejectMoveRequest(SelectedMoveRequest);

                OwnerMain.AccommodationReservationMoveRequests.Remove(SelectedMoveRequest);

                Close();
            }
        }
    }
}
