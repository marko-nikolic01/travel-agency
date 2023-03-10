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
        public CreateTour(TourRepository tourRepository)
        {
            InitializeComponent();
            DataContext = this;
            NewTour = new Tour();
            Location = new Location();
            TourRepository = tourRepository;
            LocationRepository = new LocationRepository();
            DateCalendar.DisplayDateStart = DateTime.Today;
            //DatePickerText = new DateTime();
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
            ListImages.Items.Add(ImageText.Text);
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
            foreach(String keyPoint in ListKeyPoints.Items)
            {
                //NewTour.KeyPoints.Add(keyPoint);
            }
            foreach (String image in ListImages.Items)
            {
                NewTour.Images.Add(image);
            }
            foreach(String dateTime in ListDateTimes.Items)
            {
                DateTime dt = DateTime.ParseExact(dateTime, "dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);
                //NewTour.DateTimes.Add(dt);
            }
            TourRepository.SaveTours(NewTour);
            Close();
        }

    }
}
