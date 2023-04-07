using System.Collections.Generic;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourGuests.xaml
    /// </summary>
    public partial class TourGuests : Window
    {
        private int numberOfGuests;
        private UserRepository userRepository;
        private TourReservationRepository tourReservationRepository;
        private TourOccurrence TourOccurrence;
        private TourReservationWindow reservationsWindow;
        private User activeGuest;
        public TourOccurrenceRepository TourOccurrenceRepository;
        VoucherViewModel voucherViewModel;
        public TourGuests(int guests, TourOccurrence tourOccurrence, TourReservationWindow reservationsWindow, User user, TourOccurrenceRepository tourOccurrenceRepository)
        {
            numberOfGuests = guests;
            InitializeComponent();
            userRepository = new UserRepository();
            tourReservationRepository = new TourReservationRepository();
            TourOccurrence = tourOccurrence;
            TourOccurrenceRepository = tourOccurrenceRepository;
            this.reservationsWindow = reservationsWindow;
            activeGuest = user;
            GuestList.Items.Add(activeGuest.Username);
            voucherViewModel = new VoucherViewModel(user.Id);
            vouchersList.DataContext = voucherViewModel;
            CheckGuestNumber();
        }

        private void AddGuest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GuestUsernameText.Text))
            {
                return;
            }
            GuestList.Items.Add(GuestUsernameText.Text);
            GuestUsernameText.Clear();
            GuestUsernameText.Focus();
            isListBoxFull();
        }
        private void isListBoxFull()
        {
            if(GuestList.Items.Count == numberOfGuests)
            {
                AddButton.IsEnabled = false;
                SubmitButton.IsEnabled = true;
            }
            else
            {
                AddButton.IsEnabled = true;
                SubmitButton.IsEnabled = false;
            }
        }
        private void RemoveGuest_Click(object sender, RoutedEventArgs e)
        {
            if(GuestList.SelectedItem != null && GuestList.SelectedIndex != 0)
            {
                GuestList.Items.RemoveAt(GuestList.SelectedIndex);
                AddButton.IsEnabled = true;
                SubmitButton.IsEnabled = false;
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            List<User> users = new List<User>();
            TourReservation tourReservation;
            User user;
            for(int i = 0; i<numberOfGuests; i++)
            {
                user = GetUserByName(i);
                if (user == null)
                {
                    user = new User(GuestList.Items.GetItemAt(i).ToString(), "ftn", Roles.Guest2);
                    userRepository.SaveUser(user);
                }
                users.Add(user);
                tourReservation = new TourReservation(TourOccurrence.Id, user.Id);
                tourReservationRepository.SaveTourReservation(tourReservation);
            }
            TourOccurrence.Guests.AddRange(users);

            TourOccurrence.FreeSpots -= users.Count;
            TourOccurrenceRepository.UpdateTourOccurrence(TourOccurrence);
            voucherViewModel.UpdateVoucher();
            Guest2Main.TourOccurrenceRepository.NotifyObservers();
            Close();
            reservationsWindow.CloseWindow();
        }
        private User GetUserByName(int i)
        {
            return userRepository.GetUsers().Find(x => (x.Username == GuestList.Items.GetItemAt(i).ToString() && x.Role == Roles.Guest2));
        }
        private void CheckGuestNumber()
        {
            if(numberOfGuests == 1)
            {
                AddButton.IsEnabled = false;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
