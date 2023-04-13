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
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationGuestRatingWindow.xaml
    /// </summary>
    public partial class AccommodationGuestRatingWindow : Window
    {
        public ObservableCollection<AccommodationReservation> UnratedReservations { get; set; }
        public ObservableCollection<AccommodationGuestRating> AccommodationGuestRatings { get; set; }

        public AccommodationReservation SelectedUnratedReservation { get; set; }

        public AccommodationGuestRating NewAccommodationGuestRating { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationReservationRepository _AccommodationReservationRepository;
        private readonly AccommodationGuestRatingRepository _AccommodationGuestRatingRepository;
        
        public AccommodationGuestRatingWindow(User loggedInUser, AccommodationReservationRepository accommodationReservationRepository)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = loggedInUser;

            _AccommodationReservationRepository = accommodationReservationRepository;
            _AccommodationGuestRatingRepository = new AccommodationGuestRatingRepository(accommodationReservationRepository.GetAll());

            UnratedReservations = new ObservableCollection<AccommodationReservation>(_AccommodationReservationRepository.GetUnrated(_AccommodationGuestRatingRepository.GetAll()));
            AccommodationGuestRatings = new ObservableCollection<AccommodationGuestRating>(_AccommodationGuestRatingRepository.GetAllByOwner(loggedInUser));

            NewAccommodationGuestRating = new AccommodationGuestRating();
        }

        private void CreateRating_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnratedReservation == null)
            {
                MessageBox.Show("Select a reservation first!");
                return;
            }

            NewAccommodationGuestRating.AccommodationReservationId = SelectedUnratedReservation.Id;
            NewAccommodationGuestRating.AccommodationReservation = SelectedUnratedReservation;
            NewAccommodationGuestRating.Cleanliness = (int)CleanlinessSlider.Value;
            NewAccommodationGuestRating.Compliance = (int)ComplianceSlider.Value;
            NewAccommodationGuestRating.Noisiness = (int)NoisinessSlider.Value;
            NewAccommodationGuestRating.Friendliness = (int)FriendlinessSlider.Value;
            NewAccommodationGuestRating.Responsivenes = (int)ResponsivenesSlider.Value;
            NewAccommodationGuestRating.Comment = CommentTextBox.Text;

            _AccommodationGuestRatingRepository.Save(NewAccommodationGuestRating);

            NewAccommodationGuestRating = new AccommodationGuestRating();

            CleanlinessSlider.Value = 1;
            ComplianceSlider.Value = 1;
            NoisinessSlider.Value = 1;
            FriendlinessSlider.Value = 1;
            ResponsivenesSlider.Value = 1;
            CommentTextBox.Text = "";

            UnratedReservations.Clear();
            foreach (var reservation in _AccommodationReservationRepository.GetUnrated(_AccommodationGuestRatingRepository.GetAll()))
            {
                UnratedReservations.Add(reservation);
            }

            AccommodationGuestRatings.Clear();
            foreach (var rating in _AccommodationGuestRatingRepository.GetAll())
            {
                AccommodationGuestRatings.Add(rating);
            }

            MessageBox.Show("Rating has been created successfully!");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
