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
    public partial class GuideMain : Window, IObserver
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourRepository TourRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public KeyPointRepository KeyPointRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public static TourReservationRepository? TourReservationRepository { get; set; } 
        public TourOccurrenceAttendanceRepository TourOccurrenceAttendanceRepository { get; set; }
        public ObservableCollection<User> Guests { get; set; }
        public ObservableCollection<KeyPoint> StartedTourKeyPoints { get; set; }
        public TourOccurrence? SelectedTourOccurrence { get; set; }
        public KeyPoint? SelectedKeyPoint { get; set; }
        public int ChosenKeyPointId { get; set; }
        public User? ChosenGuest { get; set; }
        public User ActiveGuide { get; set; }
        public Dictionary<User, int> GuestKeyPointIdPairs { get; set; }

        public GuideMain(User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = user;
            TourRepository = new TourRepository();
            LocationRepository = new LocationRepository();
            PhotoRepository = new PhotoRepository();
            TourOccurrenceRepository = new TourOccurrenceRepository();
            KeyPointRepository = new KeyPointRepository();
            TourReservationRepository = new TourReservationRepository();
            UserRepository = new UserRepository();
            TourOccurrenceAttendanceRepository = new TourOccurrenceAttendanceRepository();
            LinkData();
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceRepository.GetTodaysTourOccurrences(ActiveGuide));
            Guests = new ObservableCollection<User>();
            StartedTourKeyPoints = new ObservableCollection<KeyPoint>();
            TourOccurrenceRepository.Subscribe(this);
            GuestKeyPointIdPairs = new Dictionary<User, int>();

            ShadowFinishedTourOccurrences();
        }

        private void ShadowFinishedTourOccurrences()
        {
            foreach (TourOccurrence tourOccurrence in TourOccurrences)
            {
                if (tourOccurrence.CurrentState == CurrentState.Ended)
                {
                    tourOccurrence.ToShadow = 1;
                    tourOccurrence.ToDisplay = 0;
                }
                if(tourOccurrence.CurrentState == CurrentState.Started)
                {
                    SelectedTourOccurrence = tourOccurrence;
                    StartTour();
                    RemoveNotifiedGuests();
                }
            }
        }

        private void RemoveNotifiedGuests()
        { 
            List<User> notifiedGuests = new List<User>();
            foreach(TourOccurrenceAttendance attendance in TourOccurrenceAttendanceRepository.GetByTourOccurrenceId(SelectedTourOccurrence.Id)){
                User guest = UserRepository.GetUsers().Find(g => g.Id == attendance.GuestId);
                if(guest != null && attendance.KeyPointId != -1)
                {
                    notifiedGuests.Add(guest);
                }
            }
            foreach(User guest in notifiedGuests)
            {
                Guests.Remove(guest);
            }
        }

        private void LinkData()
        {
            LinkTourLocation();
            LinkTourPhotos();
            LinkTourOccurrences();
            LinkKeyPoints();
            LinkTourGuests();
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
                Location location = LocationRepository.GetLocations().Find(l => l.Id == tour.LocationId);
                if(location != null)
                {
                    tour.Location = location;
                }
            }
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            if (!StartButton.IsEnabled)
            {
                UpdateKeyPoints();

                TourOccurrenceRepository.UpdateTourOccurrence(SelectedTourOccurrence);
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void NewTour_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(TourRepository, LocationRepository, PhotoRepository, TourOccurrenceRepository, KeyPointRepository, ActiveGuide);
            createTour.Show();
        }

        public void Update()
        {
            TourOccurrences.Clear();
            foreach(TourOccurrence tourOccurrence in TourOccurrenceRepository.GetTodaysTourOccurrences(ActiveGuide))
            {
                TourOccurrences.Add(tourOccurrence);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckStartConditions())
            {
                return;
            }

            StartTour();
        }

        private void StartTour()
        {
            ModifyViewStart();

            StartTourShadowNotActive();

            LoadKeyPoints();

            LoadGuests();

            ComboColumn.ItemsSource = StartedTourKeyPoints;
            ChosenKeyPointId = -1;
        }

        private void StartTourShadowNotActive()
        {
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
        }

        private void LoadGuests()
        {
            Guests.Clear();
            foreach (User guest in SelectedTourOccurrence.Guests)
            {
                Guests.Add(guest);
                GuestKeyPointIdPairs[guest] = -1;
            }
        }

        private void LoadKeyPoints()
        {
            StartedTourKeyPoints.Clear();
            StartedTourKeyPoints.Add(new KeyPoint(-1, "NOT PRESENT", SelectedTourOccurrence.Id));
            foreach (KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                StartedTourKeyPoints.Add(keyPoint);
            }
        }

        private void ModifyViewStart()
        {
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            NewTourButton.IsEnabled = false;

            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;
        }

        private bool CheckStartConditions()
        {
            if (SelectedTourOccurrence == null)
            {
                MessageBox.Show("You have to select the tour you would like to start first!");
                return false;
            }
            else if (SelectedTourOccurrence.Guests.Count == 0)
            {
                MessageBox.Show("No guests have reserved this tour, therefore it can't be started!");
                return false;
            }
            return true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            EndTour();
        }

        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GuestKeyPointIdPairs[ChosenGuest] = ChosenKeyPointId;
            TourOccurrenceAttendanceRepository.SaveOrUpdate(new TourOccurrenceAttendance(SelectedTourOccurrence.Id, ChosenKeyPointId, ChosenGuest.Id));
        }

        private void RowButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedKeyPoint.IsChecked = true;
            if (SelectedTourOccurrence.KeyPoints[SelectedTourOccurrence.KeyPoints.Count - 1].Id == SelectedKeyPoint.Id)
            {
                EndTour();
            }
        }

        private void EndTour()
        {
            ModifyViewEnd();

            EndTourShadowNotActive();

            Guests.Clear();

            UpdateKeyPoints();

            TourOccurrenceRepository.UpdateTourOccurrence(SelectedTourOccurrence);
            MessageBox.Show("Tour ended!");
        }

        private void UpdateKeyPoints()
        {
            StartedTourKeyPoints.Clear();
            foreach (KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                KeyPointRepository.UpdateKeyPoint(keyPoint);
            }
        }

        private void EndTourShadowNotActive()
        {
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
        }

        private void ModifyViewEnd()
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            NewTourButton.IsEnabled = true;
            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }
    }
}
