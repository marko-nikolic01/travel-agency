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
        public List<BitmapImage> Photos { get; set; }
        public int currentPhotoIndex;
        
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

            Guest = guest;
            Accommodation = accommodation;
            Reservation = new AccommodationReservation(Accommodation.Id, Accommodation, Guest.Id, Guest);

            DayNumber = Accommodation.MinDays;
            FirstDate = DateTime.Now.Date;
            LastDate = DateTime.Now.Date;
            AvailableDateSpans = new ObservableCollection<DateSpan>();

            nameLabel.Content = "Name: " + Accommodation.Name;
            locationLabel.Content = "Location: " + Accommodation.Location.City + ", " + Accommodation.Location.Country;
            typeLabel.Content = "Type: " + Accommodation.Type;
            maxGuestsLabel.Content = "Max. guests: " + Accommodation.MaxGuests;
            minDaysLabel.Content = "Min. days: " + Accommodation.MinDays;
            daysToCancelLabel.Content = "Days to cancel: " + Accommodation.DaysToCancel;
            ownerLabel.Content = "Owner: " + Accommodation.Owner.Username;
            firstDatePicker.DisplayDateStart = DateTime.Today;
            lastDatePicker.DisplayDateStart = DateTime.Today;

            LoadPhotos();
        }

        private void LoadPhotos()
        {
            Photos = new List<BitmapImage>();

            foreach (AccommodationPhoto accommodationPhoto in Accommodation.Images)
            {
                Uri uri = new Uri(accommodationPhoto.Path, UriKind.RelativeOrAbsolute);
                BitmapImage photo = new BitmapImage(uri);
                Photos.Add(photo);
            }

            currentPhotoIndex = 0;
            accommodationPhoto.Source = Photos[0];
        }

        private void ShowPreviousImage(object sender, RoutedEventArgs e)
        {
            currentPhotoIndex--;

            if (currentPhotoIndex == -1)
            {
                currentPhotoIndex = Photos.Count() - 1;
                accommodationPhoto.Source = Photos[currentPhotoIndex];
            }
            else
            {
                accommodationPhoto.Source = Photos[currentPhotoIndex];
            }
        }

        private void ShowNextImage(object sender, RoutedEventArgs e)
        {
            currentPhotoIndex++;

            if (currentPhotoIndex == Photos.Count())
            {
                currentPhotoIndex = 0;
                accommodationPhoto.Source = Photos[currentPhotoIndex];
            }
            else
            {
                accommodationPhoto.Source = Photos[currentPhotoIndex];
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
