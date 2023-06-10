using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1WhereverWheneverReservationViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationReservationService _reservationService;
        private WhereverWheneverService _whereverWheneverService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand MakeReservationCommand { get; private set; }
        public MyICommand PreviousPhotoCommand { get; private set; }
        public MyICommand NextPhotoCommand { get; private set; }

        public User Guest { get; set; }
        private Accommodation _accommodation;
        private WhereverWheneverSearchFilter _searchFilter;
        private AccommodationReservation _reservation;
        private ObservableCollection<DateSpan> _availableDateSpans;
        private DateSpan _selectedDateSpan;
        private List<BitmapImage> _photos;
        private BitmapImage _selectedPhoto;
        private int _currentPhotoIndex;

        public Accommodation Accommodation
        {
            get => _accommodation;
            set
            {
                if (value != _accommodation)
                {
                    _accommodation = value;
                    OnPropertyChanged();
                }
            }
        }

        public WhereverWheneverSearchFilter SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (value != _searchFilter)
                {
                    _searchFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationReservation Reservation
        {
            get => _reservation;
            set
            {
                if (value != _reservation)
                {
                    _reservation = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<DateSpan> AvailableDateSpans
        {
            get => _availableDateSpans;
            set
            {
                if (value != _availableDateSpans)
                {
                    _availableDateSpans = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateSpan SelectedDateSpan
        {
            get => _selectedDateSpan;
            set
            {
                if (value != _selectedDateSpan)
                {
                    _selectedDateSpan = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<BitmapImage> Photos
        {
            get => _photos;
            set
            {
                if (value != _photos)
                {
                    _photos = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage SelectedPhoto
        {
            get => _selectedPhoto;
            set
            {
                if (value != _selectedPhoto)
                {
                    _selectedPhoto = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1WhereverWheneverReservationViewModel(MyICommand<string> navigationCommand, User guest, Accommodation accommodation, WhereverWheneverSearchFilter searchFilter)
        {
            _reservationService = new AccommodationReservationService();
            _whereverWheneverService = new WhereverWheneverService();

            NavigationCommand = navigationCommand;
            MakeReservationCommand = new MyICommand(OnMakeReservation);
            PreviousPhotoCommand = new MyICommand(OnGetPreviousPhoto);
            NextPhotoCommand = new MyICommand(OnGetNextPhoto);

            Guest = guest;
            Accommodation = accommodation;
            Reservation = new AccommodationReservation(accommodation.Id, accommodation, Guest.Id, Guest);
            SearchFilter = searchFilter;

            InitializeData();
        }

        private void InitializeData()
        {
            InitializePhotos();
            InitializeDateSpanData();
            Reservation.NumberOfGuests = SearchFilter.GuestNumber;
        }


        private void InitializePhotos()
        {
            Photos = new List<BitmapImage>();
            foreach (AccommodationPhoto photo in Accommodation.Photos)
            {
                Uri uri = new Uri(photo.Path, UriKind.RelativeOrAbsolute);
                BitmapImage image = new BitmapImage(uri);
                Photos.Add(image);
            }
            SelectedPhoto = Photos[0];
            _currentPhotoIndex = 0;
        }

        private void InitializeDateSpanData()
        {
            List<DateSpan> availableDateSpans = _whereverWheneverService.GetAvailableDateSpans(SearchFilter, Accommodation);
            AvailableDateSpans = new ObservableCollection<DateSpan>(availableDateSpans);
        }

        public void OnGetNextPhoto()
        {
            if (++_currentPhotoIndex > (Photos.Count() - 1))
            {
                _currentPhotoIndex = 0;
            }
            SelectedPhoto = Photos[_currentPhotoIndex];
        }

        public void OnGetPreviousPhoto()
        {
            if (--_currentPhotoIndex < 0)
            {
                _currentPhotoIndex = Photos.Count() - 1;
            }
            SelectedPhoto = Photos[_currentPhotoIndex];
        }

        public void OnMakeReservation()
        {
            if (!Reservation.IsValid) return;
            string messageBoxText = "Da li ste sigurni da želite da rezervišete smeštaj?\nSmeštaj: " + Accommodation.Name +
                "\nLokacija: " + Accommodation.Location.City + ", " + Accommodation.Location.Country +
                "\nTermin: " + Reservation.DateSpan.StartDate.ToString() + " - " + Reservation.DateSpan.EndDate.ToString();
            string caption = "Rezervacija smeštaja";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                _reservationService.CreateReservation(Reservation);
                NavigationCommand.Execute("previousViewModel");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
