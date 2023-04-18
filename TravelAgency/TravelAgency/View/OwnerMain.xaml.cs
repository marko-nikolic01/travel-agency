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
using TravelAgency.Services;

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

        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly AccommodationPhotoRepository imageRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        private readonly AccommodationGuestRatingRepository accommodationGuestRatingRepository;
        private readonly AccommodationOwnerRatingRepository accommodationOwnerRatingRepository;

        public UserService UserService { get; set; }
        public AccommodationReservationMoveService moveReqestService { get; set; } 
        public SuperOwnerService SuperOwnerService { get; set; }

        public OwnerMain(User user)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = user;

            UserService = new UserService();
            moveReqestService = new AccommodationReservationMoveService();
            SuperOwnerService = new SuperOwnerService();

            locationRepository = new LocationRepository();
            imageRepository = new AccommodationPhotoRepository();
            accommodationRepository = new AccommodationRepository(UserService.GetAllUsers(), locationRepository.GetAll(), imageRepository.GetAll());
            accommodationReservationRepository = new AccommodationReservationRepository(accommodationRepository.GetAll(), UserService.GetAllUsers());
            accommodationGuestRatingRepository = new AccommodationGuestRatingRepository(accommodationReservationRepository.GetAll());
            accommodationOwnerRatingRepository = new AccommodationOwnerRatingRepository(accommodationReservationRepository.GetAll());

            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetByOwner(user));
            AccommodationOwnerRatings = new ObservableCollection<AccommodationOwnerRating>(accommodationOwnerRatingRepository.GetRatingsVisibleToOwner(user, accommodationGuestRatingRepository.GetAll()));

            AccommodationReservationMoveRequests = new ObservableCollection<AccommodationReservationMoveRequest>(moveReqestService.GetWaitingMoveRequestsByOwner(LoggedInUser));

            ShowNotifications();
            SetSuperOwner();
        }

        private void ShowCreateAccommodation_Click(object sender, RoutedEventArgs e)
        {
            CreateAccommodation createAccommodation = new CreateAccommodation(LoggedInUser);
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
                                $"Accommodation: {unratedGuest.Accommodation.Name}\n" +
                                $"Days left for rating: {daysLeft}",
                                "Unrated guest", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetSuperOwner()
        {
            AverageRatingLabel.Content = SuperOwnerService.GetAverageRatingForOwner(LoggedInUser).ToString();
            if (SuperOwnerService.IsSuperOwner(LoggedInUser)) {
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
                AccommodationReservationMoveRequestManagingWindow moveRequestManagingWindow = new AccommodationReservationMoveRequestManagingWindow(LoggedInUser, SelectedMoveRequest);
                moveRequestManagingWindow.Show();
            }
        }
    }
}
