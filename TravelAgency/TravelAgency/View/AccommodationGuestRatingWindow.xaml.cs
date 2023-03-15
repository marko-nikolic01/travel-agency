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
            _AccommodationGuestRatingRepository = new AccommodationGuestRatingRepository();

            UnratedReservations = new ObservableCollection<AccommodationReservation>(_AccommodationReservationRepository.GetUnrated(_AccommodationGuestRatingRepository.GetAll()));

            NewAccommodationGuestRating = new AccommodationGuestRating();

            MessageBox.Show(UnratedReservations.Count.ToString());
        }

        private void CreateRating_Click(object sender, RoutedEventArgs e)
        {
            NewAccommodationGuestRating.AccommodationReservationId = SelectedUnratedReservation.Id;

            _AccommodationGuestRatingRepository.Save(NewAccommodationGuestRating);

            MessageBox.Show("Ocena uspesno sacuvana");

            CleanlinessSlider.Value = 1;
            RuleComplianceSlider.Value = 1;
            CommentTextBox.Text = "";

            UnratedReservations.Clear();

            foreach (var reservation in _AccommodationReservationRepository.GetUnrated(_AccommodationGuestRatingRepository.GetAll()))
            {
                UnratedReservations.Add(reservation);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
