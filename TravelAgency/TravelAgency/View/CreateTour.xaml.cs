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
        public Location Location { get; set; }
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
            Location = new Location();
            TourRepository = tourRepository;
            LocationRepository = locationRepository;
            PhotoRepository = photoRepository;
            TourOccurrenceRepository = tourOccurrenceRepository;
            KeyPointRepository = keyPointeRepository;

            DateCalendar.DisplayDateStart = DateTime.Today;
            //cultureinfo from stackoverflow
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void AddKeyPointClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyPointsText.Text))
            {
                return;
            }
            ListKeyPoints.Items.Add(KeyPointsText.Text);
            KeyPointsText.Clear();
            KeyPointsText.Focus();
        }

        private void AddDateTimeClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DateCalendar.Text) || string.IsNullOrEmpty(TimeText.Text))
            {
                return;
            }
            ListDateTimes.Items.Add(DateCalendar.Text + " " + TimeText.Text);
            DateCalendar.Focus();
        }

        private void AddImagesClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageText.Text))
            {
                return;
            }
            ListPhotos.Items.Add(ImageText.Text);
            ImageText.Clear();
            ImageText.Focus();
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            if(ListKeyPoints.Items.Count < 2) {
                MessageBox.Show("You have to enter at least two key points!");
                return;
            }
            else if (ListPhotos.Items.Count == 0)
            {
                MessageBox.Show("You have to enter at least one photo link!");
                return;
            }
            else if (ListDateTimes.Items.Count == 0)
            {
                MessageBox.Show("You have to enter at least one date and time!");
                return;
            }

            int result;
            if(int.TryParse(MaxGuests.Text, out result))
            {
                NewTour.MaxGuestNumber = result;
            }
            if (int.TryParse(Duration.Text, out result))
            {
                NewTour.Duration = result;
            }

            if (Location.IsValid == false)
            {
                MessageBox.Show("Location entry is wrong");
                return;
            }
            string[] cityCountry = Location.FullName.Split(',');
            Location.City = cityCountry[0];
            Location.Country = cityCountry[1];

            Location savedLocation = LocationRepository.SaveLocation(Location);
            NewTour.Location = savedLocation;
            NewTour.LocationId = savedLocation.Id;

            if(NewTour.IsValid == false)
            {
                MessageBox.Show("Tour entry is wrong");
                return;
            }

            TourRepository.SaveTours(NewTour);

            foreach (string link in ListPhotos.Items)
            {
                Photo photo = new Photo();
                photo.TourId = NewTour.Id;
                photo.Link = link;
                NewTour.Photos.Add(photo);
                PhotoRepository.SavePhotos(photo);
            }

            foreach (string dateTimeItem in ListDateTimes.Items)
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeItem, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));
                TourOccurrence tourOccurrence = new TourOccurrence();
                tourOccurrence.TourId = NewTour.Id;
                tourOccurrence.Tour = NewTour;
                tourOccurrence.DateTime = dateTime;
                TourOccurrenceRepository.SaveTourOccurrences(tourOccurrence, ActiveGuide);
                foreach (string keyPointItem in ListKeyPoints.Items)
                {
                    KeyPoint keyPoint = new KeyPoint();
                    keyPoint.TourOccurrenceId = tourOccurrence.Id;
                    keyPoint.Name = keyPointItem;
                    tourOccurrence.KeyPoints.Add(keyPoint);
                    KeyPointRepository.SaveKeyPoints(keyPoint);
                }
            }
            Close();
        }

    }
}
