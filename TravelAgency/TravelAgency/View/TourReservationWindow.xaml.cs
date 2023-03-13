using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
    public partial class TourReservationWindow : Window, INotifyPropertyChanged
    {
        public TourOccurrence TourOccurrence { get; set; }
        //jel i ovo treba sa malom crtom ispred?
        private ObservableCollection<TourOccurrence> tourOccurrences;
        public static bool reservationFinished;
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

        public TourReservationWindow(TourOccurrence tourOccurrence, ObservableCollection<TourOccurrence> tO)
        {
            InitializeComponent();
            DataContext = this;
            TourOccurrence= tourOccurrence;
            i = 0;
            imagesCount=TourOccurrence.Tour.Photos.Count;
            images = TourOccurrence.Tour.Photos;
            ImageUrl = images[i].Link;
            SpotsLeft = "";
            tourOccurrences = tO;
            reservationFinished = false;
            AddGuestsButton.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int input;
            if (int.TryParse(NumberOfGuestsInput, out input))
            {
                int spotsLeft = TourOccurrence.Tour.MaxGuestNumber - (TourOccurrence.Guests.Count + input);
                if (spotsLeft < 0)
                {
                    SpotsLeft = "Not enough spots on tour";
                    AddGuestsButton.IsEnabled = false;
                }
                else
                {
                    SpotsLeft = spotsLeft.ToString();
                    AddGuestsButton.IsEnabled = true;
                }
            }
            else
            {
                SpotsLeft = "Wrong input";
                AddGuestsButton.IsEnabled = false;
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

        private void AlternativeToursClick(object sender, RoutedEventArgs e)
        {
            AlternativeTours alternativeTours = new AlternativeTours(tourOccurrences, TourOccurrence.Id, TourOccurrence.Tour.Location, this);
            alternativeTours.Show();
        }
       
        private void AddGuestsClick(object sender, RoutedEventArgs e)
        {
            if (AddGuestsButton.IsEnabled)
            {
                int input;
                input = int.Parse(NumberOfGuestsInput);
                TourGuests tourGuests = new TourGuests(input, TourOccurrence, this);
                tourGuests.Show();
                if (reservationFinished)
                    Close();
            }
        }
        public void CloseWindow()
        {
            Close();
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
