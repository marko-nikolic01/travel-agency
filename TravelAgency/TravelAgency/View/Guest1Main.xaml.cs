﻿using System;
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

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
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

            Guest = guest;
            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAll());

            Countries = locationRepository.GetAllCountries();
            Countries.Insert(0, "Not specified");
            SelectedCountry = Countries[0];
        }

        private void LoadDateTime(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.statusBarDateTime.Content = DateTime.Now.ToString("dd/mm/yyyy     hh:mm:ss tt");
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
            if (SelectedCountry != "")
            {
                Cities = locationRepository.GetCitiesByCountry(SelectedCountry);
                Cities.Insert(0, "Not specified");
                cityComboBox.ItemsSource = Cities;
                cityComboBox.SelectedItem = 0;
                SelectedCity = Cities[0];
                cityComboBox.Text = Cities[0];
            }

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
            var filtered = Accommodations.Where(accommodation => accommodation.Name != "");

            if (nameTextBox.Text != "")
            {
                filtered = filtered.Where(accommodation => accommodation.Name.ToLower().Contains(nameTextBox.Text.ToLower()));
            }

            if (countryComboBox.SelectedIndex != 0)
            {
                filtered = filtered.Where(accommodation => accommodation.Location.Country.ToLower().Contains(countryComboBox.Text.ToLower()));
            }

            if (cityComboBox.SelectedIndex != 0)
            {
                filtered = filtered.Where(accommodation => accommodation.Location.City.ToLower().Contains(cityComboBox.Text.ToLower()));
            }

            switch (typeComboBox.SelectedIndex)
            {
                case 1:
                    filtered = filtered.Where(accommodation => accommodation.Type == AccommodationType.APARTMENT);
                    break;
                case 2:
                    filtered = filtered.Where(accommodation => accommodation.Type == AccommodationType.HOUSE);
                    break;
                case 3:
                    filtered = filtered.Where(accommodation => accommodation.Type == AccommodationType.HUT);
                    break;
            }

            if (guestNumberUpDown.Value != 0)
            {
                filtered = filtered.Where(accommodation => guestNumberUpDown.Value <= accommodation.MaxGuests);
            }

            if (dayNumberUpDown.Value != 0)
            {
                filtered = filtered.Where(accommodation => dayNumberUpDown.Value >= accommodation.MinDays);
            }

            accommodationsDataGrid.ItemsSource = filtered;
        }

        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            accommodationsDataGrid.ItemsSource = Accommodations;
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
    }
}
