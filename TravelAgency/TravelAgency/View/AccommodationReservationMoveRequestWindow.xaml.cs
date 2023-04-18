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
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AccommodationReservationMoveRequestWindow.xaml
    /// </summary>
    public partial class AccommodationReservationMoveRequestWindow : Window, IDataErrorInfo, INotifyPropertyChanged
    {
        public AccommodationReservationRepository accommodationReservationRepository;
        public AccommodationReservationMoveRequestRepository moveRequestRepository;
        public AccommodationReservation Reservation { get; set; }
        public AccommodationReservationMoveRequest MoveRequest { get; set; }
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

        public AccommodationReservationMoveRequestWindow(AccommodationReservationRepository accommodationReservationRepository, AccommodationReservation reservation, AccommodationReservationMoveRequestRepository moveRequestRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Height = 600;
            this.Width = 1000;

            this.accommodationReservationRepository = accommodationReservationRepository;
            this.moveRequestRepository = moveRequestRepository;


            Reservation = reservation;
            MoveRequest = new AccommodationReservationMoveRequest(Reservation);

            DayNumber = Reservation.DateSpan.EndDate.DayNumber - Reservation.DateSpan.StartDate.DayNumber + 1;
            FirstDate = DateTime.Now.Date;
            LastDate = DateTime.Now.Date;
            AvailableDateSpans = new ObservableCollection<DateSpan>();

            nameLabel.Content = "Name: " + Reservation.Accommodation.Name;
            locationLabel.Content = "Location: " + Reservation.Accommodation.Location.City + ", " + Reservation.Accommodation.Location.Country;
            typeLabel.Content = "Type: " + Reservation.Accommodation.Type;
            maxGuestsLabel.Content = "Max. guests: " + Reservation.Accommodation.MaxGuests;
            minDaysLabel.Content = "Min. days: " + Reservation.Accommodation.MinDays;
            daysToCancelLabel.Content = "Days to cancel: " + Reservation.Accommodation.DaysToCancel;
            ownerLabel.Content = "Owner: " + Reservation.Accommodation.Owner.Username;
            firstDatePicker.DisplayDateStart = DateTime.Today;
            lastDatePicker.DisplayDateStart = DateTime.Today;
            ShouldValidate = true;

            LoadPhotos();
        }

        private void LoadPhotos()
        {
            Photos = new List<BitmapImage>();

            foreach (AccommodationPhoto accommodationPhoto in Reservation.Accommodation.Photos)
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
                AvailableDateSpans = new ObservableCollection<DateSpan>(accommodationReservationRepository.FindDatesForReservationMoveRequest(FirstDate, LastDate, Reservation));
                dateSpansDataGrid.ItemsSource = AvailableDateSpans;

                dateSpansDataGrid.Visibility = Visibility.Visible;
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
                    else if (DayNumber < Reservation.Accommodation.MinDays)
                    {
                        return "* Number of days is smaller than allowed";
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

        private void MakeReservationMoveRequest(object sender, RoutedEventArgs e)
        {
            if (MoveRequest.DateSpan != null)
            {
                moveRequestRepository.Save(MoveRequest);
                Guest1Main.ReservationMoveRequests.Add(MoveRequest);
                Close();
            }
            else
            {
                System.Windows.MessageBox.Show("Reservation move request wasn't properly specified!");
            }
        }
    }
}
