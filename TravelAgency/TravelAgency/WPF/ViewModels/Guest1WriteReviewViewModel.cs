using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1WriteReviewViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationOwnerRatingService _ratingService;
        private RenovationService _renovationService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand SendReviewCommand { get; private set; }
        public MyICommand PreviousAccommodationPhotoCommand { get; private set; }
        public MyICommand NextAccommodationPhotoCommand { get; private set; }
        public MyICommand PreviousRatingPhotoCommand { get; private set; }
        public MyICommand NextRatingPhotoCommand { get; private set; }
        public MyICommand AddRatingPhotoCommand { get; private set; }
        public MyICommand RemoveRatingPhotoCommand { get; private set; }

        private AccommodationReservation _stay;
        private AccommodationOwnerRating _rating;
        private RenovationRecommendation _renovationRecommendation;
        private List<BitmapImage> _accommodationPhotos;
        private BitmapImage _selectedAccommodationPhoto;
        private int _currentAccommodationPhotoIndex;
        private List<BitmapImage> _ratingPhotos;
        private BitmapImage _selectedRatingPhoto;
        private string _ratingPhotoPath;
        private BitmapImage _placeholderPhoto;
        private int _currentRatingPhotoIndex;
        private bool _writeRenovationRecommendation;

        public AccommodationReservation Stay
        {
            get => _stay;
            set
            {
                if (value != _stay)
                {
                    _stay = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationOwnerRating Rating
        {
            get => _rating;
            set
            {
                if (value != _rating)
                {
                    _rating = value;
                    OnPropertyChanged();
                }
            }
        }

        public RenovationRecommendation RenovationRecommendation
        {
            get => _renovationRecommendation;
            set
            {
                if (value != _renovationRecommendation)
                {
                    _renovationRecommendation = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<BitmapImage> AccommodationPhotos
        {
            get => _accommodationPhotos;
            set
            {
                if (value != _accommodationPhotos)
                {
                    _accommodationPhotos = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage SelectedAccommodationPhoto
        {
            get => _selectedAccommodationPhoto;
            set
            {
                if (value != _selectedAccommodationPhoto)
                {
                    _selectedAccommodationPhoto = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<BitmapImage> RatingPhotos
        {
            get => _ratingPhotos;
            set
            {
                if (value != _ratingPhotos)
                {
                    _ratingPhotos = value;
                    OnPropertyChanged();
                }
            }
        }

        public BitmapImage SelectedRatingPhoto
        {
            get => _selectedRatingPhoto;
            set
            {
                if (value != _selectedRatingPhoto)
                {
                    _selectedRatingPhoto = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RatingPhotoPath
        {
            get => _ratingPhotoPath;
            set
            {
                if (value != _ratingPhotoPath)
                {
                    _ratingPhotoPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool WriteRenovationRecommendation
        {
            get => _writeRenovationRecommendation;
            set
            {
                if (value != _writeRenovationRecommendation)
                {
                    _writeRenovationRecommendation = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1WriteReviewViewModel(MyICommand<string> navigationCommand, AccommodationReservation stay)
        {
            _ratingService = new AccommodationOwnerRatingService();
            _renovationService = new RenovationService();

            NavigationCommand = navigationCommand;
            SendReviewCommand = new MyICommand(OnSendReview);
            PreviousAccommodationPhotoCommand = new MyICommand(OnGetPreviousAccommodationPhoto);
            NextAccommodationPhotoCommand = new MyICommand(OnGetNextAccommodationPhoto);
            PreviousRatingPhotoCommand = new MyICommand(OnGetPreviousRatingPhoto);
            NextRatingPhotoCommand = new MyICommand(OnGetNextRatingPhoto);
            AddRatingPhotoCommand = new MyICommand(OnAddRatingPhoto);
            RemoveRatingPhotoCommand = new MyICommand(OnRemoveRatingPhoto);

            Stay = stay;

            InitializeData();
        }

        private void InitializeData()
        {
            _writeRenovationRecommendation = false;
            Rating = new AccommodationOwnerRating(Stay);
            RenovationRecommendation = new RenovationRecommendation(Rating);
            RatingPhotoPath = "";
            InitializePhotos();
        }

        private void InitializePhotos()
        {
            AccommodationPhotos = new List<BitmapImage>();
            RatingPhotos = new List<BitmapImage>();
            foreach (AccommodationPhoto photo in Stay.Accommodation.Photos)
            {
                Uri uri = new Uri(photo.Path, UriKind.RelativeOrAbsolute);
                BitmapImage image = new BitmapImage(uri);
                AccommodationPhotos.Add(image);
            }
            SelectedAccommodationPhoto = AccommodationPhotos[0];
            _currentAccommodationPhotoIndex = 0;
            Uri uriPlaceholder = new Uri("https://media.istockphoto.com/id/1147544807/vector/thumbnail-image-vector-graphic.jpg?s=612x612&w=0&k=20&c=rnCKVbdxqkjlcs3xH87-9gocETqpspHFXu5dIGB4wuM=", UriKind.RelativeOrAbsolute);
            _placeholderPhoto = new BitmapImage(uriPlaceholder);
            RatingPhotos.Add(_placeholderPhoto);
            _currentRatingPhotoIndex = 0;
            SelectedRatingPhoto = _placeholderPhoto;
        }

        public void OnGetNextAccommodationPhoto()
        {
            if (++_currentAccommodationPhotoIndex > (AccommodationPhotos.Count() - 1))
            {
                _currentAccommodationPhotoIndex = 0;
            }
            SelectedAccommodationPhoto = AccommodationPhotos[_currentAccommodationPhotoIndex];
        }

        public void OnGetPreviousAccommodationPhoto()
        {
            if (--_currentAccommodationPhotoIndex < 0)
            {
                _currentAccommodationPhotoIndex = AccommodationPhotos.Count() - 1;
            }
            SelectedAccommodationPhoto = AccommodationPhotos[_currentAccommodationPhotoIndex];
        }

        public void OnAddRatingPhoto()
        {
            RatingPhotos.Remove(_placeholderPhoto);
            Uri uri = new Uri(RatingPhotoPath, UriKind.RelativeOrAbsolute);
            BitmapImage image = new BitmapImage(uri);
            RatingPhotos.Add(image);
            OnGetNextRatingPhoto();
            AccommodationRatingPhoto photo = new AccommodationRatingPhoto(RatingPhotoPath);
            Rating.Photos.Add(photo);
            RatingPhotoPath = "";
        }

        public void OnRemoveRatingPhoto()
        {
            Rating.Photos.Remove(Rating.Photos[_currentRatingPhotoIndex]);
            if (SelectedRatingPhoto != _placeholderPhoto)
            {
                RatingPhotos.Remove(SelectedRatingPhoto);
            }
            if (RatingPhotos.Count() == 0)
            {
                RatingPhotos.Add(_placeholderPhoto);
            }
            OnGetPreviousRatingPhoto();
        }

        public void OnGetNextRatingPhoto()
        {
            if (++_currentRatingPhotoIndex > (RatingPhotos.Count() - 1))
            {
                _currentRatingPhotoIndex = 0;
            }
            if (RatingPhotos.Contains(_placeholderPhoto))
            {
                _currentRatingPhotoIndex = 0;
            }
            SelectedRatingPhoto = RatingPhotos[_currentRatingPhotoIndex];
        }

        public void OnGetPreviousRatingPhoto()
        {
            if (--_currentRatingPhotoIndex < 0)
            {
                _currentRatingPhotoIndex = RatingPhotos.Count() - 1;
            }
            SelectedRatingPhoto = RatingPhotos[_currentRatingPhotoIndex];
        }

        public void OnSendReview()
        {
            if (Rating.IsValid)
            {
                if (WriteRenovationRecommendation)
                {
                    if (!RenovationRecommendation.IsValid)
                    {
                        return;
                    }
                    _ratingService.CreateRating(Rating);
                    _renovationService.RecommendRenovation(Rating, RenovationRecommendation);
                    string messageBoxText = "Ocena i preporuka za renoviraje su uspešno formirani.";
                    string caption = "Ocenjivanje vlasnika i smeštaja";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Information;
                    MessageBoxResult result;
                    result = MessageBox.Show(messageBoxText, caption, button, icon);
                    if (result == MessageBoxResult.OK)
                    {
                        NavigationCommand.Execute("guest1RateableStaysViewModel");
                    }
                    return;
                }
                _ratingService.CreateRating(Rating);
                string messageBoxText1 = "Ocena je uspešno formirana.";
                string caption1 = "Ocenjivanje vlasnika i smeštaja";
                MessageBoxButton button1 = MessageBoxButton.OK;
                MessageBoxImage icon1 = MessageBoxImage.Information;
                MessageBoxResult result1;
                result1 = MessageBox.Show(messageBoxText1, caption1, button1, icon1);
                if (result1 == MessageBoxResult.OK)
                {
                    NavigationCommand.Execute("guest1RateableStaysViewModel");
                }
                return;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
