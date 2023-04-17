using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TravelAgency.Model;
using TravelAgency.Repository;

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
        bool ShouldValidate { get; set; }

        public int DayNumber
        {
            get => _dayNumber;
            set
            {
                if (value != _dayNumber)
                {
                    _dayNumber = value;
                    OnPropertyChanged(nameof(DayNumber));
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
                    OnPropertyChanged(nameof(FirstDate));
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
                    OnPropertyChanged(nameof(LastDate));
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
            ShouldValidate = true;

            LoadPhotos();
        }

        private void LoadPhotos()
        {
            Photos = new List<BitmapImage>();

            foreach (AccommodationPhoto accommodationPhoto in Accommodation.Photos)
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
                AvailableDateSpans = new ObservableCollection<DateSpan>(accommodationReservationRepository.FindAvailableDatesInsideDateRange(FirstDate, LastDate, Accommodation.Id));
                dateSpansDataGrid.ItemsSource = AvailableDateSpans;

                if (AvailableDateSpans.Count == 0)
                {
                    AvailableDateSpans = new ObservableCollection<DateSpan>(accommodationReservationRepository.FindAvailableDatesOutsideDateRange(FirstDate, LastDate, Accommodation.Id));
                    dateSpansDataGrid.ItemsSource = AvailableDateSpans;
                    System.Windows.MessageBox.Show("There aren't any dates available in the specified date span! Pick one of our suggestions or adjust your search.");
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
                        return "* Number of days is required";
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
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
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
                    if (dateSpanLength <= 0)
                    {
                        return "*Last date can't be before first date";
                    }
                    else if (dateSpanLength < DayNumber)
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

        private void DatePickerSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ShouldValidate)
            {
                return;
            }
            ShouldValidate = false;

            var datePicker = (sender as DatePicker);

            if (datePicker.Name == firstDatePicker.Name)
            {
                TriggerValidationMessage(false, true);
            }

            if (datePicker.Name == lastDatePicker.Name)
            {
                TriggerValidationMessage(true, false);
            }
        }

        private void DayNumberSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ShouldValidate = false;
            TriggerValidationMessage(true, true);
        }

        private void TriggerValidationMessage(bool shouldValidateFirst, bool shouldValidateLast)
        {
            if (shouldValidateFirst)
            {
                FirstDate = FirstDate.AddDays(1);
                FirstDate = FirstDate.AddDays(-1);
            }

            if (shouldValidateLast)
            {
                LastDate = LastDate.AddDays(1);
                LastDate = LastDate.AddDays(-1);
            }

            ShouldValidate = true;
        }

        private void MakeReservation(object sender, RoutedEventArgs e)
        {
            if (Reservation.IsValid)
            {
                accommodationReservationRepository.Save(Reservation);
                Guest1Main.Reservations.Add(Reservation);
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Reservation wasn't properly specified!");
            }
        }


    }
}
