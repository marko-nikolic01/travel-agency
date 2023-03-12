using System;
using System.Collections.Generic;
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
using TravelAgency.Model;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservation : Window, INotifyPropertyChanged
    {
        public TourOccurrence TourOccurrence { get; set; }
        private string _imageUrl;
        private string _numberOfGuestsInput;
        private string _spotsLeft;
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if(value != _imageUrl)
                {
                    _imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NumberOfGuestsInput
        {
            get => _numberOfGuestsInput;
            set
            {
                if (value != _numberOfGuestsInput)
                {
                    _numberOfGuestsInput = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SpotsLeft
        {
            get => _spotsLeft;
            set
            {
                if (value != _spotsLeft)
                {
                    _spotsLeft = value;
                    OnPropertyChanged();
                }
            }
        }
        private int i, imagesCount;
        private List<Photo> images;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourReservation(TourOccurrence tourOccurrence)
        {
            InitializeComponent();
            DataContext = this;
            TourOccurrence= tourOccurrence;
            i = 0;
            imagesCount=TourOccurrence.Tour.Photos.Count;
            images = TourOccurrence.Tour.Photos;
            ImageUrl = images[i].Link;
            SpotsLeft = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int input;
            if (int.TryParse(NumberOfGuestsInput, out input))
            {
                int spotsLeft = TourOccurrence.Tour.MaxGuestNumber - (TourOccurrence.Guests.Count + input);
                if (spotsLeft < 0)
                {
                    SpotsLeft = "There is not enough space on tour for that number of guests";
                }
                else
                {
                    SpotsLeft = spotsLeft.ToString();
                }
            }
            else
            {
                SpotsLeft = "Wrong input";
            }
        }

        private void ChangeImage(object sender, RoutedEventArgs e)
        {
            if(i!=imagesCount - 1)
            {
                i++;
            }
            else
            {
                i = 0;
            }
            ImageUrl = images[i].Link;
        }
    }
}
