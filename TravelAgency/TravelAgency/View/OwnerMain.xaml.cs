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
    /// Interaction logic for OwnerMain.xaml
    /// </summary>
    public partial class OwnerMain : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }
        public static ObservableCollection<AccommodationOwnerRating> AccommodationOwnerRatings { get; set; }
        public static ObservableCollection<AccommodationReservationMoveRequest> AccommodationReservationMoveRequests { get; set; }

        public AccommodationReservationMoveRequest SelectedMoveRequest { get; set; }

        public Accommodation? SelectedAccommodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly UserRepository userRepository;
        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly AccommodationPhotoRepository imageRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        private readonly AccommodationGuestRatingRepository accommodationGuestRatingRepository;
        private readonly AccommodationOwnerRatingRepository accommodationOwnerRatingRepository;
        private readonly AccommodationReservationMoveRequestRepository accommodationReservationMoveRequestRepository;

        public OwnerMain(User user)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = user;

            userRepository = new UserRepository();
            locationRepository = new LocationRepository();
            imageRepository = new AccommodationPhotoRepository();
            accommodationRepository = new AccommodationRepository(userRepository, locationRepository, imageRepository);
            accommodationReservationRepository = new AccommodationReservationRepository(accommodationRepository, userRepository);
            accommodationGuestRatingRepository = new AccommodationGuestRatingRepository(accommodationReservationRepository.GetAll());
            accommodationOwnerRatingRepository = new AccommodationOwnerRatingRepository(accommodationReservationRepository.GetAll());
            accommodationReservationMoveRequestRepository = new AccommodationReservationMoveRequestRepository(accommodationReservationRepository);

            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetByUser(user));
            AccommodationOwnerRatings = new ObservableCollection<AccommodationOwnerRating>(accommodationOwnerRatingRepository.GetRatingsVisibleToOwner(user, accommodationGuestRatingRepository.GetAll()));
            AccommodationReservationMoveRequests = new ObservableCollection<AccommodationReservationMoveRequest>(accommodationReservationMoveRequestRepository.GetWaitingByOwner(LoggedInUser));

            ShowNotifications();
            SetSuperOwner();
        }

        private void ShowCreateAccommodation_Click(object sender, RoutedEventArgs e)
        {
            CreateAccommodation createAccommodation = new CreateAccommodation(LoggedInUser, accommodationRepository, locationRepository, imageRepository);
            createAccommodation.ShowDialog();
        }

        private void ShowAccommodationGuestRatingWindow_Click(object sender, RoutedEventArgs e)
        {
            AccommodationGuestRatingWindow accommodationGuestRatingWindow = new AccommodationGuestRatingWindow(LoggedInUser, accommodationReservationRepository);
            accommodationGuestRatingWindow.ShowDialog();
        }

        private void ShowNotifications()
        {
            var unratedGuests = accommodationReservationRepository.GetUnrated(accommodationGuestRatingRepository.GetAll());

            foreach (var unratedGuest in unratedGuests)
            {
                int daysLeft = accommodationReservationRepository.CalculateDaysLeftForRating(unratedGuest);
                MessageBox.Show($"Rate the guest.\n\nUsername: {unratedGuest.Guest.Username}\n" +
                    $"Accommodation: {unratedGuest.Accommodation.Name}\nDays left for rating: {daysLeft}", "Unrated guest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetSuperOwner()
        {
            AverageRatingLabel.Content = accommodationOwnerRatingRepository.GetAverageRatingForOwner(LoggedInUser).ToString();
            if (accommodationOwnerRatingRepository.IsSuperOwner(LoggedInUser)) {
                SuperOwnerLabel.Content = "Yes";
            }
            else
            {
                SuperOwnerLabel.Content = "No";
            }
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void OpenReviewAccommodationReservationMoveRequestWindow_click(object sender, RoutedEventArgs e)
        {
            if (SelectedMoveRequest == null)
            {
                MessageBox.Show("Select a move request.");
            }
            else
            {
                AccommodationReservationMoveRequestManagingWindow accommodationReservationMoveRequestManagingWindow = new AccommodationReservationMoveRequestManagingWindow(LoggedInUser, accommodationReservationRepository, accommodationReservationMoveRequestRepository, SelectedMoveRequest);
                accommodationReservationMoveRequestManagingWindow.Show();
            }
        }
    }
}
