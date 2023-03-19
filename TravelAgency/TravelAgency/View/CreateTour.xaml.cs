using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using TravelAgency.Observer;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateTour.xaml
    /// </summary>
    public partial class CreateTour : Window
    {
        public TourRepository TourRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public Tour NewTour { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public KeyPointRepository KeyPointRepository { get; set; }
        public User ActiveGuide { get; set; }
        public CreateTour(TourRepository tourRepository, LocationRepository locationRepository, PhotoRepository photoRepository, TourOccurrenceRepository tourOccurrenceRepository, KeyPointRepository keyPointeRepository, User activeGuide)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = activeGuide;
            NewTour = new Tour();
            TourRepository = tourRepository;
            LocationRepository = locationRepository;
            PhotoRepository = photoRepository;
            TourOccurrenceRepository = tourOccurrenceRepository;
            KeyPointRepository = keyPointeRepository;

            InitializeComboboxes();

            DateCalendar.DisplayDateStart = DateTime.Today;
            //cultureinfo from stackoverflow
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void InitializeComboboxes()
        {
            var countries = LocationRepository.GetAllCountries();
            foreach (var country in countries)
            {
                CountryComboBox.Items.Add(country);
            }
            CountryComboBox.SelectedIndex = 0;
        }

        private void AddKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyPointsText.Text))
            {
                return;
            }
            ListKeyPoints.Items.Add(KeyPointsText.Text);
            KeyPointsText.Clear();
            KeyPointsText.Focus();
        }

        private void AddDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DateCalendar.Text) || string.IsNullOrEmpty(Time.Text))
            {
                return;
            }
            ListDateTimes.Items.Add(DateCalendar.Text + " " + Time.Text);
            DateCalendar.Focus();
        }

        private void AddImages_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageText.Text))
            {
                return;
            }
            ListPhotos.Items.Add(ImageText.Text);
            ImageText.Clear();
            ImageText.Focus();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!AreListsComplete() || !AreInputsValid())
            {
                return;
            }

            ProcessInputs(NewTour);

            SaveTours();

            Close();
        }

        private void SaveTours()
        {
            TourRepository.Save(NewTour);

            SaveTourPhotos(NewTour);

            SaveTourOccurrences(NewTour);
        }

        private void ProcessInputs(Tour newTour)
        {
            ProcessIntInputs(newTour);
            ProcessLocationInput(newTour);

        }

        private void SaveTourOccurrences(Tour newTour)
        {
            foreach (string dateTimeItem in ListDateTimes.Items)
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeItem, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));
                TourOccurrence tourOccurrence = SaveTourOccurrence(dateTime, newTour);
                
                foreach (string keyPointItem in ListKeyPoints.Items)
                {
                    SaveKeyPoint(keyPointItem, tourOccurrence);
                }
            }
        }

        private void SaveKeyPoint(string keyPointItem, TourOccurrence tourOccurrence)
        {
            KeyPoint keyPoint = new KeyPoint();
            keyPoint.TourOccurrenceId = tourOccurrence.Id;
            keyPoint.Name = keyPointItem;
            tourOccurrence.KeyPoints.Add(keyPoint);
            KeyPointRepository.SaveKeyPoints(keyPoint);
        }

        private TourOccurrence SaveTourOccurrence(DateTime dateTime, Tour newTour)
        {
            TourOccurrence tourOccurrence = new TourOccurrence();
            tourOccurrence.TourId = newTour.Id;
            tourOccurrence.Tour = newTour;
            tourOccurrence.DateTime = dateTime;
            return TourOccurrenceRepository.SaveTourOccurrence(tourOccurrence, ActiveGuide);
        }

        private void SaveTourPhotos(Tour newTour)
        {
            foreach (string link in ListPhotos.Items)
            {
                Photo photo = new Photo();
                photo.TourId = newTour.Id;
                photo.Link = link;
                newTour.Photos.Add(photo);
                PhotoRepository.Save(photo);
            }
        }
        private void ProcessIntInputs(Tour newTour)
        {
            int result;
            if (int.TryParse(MaxGuests.Text, out result))
            {
                newTour.MaxGuestNumber = result;
            }
            if (int.TryParse(Duration.Text, out result))
            {
                newTour.Duration = result;
            }
        }

        private void ProcessLocationInput(Tour newTour)
        {

            var country = CountryComboBox.SelectedItem as string;
            var city = CityComboBox.SelectedItem as string;

            newTour.Location = LocationRepository.GetLocationForCountryAndCity(country, city);
            newTour.LocationId = newTour.Location.Id;
        }

        private bool AreListsComplete()
        {
            if (ListKeyPoints.Items.Count < 2)
            {
                MessageBox.Show("You have to enter at least two key points!");
                return false;
            }
            else if (ListPhotos.Items.Count == 0)
            {
                MessageBox.Show("You have to enter at least one photo link!");
                return false;
            }
            else if (ListDateTimes.Items.Count == 0)
            {
                MessageBox.Show("You have to enter at least one date and time!");
                return false;
            }
            return true;
        }

        private bool AreInputsValid()
        {
            if (CountryComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Select a country!");
                return false;
            }
            if (CityComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Select a city!");
                return false;
            }
            else if (NewTour.IsValid == false)
            {
                MessageBox.Show("Tour entry is wrong");
                return false;
            }
            return true;
        }

        private void CountryChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CountryComboBox.SelectedIndex != 0)
            {
                string country = (string)CountryComboBox.SelectedValue;
                var cities = LocationRepository.GetCitiesByCountry(country);
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

        private void DateCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(DateCalendar.SelectedDate == DateTime.Now.Date)
            {
                Time.StartTime = DateTime.Now.TimeOfDay;
            }
            else
            {
                TimeSpan timeSpan = (TimeSpan)DateTime.ParseExact("00:00", "HH:mm", null).TimeOfDay;
                Time.StartTime = timeSpan;
            }
        }
    }
}
