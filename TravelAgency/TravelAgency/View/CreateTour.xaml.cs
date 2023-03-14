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
            //sa stackoverflow
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
            TimeText.Clear();
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
            string[] cityCountry = Location.FullName.Split(',');
            Location.City = cityCountry[0];
            Location.Country = cityCountry[1];
            LocationRepository.SaveLocation(Location);
            NewTour.LocationId = Location.Id;
            NewTour.Location = Location;
            int result = 0;
            int.TryParse(MaxGuestsText.Text, out result);
            NewTour.MaxGuestNumber = result;
            int.TryParse(DurationText.Text, out result);
            NewTour.Duration = result;
            TourRepository.SaveTours(NewTour);
            foreach (String link in ListPhotos.Items)
            {
                Photo photo = new Photo();
                photo.TourId = NewTour.Id;
                photo.Link = link;
                NewTour.Photos.Add(photo);
                PhotoRepository.SavePhotos(photo);
            }
            foreach (String dateTimeItem in ListDateTimes.Items)
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeItem, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));
                TourOccurrence tourOccurrence = new TourOccurrence();
                tourOccurrence.TourId = NewTour.Id;
                tourOccurrence.Tour = NewTour;
                tourOccurrence.DateTime = dateTime;
                TourOccurrenceRepository.SaveTourOccurrences(tourOccurrence, ActiveGuide);
                foreach (String keyPointItem in ListKeyPoints.Items)
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
