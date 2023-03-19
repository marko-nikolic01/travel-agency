using System;
using System.Collections.Generic;
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
using TravelAgency.Model.DTO;
using System.ComponentModel;
using TravelAgency.Repository;
using System.Collections.ObjectModel;
using Xceed.Wpf.Toolkit;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationWindow.xaml
    /// </summary>
    public partial class AccommodationReservationWindow : Window, IDataErrorInfo, INotifyPropertyChanged
    {
        public User Guest { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationReservationRepository accommodationReservationRepository;
        public AccommodationReservation Reservation { get; set; }
        private int _dayNumber;
        private DateTime _firstDate;
        private DateTime _lastDate;
        public ObservableCollection<DateSpan> AvailableDateSpans { get; set; }
        public DateSpan SelectedDateSpan { get; set; }
        public List<BitmapImage> ImageSources { get; set; }
        public int currentImageNumber;
        
        public int DayNumber
        {
            get => _dayNumber;
            set
            {
                if (value != _dayNumber)
                {
                    _dayNumber = value;
                    OnPropertyChanged("DayNumber");
                }
            }
        }

        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                if (value != _firstDate)
                {
                    _firstDate = value;
                    OnPropertyChanged("FirstDate");
                }
            }
        }

        public DateTime LastDate
        {
            get => _lastDate;
            set
            {
                if (value != _lastDate)
                {
                    _lastDate = value;
                    OnPropertyChanged("LastDate");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationWindow(User guest, Accommodation accommodation, AccommodationReservationRepository accommodationReservationRepository)
        {
            

            InitializeComponent();
            this.DataContext = this;
            this.Height = 600;
            this.Width = 1000;

            this.accommodationReservationRepository = accommodationReservationRepository;

            FirstDate = DateTime.Now.Date;
            LastDate = DateTime.Now.Date;
            AvailableDateSpans = new ObservableCollection<DateSpan>();

            Guest = guest;
            Accommodation = accommodation;
            Reservation = new AccommodationReservation(Accommodation.Id, Accommodation, Guest.Id, Guest);

            DayNumber = Accommodation.MinDays;
            
            nameLabel.Content = "Name: " + Accommodation.Name;
            locationLabel.Content = "Location: " + Accommodation.Location.City + ", " + Accommodation.Location.Country;
            typeLabel.Content = "Type: " + Accommodation.Type;
            maxGuestsLabel.Content = "Max. guests: " + Accommodation.MaxGuests;
            minDaysLabel.Content = "Min. days: " + Accommodation.MinDays;
            daysToCancelLabel.Content = "Days to cancel: " + Accommodation.DaysToCancel;
            ownerLabel.Content = "Owner: " + Accommodation.Owner.Username;
            firstDatePicker.DisplayDateStart = DateTime.Today;
            lastDatePicker.DisplayDateStart = DateTime.Today;






            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            string img1 = "https://optimise2.assets-servd.host/maniacal-finch/production/animals/amur-tiger-01-01.jpg?w=1200&auto=compress%2Cformat&fit=crop&dm=1658935145&s=1b96c26544a1ee414f976c17b18f2811";
            string img2 = "..\\Resources\\Photos\\ProfilePicture.jpg";
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
            if (this.IsValid)
            {
                accommodationReservationRepository.SetReservationLength(DayNumber);
                AvailableDateSpans = new ObservableCollection<DateSpan>(accommodationReservationRepository.FindAvailableDatesInsideDateSpan(FirstDate, LastDate, Accommodation.Id));
                dateSpansDataGrid.ItemsSource = AvailableDateSpans;

                if (AvailableDateSpans.Count == 0)
                {
                    AvailableDateSpans = new ObservableCollection<DateSpan>(accommodationReservationRepository.FindAvailableDatesOutsideDateSpan(FirstDate, LastDate, Accommodation.Id));
                    dateSpansDataGrid.ItemsSource = AvailableDateSpans;
                }

                Reservation.DateSpan = null;
                Reservation.NumberOfGuests = 1;

                dateSpansDataGrid.Visibility = Visibility.Visible;
                guestsLabel.Visibility = Visibility.Visible;
                guestsNumberUpDown.Visibility = Visibility.Visible;
                guestsNumberUpDown.Value = 1;
                makeReservationButton.Visibility = Visibility.Visible;
            }
            else 
            {
                System.Windows.MessageBox.Show("Date span wasn't properly specified!");
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "DayNumber")
                {
                    if (DayNumber < 0)
                    {
                        return "* Number of days can't be negative";
                    }
                    else if (DayNumber == 0)
                    {
                        return "* Number of guests is required";
                    }
                    else if (DayNumber < Accommodation.MinDays)
                    {
                        return "* Number of guests is smaller than allowed";
                    }
                }
                else if (columnName == "FirstDate")
                {
                    bool isFutureDate = FirstDate.CompareTo(DateTime.Now) > 0;
                    
                    if (!isFutureDate)
                    {
                        return "* First date must be a future date";
                    }

                    double dateSpanLength = (LastDate - FirstDate).TotalDays + 1;
                    if (dateSpanLength < 0)
                    {
                        return "*First date can't be after end date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                         return "*Date span can't be shorter than specified\nnumber of days";
                    }

                }
                else if (columnName == "LastDate")
                {
                    bool isFutureDate = LastDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* Last date must be a future date";
                    }

                    double dateSpanLength = (LastDate - FirstDate).TotalDays + 1;
                    if (dateSpanLength < 0)
                    {
                    }
                    else if(dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified\nnumber of days";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "DayNumber", "FirstDate", "LastDate" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }


        private void DatePickerLostFocus(object sender, RoutedEventArgs e)
        {
            FirstDate = FirstDate.AddDays(1);
            FirstDate = FirstDate.AddDays(-1);
            LastDate = LastDate.AddDays(1);
            LastDate = LastDate.AddDays(-1);
        }

        private void DayNumberSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FirstDate = FirstDate.AddDays(1);
            FirstDate = FirstDate.AddDays(-1);
            LastDate = LastDate.AddDays(1);
            LastDate = LastDate.AddDays(-1);
        }

        private void MakeReservation(object sender, RoutedEventArgs e)
        {
            if (Reservation.IsValid)
            {
                accommodationReservationRepository.Save(Reservation);
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Reservation wasn't properly specified!");
            }
        }


    }
}
