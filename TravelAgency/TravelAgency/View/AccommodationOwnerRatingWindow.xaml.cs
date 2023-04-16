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
    /// Interaction logic for AccommodationOwnerRatingWindow.xaml
    /// </summary>
    public partial class AccommodationOwnerRatingWindow : Window
    {
        public AccommodationReservation Stay { get; set; }
        public AccommodationOwnerRating Rating { get; set; }
        public AccommodationOwnerRatingRepository ratingRepository;
        public AccommodationOwnerRatingWindow(AccommodationOwnerRatingRepository ratingRepository, AccommodationReservation stay)
        {
            InitializeComponent();
            this.ratingRepository = ratingRepository;

            Stay = stay;
            Rating = new AccommodationOwnerRating(Stay);

        }

        private void RateAccommodationOwner(object sender, RoutedEventArgs e)
        {
            Rating.AccommodationCleanliness = Convert.ToInt32(cleanlinessNumberUpDown.Value);
            Rating.AccommodationComfort = Convert.ToInt32(comfortNumberUpDown.Value);
            Rating.AccommodationLocation = Convert.ToInt32(locationNumberUpDown.Value);
            Rating.OwnerCorrectness = Convert.ToInt32(corectnessNumberUpDown.Value);
            Rating.OwnerResponsiveness = Convert.ToInt32(responsivenessNumberUpDown.Value);
            Rating.Comment = commentTextBox.Text;
            if (Rating.IsValid)
            {
                ratingRepository.Save(Rating);
                Guest1Main.Stays.Remove(Stay);
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
    }
}
