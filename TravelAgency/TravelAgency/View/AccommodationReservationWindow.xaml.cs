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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationWindow.xaml
    /// </summary>
    public partial class AccommodationReservationWindow : Window, IDataErrorInfo, INotifyPropertyChanged
    {
        public User Guest { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationReservation Reservation { get; set; }
        private int _dayNumber;
        private DateTime _startDate;
        private DateTime _endDate;
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

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationWindow(User guest, Accommodation accommodation)
        {
            StartDate = new DateTime();
            EndDate = new DateTime();

            InitializeComponent();
            this.DataContext = this;
            this.Height = 600;
            this.Width = 1000;

            Guest = guest;
            Accommodation = accommodation;
            Reservation = new AccommodationReservation(Accommodation.Id, Accommodation, Guest.Id, Guest);

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
            if (this.IsValid)
            {

            }
            else 
            {
                MessageBox.Show("Date span wasn't properly specified!");
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
                else if (columnName == "StartDate")
                {
                    bool isFutureDate = StartDate.CompareTo(DateTime.Now) > 0;
                    
                    if (!isFutureDate)
                    {
                        return "* Start date must be a future date";
                    }

                    double dateSpanLength = (EndDate - StartDate).TotalDays + 1;
                    if (dateSpanLength < 0)
                    {
                        return "*Start date can't be after end date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                         return "*Date span can't be shorter thanspecified\nnumber of days";
                    }

                }
                else if (columnName == "EndDate")
                {
                    bool isFutureDate = EndDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* End date must be a future date";
                    }

                    double dateSpanLength = (EndDate - StartDate).TotalDays + 1;
                    if (dateSpanLength < 0)
                    {
                        return "*End date can't be before start date";
                    }
                    else if(dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified\nnumber of days";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "DayNumber", "StartDate", "EndDate" };

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

        private void DayNumberSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            StartDate = StartDate.AddDays(1);
            StartDate = StartDate.AddDays(-1);
            EndDate = EndDate.AddDays(1);
            EndDate = EndDate.AddDays(-1);
        }


        private void DateLostFocus(object sender, RoutedEventArgs e)
        {
            StartDate = StartDate.AddDays(1);
            StartDate = StartDate.AddDays(-1);
            EndDate = EndDate.AddDays(1);
            EndDate = EndDate.AddDays(-1);
        }
    }
}
