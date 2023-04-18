using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Services;
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourGuests.xaml
    /// </summary>
    public partial class TourGuests : Window, INotifyPropertyChanged
    {
        private string numberOfGuestsInput;
        private string spotsLeft;
        public string NumberOfGuestsInput
        {
            get => numberOfGuestsInput;
            set
            {
                if (value != numberOfGuestsInput)
                {
                    numberOfGuestsInput = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SpotsLeft
        {
            get => spotsLeft;
            set
            {
                if (value != spotsLeft)
                {
                    spotsLeft = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //private int numberOfGuests;
        private UserRepository userRepository;
        private TourReservationRepository tourReservationRepository;
        private TourOccurrence TourOccurrence;
        private User activeGuest;
        public TourOccurrenceService tourOccurrenceService;
        VoucherViewModel voucherViewModel;
        public TourGuests(TourOccurrence tourOccurrence, User user)
        {
            InitializeComponent();
            DataContext = this;
            userRepository = new UserRepository();
            tourReservationRepository = new TourReservationRepository();
            TourOccurrence = tourOccurrence;
            tourOccurrenceService = new TourOccurrenceService();
            activeGuest = user;
            GuestList.Items.Add(activeGuest.Username);
            voucherViewModel = new VoucherViewModel(user.Id);
            vouchersList.DataContext = voucherViewModel;
            AddButton.IsEnabled = false;
            NumberOfGuestsInput = "1";
            descriptionLabel.Content = tourOccurrence.Tour.Name + " in " + tourOccurrence.Tour.Location.Country +
                ", " + tourOccurrence.Tour.Location.City + ". " + tourOccurrence.Tour.Description;
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
            if (GuestList.Items.Count == int.Parse(NumberOfGuestsInput))
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
            if (GuestList.SelectedItem != null && GuestList.SelectedIndex != 0)
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

            for (int i = 0; i < GuestList.Items.Count; i++)
            {
                user = GetUserByName(i);
                if (user == null)
                {
                    user = new User(GuestList.Items.GetItemAt(i).ToString(), "ftn", Roles.Guest2, new DateOnly(2004, 2, 15));
                    userRepository.SaveUser(user);
                }
                users.Add(user);
                tourReservation = new TourReservation(TourOccurrence.Id, user.Id);
                tourReservationRepository.SaveTourReservation(tourReservation);
            }
            TourOccurrence.Guests.AddRange(users);
            TourOccurrence.FreeSpots -= users.Count;
            tourOccurrenceService.UpdateTour(TourOccurrence);
            voucherViewModel.UpdateVoucher(TourOccurrence.Id);
            Close();
        }
        private User GetUserByName(int i)
        {
            return userRepository.GetUsers().Find(x => (x.Username == GuestList.Items.GetItemAt(i).ToString() && x.Role == Roles.Guest2));
        }

        private void CheckSpotsNumber(int input)
        {
            if (input <= 0)
            {
                SpotsLeft = "Wrong input";
                SubmitButton.IsEnabled = false;
            }
            else
            {
                int spotsLeft = TourOccurrence.Tour.MaxGuestNumber - (TourOccurrence.Guests.Count + input);
                if (spotsLeft < 0)
                {
                    SpotsLeft = "Not enough spots on tour";
                    SubmitButton.IsEnabled = false;
                }
                else
                {
                    UpdateList();
                    SpotsLeft = "Spots left: "+spotsLeft.ToString();
                }
            }
        }
        // provera ako je korisnik smanjio broj gostiju, da se izbaci iz listbox-a visak
        private void UpdateList()
        {
            int currentGuestNumber = int.Parse(NumberOfGuestsInput);
            if (currentGuestNumber < GuestList.Items.Count)
            {
                for (int i = GuestList.Items.Count - 1; i >= currentGuestNumber; i--)
                    GuestList.Items.RemoveAt(i);
                AddButton.IsEnabled = false;
                SubmitButton.IsEnabled = true;
            }
            else if (currentGuestNumber > GuestList.Items.Count)
            {
                AddButton.IsEnabled = true;
                SubmitButton.IsEnabled = false;
            }
            // currentGuestNumber == GuestList.Items.Count
            else
            {
                AddButton.IsEnabled = false;
                SubmitButton.IsEnabled = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            int input;
            if (int.TryParse(NumberOfGuestsInput, out input))
            {
                CheckSpotsNumber(input);
            }
            else
            {
                SpotsLeft = "Wrong input";
                SubmitButton.IsEnabled = false;
                AddButton.IsEnabled = false;
            }
        }
        private void Deselect_Click(object sender, RoutedEventArgs e)
        {
            vouchersList.UnselectAll();
        }
    }
}
