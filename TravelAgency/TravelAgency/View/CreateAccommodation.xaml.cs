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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodation : Window
    {
        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly AccommodationPhotoRepository accommodationPhotoRepository;
        
        public Accommodation NewAccommodation { get; set; }
        public Location NewLocation { get; set; }

        public CreateAccommodation(User loggedInUser, AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationPhotoRepository accommodationPhotoRepository)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = loggedInUser;

            this.locationRepository = locationRepository;
            this.accommodationRepository = accommodationRepository;
            this.accommodationPhotoRepository = accommodationPhotoRepository;

            NewAccommodation = new() { Id = this.accommodationRepository.NextId(), OwnerId = LoggedInUser.Id, Owner = LoggedInUser };
            NewLocation = new();

            InitializeComboboxes();
        }

        private void InitializeComboboxes()
        {
            var countries = locationRepository.GetAllCountries();
            foreach (var country in countries)
            {
                CountryComboBox.Items.Add(country);
            }
            CountryComboBox.SelectedIndex = 0;
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG images (*.png)|*.png|JPEG images (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            ofd.Multiselect = false;
            ofd.InitialDirectory = $"c:\\Users\\{Environment.UserName}\\Pictures";
            var result = ofd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var imagePath = ofd.FileName;

                AccommodationPhoto NewImage = new() { ObjectId = NewAccommodation.Id, Path = imagePath};

                NewAccommodation.Photos.Add(NewImage);
            }
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

            var country = CountryComboBox.SelectedItem as string;
            var city = CityComboBox.SelectedItem as string;

            NewAccommodation.Location = locationRepository.GetLocationForCountryAndCity(country, city);
            NewAccommodation.LocationId = NewAccommodation.Location.Id;

            Accommodation savedAccommodation = accommodationRepository.Save(NewAccommodation);

            accommodationPhotoRepository.SaveAll(savedAccommodation.Photos);

            OwnerMain.Accommodations.Add(savedAccommodation);

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
                var cities = locationRepository.GetCitiesByCountry(country);
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
            AccommodationPhoto NewImage = new() { ObjectId = NewAccommodation.Id, Path = photoURL };
            NewAccommodation.Photos.Add(NewImage);

            AccommodationPhotoURLTextBox.Text = "";
        }
    }
}
