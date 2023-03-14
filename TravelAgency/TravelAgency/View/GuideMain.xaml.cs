using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
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
            LinkTourLocation();
            LinkTourPhotos();
            LinkTourOccurrences();
            LinkKeyPoints();
            LinkTourGuests();
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceRepository.GetTourOccurrences());
            Guests = new ObservableCollection<User>();
            StartedTourKeyPoints = new ObservableCollection<KeyPoint>();
            TourOccurrenceRepository.Subscribe(this);
            foreach (TourOccurrence tourOccurrence in TourOccurrences)
            {
                if (tourOccurrence.CurrentState == CurrentState.Ended)
                {
                    tourOccurrence.ToShadow = 1;
                    tourOccurrence.ToDisplay = 0;
                }
            }
        }

        private void LinkTourGuests()
        {
            foreach (TourReservation tourReservation in TourReservationRepository.GetTourReservations())
            {
                TourOccurrence tourOccurrence = TourOccurrenceRepository.GetTourOccurrences().Find(x => x.Id == tourReservation.TourOccurrenceId);
                User guest = UserRepository.GetUsers().Find(x => x.Id == tourReservation.UserId);
                tourOccurrence.Guests.Add(guest);
            }
        }

        private void LinkKeyPoints()
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

        private void LinkTourOccurrences()
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

        private void LinkTourPhotos()
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

        private void LinkTourLocation()
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

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            if(SelectedTourOccurrence == null)
            {
                MessageBox.Show("You have to select the tour you would like to start first!");
                return;
            }
            if(SelectedTourOccurrence.Guests.Count == 0)
            {
                MessageBox.Show("No guests have reserved this tour, therefore it can't be started!");
                return;
            }
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            NewTourButton.IsEnabled = false;
            SignOutButton.IsEnabled = false;
            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
            foreach (TourOccurrence tourOccurrence in TourOccurrences)
            {
                if (tourOccurrence == SelectedTourOccurrence)
                {
                    tourOccurrence.CurrentState = CurrentState.Started;
                    tourOccurrence.KeyPoints[0].IsChecked = true;
                    continue;
                }
                tourOccurrence.ToDisplay = 0;
                tourOccurrence.ToShadow = 1;
            }

            StartedTourKeyPoints.Clear();
            StartedTourKeyPoints.Add(new KeyPoint(-1, "NOT PRESENT", new List<Guest>(), SelectedTourOccurrence.Id));
            foreach (KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                StartedTourKeyPoints.Add(keyPoint);
            }
            Guests.Clear();
            foreach(User user in SelectedTourOccurrence.Guests)
            {
                Guests.Add(user);
            }
            ComboColumn.ItemsSource = StartedTourKeyPoints;
        }

        private void StopButtonClick(object sender, RoutedEventArgs e)
        {
            EndTour();
        }

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show(ChosenGuest.Username + ChosenKeyPointId.ToString());
        }

        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            SelectedKeyPoint.IsChecked = true;
            if (SelectedTourOccurrence.KeyPoints[SelectedTourOccurrence.KeyPoints.Count - 1].Id == SelectedKeyPoint.Id)
            {
                EndTour();
            }
        }

        private void EndTour()
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            NewTourButton.IsEnabled = true;
            SignOutButton.IsEnabled = true;
            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
            foreach (TourOccurrence tourOccurrence in TourOccurrences)
            {
                if (tourOccurrence.CurrentState == CurrentState.Started)
                {
                    tourOccurrence.CurrentState = CurrentState.Ended;
                    tourOccurrence.ToShadow = 1;
                    tourOccurrence.ToDisplay = 0;
                }
                if (tourOccurrence.CurrentState != CurrentState.Ended)
                {
                    tourOccurrence.ToShadow = 0;
                    tourOccurrence.ToDisplay = 1;
                }
            }
            Guests.Clear();
            StartedTourKeyPoints.Clear();
            foreach(KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                KeyPointRepository.UpdateKeyPoint(keyPoint);
            }
            TourOccurrenceRepository.UpdateTourOccurrence(SelectedTourOccurrence);
            MessageBox.Show("Tour ended!");
        }
    }
}
