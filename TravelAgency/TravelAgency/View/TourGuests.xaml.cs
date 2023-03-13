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
    /// Interaction logic for TourGuests.xaml
    /// </summary>
    public partial class TourGuests : Window
    {
        private int numberOfGuests;
        private UserRepository userRepository;
        private TourReservationRepository tourReservationRepository;
        private TourOccurrence TourOccurrence;
        private TourReservationWindow reservationsWindow;
        public TourGuests(int guests, TourOccurrence tourOccurrence, TourReservationWindow reservationsWindow)
        {
            numberOfGuests = guests;
            InitializeComponent();
            userRepository = new UserRepository();
            tourReservationRepository = new TourReservationRepository();
            TourOccurrence = tourOccurrence;
            SubmitButton.IsEnabled = false;
            this.reservationsWindow = reservationsWindow;
        }

        private void AddGuestClick(object sender, RoutedEventArgs e)
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
        private void RemoveGuestClick(object sender, RoutedEventArgs e)
        {
            if(GuestList.SelectedItem !=null)
            {
                GuestList.Items.RemoveAt(GuestList.SelectedIndex);
            }
            isListBoxFull();
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            List<User> users = new List<User>();
            TourReservation tourReservation;
            User user;
            for(int i = 0; i<numberOfGuests; i++)
            {
                user = new User(GuestList.Items.GetItemAt(i).ToString(), "ftn", Roles.Guest2);
                users.Add(user);
                userRepository.SaveUser(user);
                tourReservation = new TourReservation(TourOccurrence.Id, user.Id);
                tourReservationRepository.SaveTourReservation(tourReservation);
            }
            TourOccurrence.Guests.AddRange(users);
            Guest2Main.TourOccurrenceRepository.NotifyObservers();
            Close();
            reservationsWindow.CloseWindow();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
