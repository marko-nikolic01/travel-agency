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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for GuideMain.xaml
    /// </summary>
    public partial class GuideMain : Window, IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourRepository TourRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public KeyPointRepository KeyPointRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public static TourReservationRepository TourReservationRepository { get; set; } 
        public ObservableCollection<User> Guests { get; set; }
        public ObservableCollection<KeyPoint> StartedTourKeyPoints { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public KeyPoint SelectedKeyPoint { get; set; }
        public int ChosenKeyPointId { get; set; }
        public User ChosenGuest { get; set; }
        public GuideMain()
        {
            InitializeComponent();
            DataContext = this;
            TourRepository = new TourRepository();
            LocationRepository = new LocationRepository();
            PhotoRepository = new PhotoRepository();
            TourOccurrenceRepository = new TourOccurrenceRepository();
            KeyPointRepository = new KeyPointRepository();
            TourReservationRepository = new TourReservationRepository();
            UserRepository = new UserRepository();
            LinkingTourLocation();
            LinkingTourPhotos();
            LinkingTourOccurrences();
            LinkingKeyPoints();
            LinkingTourGuests();
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceRepository.GetTourOccurrences());
            Guests = new ObservableCollection<User>();
            StartedTourKeyPoints = new ObservableCollection<KeyPoint>();
            TourOccurrenceRepository.Subscribe(this);
        }

        private void LinkingTourGuests()
        {
            foreach (TourReservation tourReservation in TourReservationRepository.GetTourReservations())
            {
                TourOccurrence tourOccurrence = TourOccurrenceRepository.GetTourOccurrences().Find(x => x.Id == tourReservation.TourOccurrenceId);
                User guest = UserRepository.GetUsers().Find(x => x.Id == tourReservation.UserId);
                tourOccurrence.Guests.Add(guest);
            }
        }

        private void LinkingKeyPoints()
        {
            foreach(KeyPoint keyPoint in KeyPointRepository.GetKeyPoints())
            {
                TourOccurrence tourOccurrence = TourOccurrenceRepository.GetTourOccurrences().Find(tO => tO.Id == keyPoint.TourOccurrenceId);
                if (tourOccurrence != null)
                {
                    tourOccurrence.KeyPoints.Add(keyPoint);
                }
            }
        }

        private void LinkingTourOccurrences()
        {
            foreach(TourOccurrence tourOccurrence in TourOccurrenceRepository.GetTourOccurrences())
            {
                Tour tour = TourRepository.GetTours().Find(t => t.Id == tourOccurrence.TourId);
                if (tour != null)
                {
                    tourOccurrence.Tour = tour;
                }
            }
        }

        private void LinkingTourPhotos()
        {
            foreach(Photo photo in PhotoRepository.GetPhotos())
            {
                Tour tour = TourRepository.GetTours().Find(t => t.Id == photo.TourId);
                if(tour != null)
                {
                    tour.Photos.Add(photo);
                }
            }
        }

        private void LinkingTourLocation()
        {
            foreach(var tour in TourRepository.GetTours())
            {
                Location location = LocationRepository.GetLocations().Find(l => l.Id == tour.Id);
                if(location != null)
                {
                    tour.Location = location;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SignOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void NewTourClick(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(TourRepository, LocationRepository, PhotoRepository, TourOccurrenceRepository, KeyPointRepository);
            createTour.Show();
        }

        public void Update()
        {
            TourOccurrences.Clear();
            foreach(TourOccurrence tourOccurrence in TourOccurrenceRepository.GetTourOccurrences())
            {
                TourOccurrences.Add(tourOccurrence);
            }
        }

        private void StopClick(object sender, RoutedEventArgs e)
        {
            Guests.Clear();
            StartedTourKeyPoints.Clear();
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            StartedTourKeyPoints.Clear();
            foreach (KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                StartedTourKeyPoints.Add(keyPoint);
            }
            Guests.Clear();
            foreach(User user in SelectedTourOccurrence.Guests)
            {
                Guests.Add(user);
            }
            
        }

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(ChosenGuest.Username + ChosenKeyPointId.ToString());
        }
    }
}
