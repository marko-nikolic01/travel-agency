using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using TravelAgency.Services;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodation : Window
    {
        public User LoggedInUser { get; set; }
        
        public Accommodation NewAccommodation { get; set; }
        public Location NewLocation { get; set; }

        public AccommodationService AccommodationService { get; set; }
        public LocationService LocationService { get; set; }

        public CreateAccommodation(User loggedInUser)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = loggedInUser;

            AccommodationService = new AccommodationService();
            LocationService = new LocationService();

            NewAccommodation = new Accommodation() { OwnerId = LoggedInUser.Id, Owner = LoggedInUser };
            NewLocation = new Location();

            InitializeComboboxes();
        }

        private void InitializeComboboxes()
        {
            var countries = LocationService.GetCountries();
            foreach (var country in countries)
            {
                CountryComboBox.Items.Add(country);
            }
            CountryComboBox.SelectedIndex = 0;
        }

        private void RegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            if (!NewAccommodation.IsValid)
            {
                System.Windows.MessageBox.Show("Fields not valid!");
                return;
            }

            if (CountryComboBox.SelectedIndex == 0)
            {
                System.Windows.MessageBox.Show("Select a country!");
                return;
            }

            if (CityComboBox.SelectedIndex == 0)
            {
                System.Windows.MessageBox.Show("Select a city!");
                return;
            }

            if (NewAccommodation.Photos.Count == 0)
            {
                System.Windows.MessageBox.Show("Add at least one photo!");
                return;
            }

            if (ApartmentRadioButton.IsChecked == true)
            {
                NewAccommodation.Type = AccommodationType.APARTMENT;
            }
            else if (HouseRadioButton.IsChecked == true)
            {
                NewAccommodation.Type = AccommodationType.HOUSE;
            }
            else
            {
                NewAccommodation.Type = AccommodationType.HUT;
            }

            string? country = CountryComboBox.SelectedItem as string;
            string? city = CityComboBox.SelectedItem as string;

            NewAccommodation.Location = LocationService.GetLocationForCountryAndCity(country, city);
            NewAccommodation.LocationId = NewAccommodation.Location.Id;

            AccommodationService.CreateNew(NewAccommodation);

            OwnerMain.Accommodations.Add(NewAccommodation);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CountryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryComboBox.SelectedIndex != 0)
            {
                string country = (string)CountryComboBox.SelectedValue;
                var cities = LocationService.GetCitiesByCountry(country);
                CityComboBox.Items.Clear();
                CityComboBox.Items.Add("<Select a city>");
                foreach (var city in cities)
                {
                    CityComboBox.Items.Add(city);
                }
                CityComboBox.SelectedIndex = 0;
                CityComboBox.IsEnabled = true;
            }
            else
            {
                CityComboBox.IsEnabled = false;
            }
        }

        private void AddPhotoFromInternet_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AccommodationPhotoURLTextBox.Text))
            {
                System.Windows.MessageBox.Show("Enter an URL for the photo.");
                return;
            }

            var photoURL = AccommodationPhotoURLTextBox.Text;

            AccommodationPhotosListView.Items.Add(photoURL);
            
            AccommodationPhoto NewImage = new() { Path = photoURL };
            NewAccommodation.Photos.Add(NewImage);

            AccommodationPhotoURLTextBox.Text = "";
        }
    }
}
