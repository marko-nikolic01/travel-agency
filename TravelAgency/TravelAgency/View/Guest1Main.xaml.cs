 using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using TravelAgency.Model;
using TravelAgency.Repository;
using Xceed.Wpf.Toolkit;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for Guest1Main.xaml
    /// </summary>
    public partial class Guest1Main : Window
    {
        public User Guest { get; set; }

        public UserRepository userRepository;
        public AccommodationRepository accommodationRepository;
        public LocationRepository locationRepository;
        public AccommodationPhotoRepository imageRepository;
        public AccommodationReservationRepository accommodationReservationRepository;
        public AccommodationReservationMoveRequestRepository accommodationReservationMoveRequestRepository;
        public AccommodationOwnerRatingRepository accommodationOwnerRatingRepository;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public static ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public static ObservableCollection<AccommodationReservationMoveRequest> ReservationMoveRequests { get; set; }
        public static ObservableCollection<AccommodationReservation> Stays { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public AccommodationReservationMoveRequest SelectedReservationMoveRequest { get; set; }
        public AccommodationReservation SelectedStay { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedCity { get; set; }

        public Guest1Main(User guest)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.9);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.9);

            userRepository = new UserRepository();
            locationRepository = new LocationRepository();
            imageRepository = new AccommodationPhotoRepository();
            accommodationRepository = new AccommodationRepository(userRepository, locationRepository, imageRepository);
            accommodationReservationRepository = new AccommodationReservationRepository(accommodationRepository, userRepository);
            accommodationOwnerRatingRepository = new AccommodationOwnerRatingRepository(accommodationReservationRepository.GetAll());
            accommodationReservationMoveRequestRepository = new AccommodationReservationMoveRequestRepository(accommodationReservationRepository);

            accommodationOwnerRatingRepository.SetSuperOwners(userRepository);

            Guest = guest;
            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAllSortedBySuperOwnersFirst());
            Reservations = new ObservableCollection<AccommodationReservation>(accommodationReservationRepository.GetAllNotCanceledByGuest(Guest));
            ReservationMoveRequests = new ObservableCollection<AccommodationReservationMoveRequest>(accommodationReservationMoveRequestRepository.GetAllByGuest(Guest));
            Stays = new ObservableCollection<AccommodationReservation>(accommodationReservationRepository.GetUnrated2(accommodationOwnerRatingRepository.GetByOwner(Guest)));

            Countries = locationRepository.GetAllCountries();
            Countries.Insert(0, "Not specified");
            SelectedCountry = Countries[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadDateTime(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.statusBarDateTime.Content = DateTime.Now.ToString("dd/MM/yyyy     hh:mm:ss tt");
            }, this.Dispatcher);
            timer.Start();
        }

        private void NumberUpDownValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var upDown = (sender as IntegerUpDown);
            if (upDown.Value == 0)
            {
                upDown.Text = "";
            }

        }

        private void CountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCountry != "Not specified")
            {
                Cities = locationRepository.GetCitiesByCountry(SelectedCountry);
            }
            else
            {
                Cities = locationRepository.GetAllCities();
            }

            Cities.Insert(0, "Not specified");
            cityComboBox.ItemsSource = Cities;
            cityComboBox.SelectedItem = 0;
            SelectedCity = Cities[0];
            cityComboBox.Text = Cities[0];

        }

        private void ComboBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.Text != "")
            {
                comboBox.Text = comboBox.SelectedItem.ToString();
            }
            else
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void SignOut(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string nameFilter = nameTextBox.Text;
            string countryFilter = countryComboBox.SelectedItem.ToString();
            string cityFilter = cityComboBox.SelectedItem.ToString();
            string typeFilter = typeComboBox.SelectedValue.ToString();
            int guestNumberFilter = guestNumberUpDown.Value.Value;
            int dayNumberFilter = dayNumberUpDown.Value.Value;
            AccommodationSearchFilter filter = new AccommodationSearchFilter(nameFilter, countryFilter, cityFilter, typeFilter, guestNumberFilter, dayNumberFilter);

            accommodationsDataGrid.ItemsSource = accommodationRepository.Search(filter);
        }

        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            accommodationsDataGrid.ItemsSource = accommodationRepository.GetAllSortedBySuperOwnersFirst();
            nameTextBox.Text = "";
            countryComboBox.SelectedItem = 0;
            SelectedCountry = Cities[0];
            countryComboBox.Text = Cities[0];
            cityComboBox.SelectedItem = 0;
            SelectedCity = Cities[0];
            cityComboBox.Text = Cities[0];
            typeComboBox.SelectedIndex = 0;
            guestNumberUpDown.Value = 0;
            dayNumberUpDown.Value = 0;

        }

        private void MakeReservation(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationReservationWindow accommodationReservationWindow = new AccommodationReservationWindow(Guest, SelectedAccommodation, accommodationReservationRepository);
                accommodationReservationWindow.Show();
            }
            else
            {
                string message = "You didn't select an accommodation!";
                System.Windows.MessageBox.Show(message);
            }
        }

        private void CancelReservation(object sender, RoutedEventArgs e)
        {
            if (accommodationReservationRepository.CancelReservation(SelectedReservation))
            {
                Reservations.Remove(SelectedReservation);
            }
            else
            {
                string message = "Cancelation deadline for this reservation is overdue!";
                System.Windows.MessageBox.Show(message);
            }
        }

        private void MoveReservation(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                AccommodationReservationMoveRequestWindow moveRequestWindow = new AccommodationReservationMoveRequestWindow(accommodationReservationRepository, SelectedReservation, accommodationReservationMoveRequestRepository);
                moveRequestWindow.ShowDialog();
            }
            else 
            {
                string message = "You didn't select a reservation";
                System.Windows.MessageBox.Show(message);
            }
        }

        private void RateAccommodationOwner(object sender, RoutedEventArgs e)
        {
            if (SelectedStay != null)
            {
                AccommodationOwnerRatingWindow accommodationOwnerRatingWindow = new AccommodationOwnerRatingWindow(accommodationOwnerRatingRepository, SelectedStay);
                accommodationOwnerRatingWindow.Show();
            }
            else
            {
                string message = "You didn't select a stay to rate";
                System.Windows.MessageBox.Show(message);
            }

        }
    }
}
