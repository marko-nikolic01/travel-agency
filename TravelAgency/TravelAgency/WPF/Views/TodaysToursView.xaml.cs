﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Services;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for TodaysToursView.xaml
    /// </summary>
    public partial class TodaysToursView : Page, IObserver
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
        public TodaysToursView(int id)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = new UserService().GetById(id);
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
                if (tourOccurrence.CurrentState == CurrentState.Started)
                {
                    SelectedTourOccurrence = tourOccurrence;
                    StartTour();
                }
            }
        }
        public void Update()
        {
            TourOccurrences.Clear();
            foreach (TourOccurrence tourOccurrence in TourOccurrenceService.GetTodays(ActiveGuide.Id))
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
            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;

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
                if (tourOccurrence == SelectedTourOccurrence && tourOccurrence.CurrentState != CurrentState.Started)
                {
                    tourOccurrence.CurrentState = CurrentState.Started;
                    tourOccurrence.KeyPoints[0].IsChecked = true;
                    KeyPointService.UpdateKeyPoint(tourOccurrence.KeyPoints[0]);
                    if (tourOccurrence.KeyPoints.Count >= 3)
                    {
                        tourOccurrence.KeyPoints[2].CanNotBeChecked = true;
                    }
                    tourOccurrence.ActiveKeyPointId = tourOccurrence.KeyPoints[0].Id;
                }
                else if(tourOccurrence.CurrentState == CurrentState.Started)
                {
                    bool pastChecked = false;
                    foreach(KeyPoint k in tourOccurrence.KeyPoints)
                    {
                        if (!k.IsChecked)
                        {
                            if (pastChecked)
                            {
                                k.CanNotBeChecked = true;
                            }
                            pastChecked = true;
                        }
                    }
                }
                else
                {
                    tourOccurrence.ToShadow = 1;
                }
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
                if (guest != null && attendance.ResponseStatus == ResponseStatus.Accepted)
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
        private bool CheckStartConditions()
        {
            if (SelectedTourOccurrence.Guests.Count == 0)
            {
                MessageBox.Show("No guests have reserved this tour, therefore it can't be started!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            KeyPointService.UpdateKeyPoint(SelectedKeyPoint);

            bool pastChecked = false;
            foreach (var k in SelectedTourOccurrence.KeyPoints)
            {
                if (!k.IsChecked)
                {
                    if (!pastChecked)
                    {
                        k.CanNotBeChecked = false;
                    }
                    else
                    {
                        k.CanNotBeChecked = true;
                    }
                    pastChecked = true;
                }
            }
            if (SelectedTourOccurrence.KeyPoints[SelectedTourOccurrence.KeyPoints.Count - 1].Id == SelectedKeyPoint.Id)
            {
                EndTour();
            }
        }
        private void EndTour()
        {
            TourOccurrenceGrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed;

            foreach (TourOccurrence tourOccurrence in TourOccurrences)
            {
                if (tourOccurrence.CurrentState != CurrentState.Ended)
                {
                    tourOccurrence.ToShadow = 0;
                }
            }

            SelectedTourOccurrence.CurrentState = CurrentState.Ended;
            SelectedTourOccurrence.ToShadow = 1;

            Guests.Clear();
            UpdateKeyPoints();
            TourOccurrenceService.UpdateTourOccurrence(SelectedTourOccurrence.Id);
            MessageBox.Show("Tour ended!","Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void UpdateKeyPoints()
        {
            StartedTourKeyPoints.Clear();
            foreach (KeyPoint keyPoint in SelectedTourOccurrence.KeyPoints)
            {
                KeyPointService.UpdateKeyPoint(keyPoint);
            }
        }
    }
}
