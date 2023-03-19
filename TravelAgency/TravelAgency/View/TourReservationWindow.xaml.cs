using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.Model;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window, INotifyPropertyChanged
    {
        public TourOccurrence TourOccurrence { get; set; }
        private ObservableCollection<TourOccurrence> tourOccurrences;
        private string imageUrl;
        private string numberOfGuestsInput;
        private string spotsLeft;
        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                if(value != imageUrl)
                {
                    imageUrl = value;
                    OnPropertyChanged();
                }
            }
        }
        public string NumberOfGuestsInput
        {
            get => numberOfGuestsInput;
            set
            {
                if (value != numberOfGuestsInput)
                {
                    numberOfGuestsInput = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SpotsLeft
        {
            get => spotsLeft;
            set
            {
                if (value != spotsLeft)
                {
                    spotsLeft = value;
                    OnPropertyChanged();
                }
            }
        }
        private int i, imagesCount;
        private List<Photo> images;
        private User activeGuest;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourReservationWindow(TourOccurrence tourOccurrence, ObservableCollection<TourOccurrence> tourOccurrences, User user)
        {
            InitializeComponent();
            DataContext = this;
            TourOccurrence = tourOccurrence;
            i = 0;
            imagesCount = TourOccurrence.Tour.Photos.Count;
            images = TourOccurrence.Tour.Photos;
            ImageUrl = images[i].Link;
            SpotsLeft = "";
            this.tourOccurrences = tourOccurrences;
            AddGuestsButton.IsEnabled = false;
            activeGuest = user;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int input;
            if (int.TryParse(NumberOfGuestsInput, out input))
            {
                CheckSpotsNumber(input);
            }
            else
            {
                SpotsLeft = "Wrong input";
                AddGuestsButton.IsEnabled = false;
            }
        }

        private void CheckSpotsNumber(int input)
        {
            if (input <= 0)
            {
                SpotsLeft = "Wrong input";
                AddGuestsButton.IsEnabled = false;
            }
            else
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
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
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

        private void AlternativeTours_Click(object sender, RoutedEventArgs e)
        {
            AlternativeTours alternativeTours = new AlternativeTours(tourOccurrences, TourOccurrence.Id, TourOccurrence.Tour.Location, activeGuest, this);
            alternativeTours.Show();
        }
       
        private void AddGuests_Click(object sender, RoutedEventArgs e)
        {
            if (AddGuestsButton.IsEnabled)
            {
                int input;
                input = int.Parse(NumberOfGuestsInput);
                TourGuests tourGuests = new TourGuests(input, TourOccurrence, this, activeGuest);
                tourGuests.Show();
            }
        }
        public void CloseWindow()
        {
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
