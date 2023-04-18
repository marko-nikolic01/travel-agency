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
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationMoveRequestManagingWindow.xaml
    /// </summary>
    public partial class AccommodationReservationMoveRequestManagingWindow : Window
    {
        public AccommodationReservationMoveRequestViewModel MoveRequestViewModel { get; set; }

        public AccommodationReservationMoveRequestManagingWindow(AccommodationReservationMoveRequest selectedMoveRequest)
        {
            InitializeComponent();
            MoveRequestViewModel = new AccommodationReservationMoveRequestViewModel(selectedMoveRequest);
            DataContext = MoveRequestViewModel;

            SetAvailability();
        }

        private void SetAvailability()
        {
            if (MoveRequestViewModel.CanSelectedReservationBeMoved())
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
            MoveRequestViewModel.AcceptSelectedMoveRequest();

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
                MoveRequestViewModel.RejectSelectedMoveRequest();

                Close();
            }
        }
    }
}
