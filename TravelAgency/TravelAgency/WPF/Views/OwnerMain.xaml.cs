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
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;

namespace TravelAgency.WPF.Views
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

        public UserService UserService { get; set; }
        public AccommodationService AccommodationService { get; set; }
        public AccommodationReservationMoveService moveReqestService { get; set; } 
        public SuperOwnerService SuperOwnerService { get; set; }
        public AccommodationOwnerRatingService AccommodationOwnerRatingService { get; set; }
        public AccommodationGuestRatingService AccommodationGuestRatingService { get; set; }

        public OwnerMain(User user)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = user;

            UserService = new UserService();
            AccommodationService = new AccommodationService();
            moveReqestService = new AccommodationReservationMoveService();
            SuperOwnerService = new SuperOwnerService();
            AccommodationOwnerRatingService = new AccommodationOwnerRatingService();
            AccommodationGuestRatingService = new AccommodationGuestRatingService();

            Accommodations = new ObservableCollection<Accommodation>(AccommodationService.GetByOwner(user));
            AccommodationOwnerRatings = new ObservableCollection<AccommodationOwnerRating>(AccommodationOwnerRatingService.GetRatingsVisibleToOwner(user));

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
            AccommodationGuestRatingWindow accommodationGuestRatingWindow = new AccommodationGuestRatingWindow(LoggedInUser);
            accommodationGuestRatingWindow.ShowDialog();
        }

        private void ShowNotifications()
        {
            var unratedGuests = AccommodationGuestRatingService.GetUnratedReservationsByOwner(LoggedInUser);

            foreach (var unratedGuest in unratedGuests)
            {
                int daysLeft = AccommodationGuestRatingService.CalculateDaysLeftForRating(unratedGuest);
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
                AccommodationReservationMoveRequestManagingWindow moveRequestManagingWindow = new AccommodationReservationMoveRequestManagingWindow(SelectedMoveRequest);
                moveRequestManagingWindow.Show();
            }
        }

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            OwnerWindow ownerWindow = new OwnerWindow();
            ownerWindow.Show();
        }
    }
}
