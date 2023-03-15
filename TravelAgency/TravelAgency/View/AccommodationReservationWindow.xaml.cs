﻿using System;
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

        public List<string> ImageSources { get; set; }
        public int currentImageNumber;
        public AccommodationReservationWindow(User guest, Accommodation accommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Height = 700;
            this.Width = 1000;

            Guest = guest;
            Accommodation = accommodation;

            NameLabel.Content = "Name: " + Accommodation.Name;
            LocationLabel.Content = "Location: " + Accommodation.Location.City + ", " + Accommodation.Location.Country;
            TypeLabel.Content = "Type: " + Accommodation.Type;
            MaxGuestsLabel.Content = "Max. guests: " + Accommodation.MaxGuests;
            MinDaysLabel.Content = "Min. days: " + Accommodation.MinDays;
            DaysToCancelLabel.Content = "Days to cancel: " + Accommodation.DaysToCancel;
            OwnerLabel.Content = "Owner: " + Accommodation.Owner.Username;


            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            string img1 = "https://optimise2.assets-servd.host/maniacal-finch/production/animals/amur-tiger-01-01.jpg?w=1200&auto=compress%2Cformat&fit=crop&dm=1658935145&s=1b96c26544a1ee414f976c17b18f2811";
            string img2 = "..\\Resources\\Images\\ProfilePicture.jpg";
            string img3 = "https://www.sfzoo.org/wp-content/uploads/2021/03/AfricanLionJasiri_resize2019.jpg";

            ImageSources = new List<string>();
            ImageSources.Add(img1);
            ImageSources.Add(img2);
            ImageSources.Add(img3);

            currentImageNumber = 0;
            Uri uri = new Uri(ImageSources[currentImageNumber]);
            AccommodationImage.Source = new BitmapImage(uri);
            

            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        }

        private void ShowPreviousImage(object sender, RoutedEventArgs e)
        {
            currentImageNumber--;

            if (currentImageNumber == -1)
            {
                currentImageNumber = ImageSources.Count() - 1;
                Uri uri = new Uri(ImageSources[currentImageNumber], UriKind.RelativeOrAbsolute);
                AccommodationImage.Source = new BitmapImage(uri);
            }
            else
            {
                Uri uri = new Uri(ImageSources[currentImageNumber], UriKind.RelativeOrAbsolute);
                AccommodationImage.Source = new BitmapImage(uri);
            }
        }

        private void ShowNextImage(object sender, RoutedEventArgs e)
        {
            currentImageNumber++;

            if (currentImageNumber == ImageSources.Count())
            {
                currentImageNumber = 0;
                Uri uri = new Uri(ImageSources[currentImageNumber], UriKind.RelativeOrAbsolute);
                AccommodationImage.Source = new BitmapImage(uri);
            }
            else
            {
                Uri uri = new Uri(ImageSources[currentImageNumber], UriKind.RelativeOrAbsolute);
                AccommodationImage.Source = new BitmapImage(uri);
            }
        }
    }
}