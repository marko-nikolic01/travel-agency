using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Repository;
using TravelAgency.Services;
using TravelAgency.View;

namespace TravelAgency.ViewModel
{
    public class TourStatisticsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TourOccurrence> FinishedTours { get; set; }
        public Model.User ActiveGuide { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }

        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (!value.Equals("ALL TIME"))
                {
                    DisplayTour = TourOccurrenceService.GetMostVisitedByYear(ActiveGuide.Id, int.Parse(value));
                }
                else
                {
                    DisplayTour = TourOccurrenceService.GetMostVisitedAllTime(ActiveGuide.Id);
                }
                CurrentPhoto = DisplayTour.Tour.Photos[0];
                KeyPoints = "";
                foreach (KeyPoint keyPoint in DisplayTour.KeyPoints)
                {
                    KeyPoints += keyPoint.Name += ", ";
                }
                GuestsNumber = AttendanceService.GetGuestsNumberByTour(DisplayTour.Id);
                selectedYear = value;
                OnPropertyChanged();
            }
        }
        private TourOccurrence displayTour;
        public TourOccurrence DisplayTour
        {
            get { return displayTour; }
            set
            {
                displayTour = value;
                OnPropertyChanged();
            }
        }

        private Photo currentPhoto;
        public Photo CurrentPhoto
        {
            get { return currentPhoto; }
            set
            {
                currentPhoto = value;
                OnPropertyChanged();
            }
        }
        TourOccurrenceService TourOccurrenceService { get; set; }

        private string keyPoints;
        public string KeyPoints
        {
            get { return keyPoints; }
            set {
                keyPoints = value;
                OnPropertyChanged();
            }
        }

        private int guestsNumber;
        public int GuestsNumber
        {
            get { return guestsNumber; }
            set { 
                guestsNumber = value;
                OnPropertyChanged();
                }
        }
        public TourOccurrenceAttendanceService AttendanceService { get; set; }
        public StatisticsButtonCommand RightCommand { get; set; }
        public StatisticsButtonCommand LeftCommand { get; set; }
        public StatisticsButtonCommand ViewCommand { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TourStatisticsViewModel(int id)
        {
            UserRepository userRepository = new UserRepository();
            ActiveGuide = userRepository.GetById(id);
            TourOccurrenceService = new TourOccurrenceService();
            AttendanceService = new TourOccurrenceAttendanceService();
            Years = new ObservableCollection<string>();
            Years.Add("ALL TIME");
            Years.Add("2023");
            Years.Add("2022");
            Years.Add("2021");
            Years.Add("2020");
            Years.Add("2019");
            SelectedYear = Years[0];
            FinishedTours = new ObservableCollection<TourOccurrence>(TourOccurrenceService.GetFinishedOccurrencesForGuide(ActiveGuide.Id));
            RightCommand = new StatisticsButtonCommand(ShowNextPhoto);
            LeftCommand = new StatisticsButtonCommand(ShowPreviousPhoto);
            ViewCommand = new StatisticsButtonCommand(ViewDetails);
        }

        public void ViewDetails()
        {
            TourStatisticsDetailsViewModel viewModel = new TourStatisticsDetailsViewModel(SelectedTourOccurrence);
            TourStatisticsDetailsView tourStatisticsDetailsView = new TourStatisticsDetailsView();
            tourStatisticsDetailsView.DataContext = viewModel;
            tourStatisticsDetailsView.ShowDialog();
        }

        private void ShowNextPhoto()
        {
            for (int i = 0; i < displayTour.Tour.Photos.Count; i++)
            {
                if (CurrentPhoto.Id == displayTour.Tour.Photos[i].Id)
                {
                    if (i < displayTour.Tour.Photos.Count - 1)
                    {
                        CurrentPhoto = displayTour.Tour.Photos[++i];
                        return;
                    }
                    else
                    {
                        CurrentPhoto = displayTour.Tour.Photos[0];
                        return;
                    }
                }
            }
            return;
        }

        private void ShowPreviousPhoto()
        {
            for (int i = 0; i < displayTour.Tour.Photos.Count; i++)
            {
                if (CurrentPhoto.Id == displayTour.Tour.Photos[i].Id)
                {
                    if (i == 0)
                    {
                        CurrentPhoto = displayTour.Tour.Photos[displayTour.Tour.Photos.Count - 1];
                        return;
                    }
                    else
                    {
                        CurrentPhoto = displayTour.Tour.Photos[--i];
                        return;
                    }
                }
            }
            return;
        }
    }
}
