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
using TravelAgency.Services;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for GuideMain.xaml
    /// </summary>
    public partial class TodaysTours : Window, IObserver
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public ObservableCollection<User> Guests { get; set; }
        public ObservableCollection<KeyPoint> StartedTourKeyPoints { get; set; }
        public TourOccurrence? SelectedTourOccurrence { get; set; }
        public KeyPoint? SelectedKeyPoint { get; set; }
        public int ChosenKeyPointId { get; set; }
        public User? ChosenGuest { get; set; }
        public User ActiveGuide { get; set; }
        public Dictionary<User, int> GuestKeyPointIdPairs { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public TourOccurrenceAttendanceService TourOccurrenceAttendanceService { get; set; }
        public KeyPointService KeyPointService { get; set; }

        public TodaysTours(User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = user;
            KeyPointService = new KeyPointService();
            TourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            TourOccurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceService.GetTodays(ActiveGuide.Id));
            Guests = new ObservableCollection<User>();
            StartedTourKeyPoints = new ObservableCollection<KeyPoint>();
            TourOccurrenceService.Subscribe(this);
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
                }
            }
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            if (!StartButton.IsEnabled)
            {
                UpdateKeyPoints();

                TourOccurrenceService.UpdateTourOccurrence(SelectedTourOccurrence);
            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        public void Update()
        {
            TourOccurrences.Clear();
            foreach(TourOccurrence tourOccurrence in TourOccurrenceService.GetTodays(ActiveGuide.Id))
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
                    tourOccurrence.ActiveKeyPointId = tourOccurrence.KeyPoints[0].Id;
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
            foreach (TourOccurrenceAttendance attendance in TourOccurrenceAttendanceService.GetByTourOccurrenceId(SelectedTourOccurrence.Id))
            {
                User guest = Guests.ToList().Find(g => g.Id == attendance.GuestId);
                if (guest != null && attendance.KeyPointId != -1)
                {
                    Guests.Remove(guest);
                }
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
            TourOccurrenceAttendanceService.SaveOrUpdate(new TourOccurrenceAttendance(SelectedTourOccurrence.Id, ChosenKeyPointId, ChosenGuest.Id));
        }

        private void RowButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedKeyPoint.IsChecked = true;
            SelectedTourOccurrence.ActiveKeyPointId = SelectedKeyPoint.Id;
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

            TourOccurrenceService.UpdateTourOccurrence(SelectedTourOccurrence);
            MessageBox.Show("Tour ended!");
        }

        private void UpdateKeyPoints()
        {
            StartedTourKeyPoints.Clear();
            foreach (KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                KeyPointService.UpdateKeyPoint(keyPoint);
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
            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;
        }
    }
}
