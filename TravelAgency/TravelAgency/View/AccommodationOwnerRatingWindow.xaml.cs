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
    /// Interaction logic for AccommodationOwnerRatingWindow.xaml
    /// </summary>
    public partial class AccommodationOwnerRatingWindow : Window
    {
        public AccommodationOwnerRatingViewModel ViewModel { get; set; }
        public AccommodationOwnerRatingWindow(AccommodationReservation stay)
        {
            InitializeComponent();
            ViewModel = new AccommodationOwnerRatingViewModel(stay);
            this.DataContext = ViewModel;

            cleanlinessNumberUpDown.Value = 1;
            comfortNumberUpDown.Value = 1;
            locationNumberUpDown.Value = 1;
            corectnessNumberUpDown.Value = 1;
            responsivenessNumberUpDown.Value = 1;
            commentTextBox.Text = "";
            photoTextBox.Text = "";
        }

        private void RateAccommodationOwner(object sender, RoutedEventArgs e)
        {
            if (ViewModel.RateAccommodationOwner())
            {
                Close();
            }
            else
            {
                string message = "Rating is not valid.";
                System.Windows.MessageBox.Show(message);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddPhoto(object sender, RoutedEventArgs e)
        {
            ViewModel.AddPhoto();
            photoTextBox.Clear();
        }
    }
}
