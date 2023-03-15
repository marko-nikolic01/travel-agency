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
using TravelAgency.Model.DTO;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationWindow.xaml
    /// </summary>
    public partial class AccommodationReservationWindow : Window
    {
        User Guest { get; set; }
        Accommodation Accommodation { get; set; }

        public List<BitmapImage> ImageSources { get; set; }
        public int currentImageNumber;
        public AccommodationReservationWindow(User guest, Accommodation accommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Height = 700;
            this.Width = 1000;

            Guest = guest;
            Accommodation = accommodation;

            nameLabel.Content = "Name: " + Accommodation.Name;
            locationLabel.Content = "Location: " + Accommodation.Location.City + ", " + Accommodation.Location.Country;
            typeLabel.Content = "Type: " + Accommodation.Type;
            maxGuestsLabel.Content = "Max. guests: " + Accommodation.MaxGuests;
            minDaysLabel.Content = "Min. days: " + Accommodation.MinDays;
            daysToCancelLabel.Content = "Days to cancel: " + Accommodation.DaysToCancel;
            ownerLabel.Content = "Owner: " + Accommodation.Owner.Username;


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            string img1 = "https://optimise2.assets-servd.host/maniacal-finch/production/animals/amur-tiger-01-01.jpg?w=1200&auto=compress%2Cformat&fit=crop&dm=1658935145&s=1b96c26544a1ee414f976c17b18f2811";
            string img2 = "..\\Resources\\Images\\ProfilePicture.jpg";
            string img3 = "https://www.sfzoo.org/wp-content/uploads/2021/03/AfricanLionJasiri_resize2019.jpg";

            ImageSources = new List<BitmapImage>();

            Uri uri1 = new Uri(img1, UriKind.RelativeOrAbsolute);
            Uri uri2 = new Uri(img2, UriKind.RelativeOrAbsolute);
            Uri uri3 = new Uri(img3, UriKind.RelativeOrAbsolute);

            BitmapImage bmi1 = new BitmapImage(uri1);
            BitmapImage bmi2 = new BitmapImage(uri2);
            BitmapImage bmi3 = new BitmapImage(uri3);

            ImageSources.Add(bmi1);
            ImageSources.Add(bmi2);
            ImageSources.Add(bmi3);

            currentImageNumber = 0;
            accommodationImage.Source = ImageSources[0];
            

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        }

        private void ShowPreviousImage(object sender, RoutedEventArgs e)
        {
            currentImageNumber--;

            if (currentImageNumber == -1)
            {
                currentImageNumber = ImageSources.Count() - 1;
                accommodationImage.Source = ImageSources[currentImageNumber];
            }
            else
            {
                accommodationImage.Source = ImageSources[currentImageNumber];
            }
        }

        private void ShowNextImage(object sender, RoutedEventArgs e)
        {
            currentImageNumber++;

            if (currentImageNumber == ImageSources.Count())
            {
                currentImageNumber = 0;
                accommodationImage.Source = ImageSources[currentImageNumber];
            }
            else
            {
                accommodationImage.Source = ImageSources[currentImageNumber];
            }
        }

        private void FindAvailableDates(object sender, RoutedEventArgs e)
        {

        }
    }
}
