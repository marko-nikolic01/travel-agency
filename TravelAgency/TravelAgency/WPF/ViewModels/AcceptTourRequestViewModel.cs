﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TravelAgency.WPF.ViewModels
{
    public class AcceptTourRequestViewModel : INotifyPropertyChanged
    {
        public TourRequest TourRequest { get; set; }
        private DateTime selectedTime;
        private bool IsTimeSelected { get; set; }
        private string selectedDate;
        public event PropertyChangedEventHandler? PropertyChanged;
        public DateTime SelectedTime
        {
            get { return selectedTime; }
            set
            {
                IsTimeSelected = true;
                selectedTime = value;
                OnPropertyChanged();
            }
        }
        public string SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<string> keyPoints;
        public ObservableCollection<string> KeyPoints
        {
            get { return keyPoints; }
            set
            {
                keyPoints = value;
                OnPropertyChanged();
            }
        }
        private string keyPoint;
        public string KeyPoint
        {
            get { return keyPoint; }
            set
            {
                keyPoint = value;
                OnPropertyChanged();
            }
        }
        private bool canRemove;
        public bool CanRemove
        {
            get { return canRemove; }
            set
            {
                canRemove = value;
                OnPropertyChanged();
            }
        }
        private string selectedKeyPoint;
        public string SelectedKeyPoint
        {
            get { return selectedKeyPoint; }
            set
            {
                selectedKeyPoint = value;
                if(selectedKeyPoint != null)
                {
                    CanRemove = true;
                }
                OnPropertyChanged();
            }
        }
        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                OnPropertyChanged();
            }
        }
        public User ActiveGuide { get; set; }   
        public ButtonCommandNoParameter ConfirmCommand { get; set; }
        public ButtonCommandNoParameter CancelCommand { get; set; }
        public ButtonCommandNoParameter AddCommand { get; set; }
        public ButtonCommandNoParameter RemoveCommand { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public TourRequestService TourRequestService { get; set; }
        public NavigationService NavigationService { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public AcceptTourRequestViewModel(Domain.Models.TourRequest selectedRequest, int id, NavigationService navigationService)
        {
            NavigationService = navigationService;
            KeyPoints = new ObservableCollection<string>();
            AddCommand = new ButtonCommandNoParameter(AddKeyPoint);
            RemoveCommand = new ButtonCommandNoParameter(RemoveKeyPoint);
            ActiveGuide = new UserService().GetById(id);
            TourRequest = selectedRequest;
            ConfirmCommand = new ButtonCommandNoParameter(Confirm);
            CancelCommand = new ButtonCommandNoParameter(Cancel);
            TourRequestService = new TourRequestService();
            TourOccurrenceService = new TourOccurrenceService();
            IsTimeSelected = false;
            EndDate = TourRequest.MaxDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            StartDate = TourRequest.MinDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            CanRemove = false;
        }
        public void AddKeyPoint()
        {
            KeyPoints.Add(KeyPoint);
            KeyPoint = "";
        }
        public void RemoveKeyPoint()
        {
            if(SelectedKeyPoint != null)
            {
                if (KeyPoints.Count > 0)
                {
                    KeyPoints.Remove(SelectedKeyPoint);
                }
            }
            CanRemove = false;
        }
        private void Cancel()
        {
            Page page = new TourRequestBookingView(ActiveGuide.Id, NavigationService);
            NavigationService.Navigate(page);
        }
        private void Confirm()
        {
            if (!CheckAllInputsPresentValid())
            {
                return;
            }
            DateTime date = DateTime.ParseExact(SelectedDate, "G", new CultureInfo("en-US"));
            string dateTime = date.ToString("dd-MM-yyyy") + " " + SelectedTime.ToString("HH:mm");
            DateTime concreteDateTime = DateTime.ParseExact(dateTime, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));

            if (!TourOccurrenceService.IsGuideFree(ActiveGuide.Id, concreteDateTime, Duration))
            {
                MessageBox.Show("You are not free in the time you entered!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int occurrenceId = TourOccurrenceService.AcceptRequest(TourRequest, concreteDateTime, ActiveGuide.Id, KeyPoints, Duration);
            TourRequestService.UpdateRequestStatus(TourRequest, concreteDateTime, ActiveGuide.Id);
            TourRequestService.SaveNotification(new RequestAcceptedNotification(concreteDateTime, ActiveGuide.Id, TourRequest.Id, false, TourRequest.GuestId, occurrenceId));
            MessageBox.Show("Tour request has been successfully booked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Page page = new TourRequestBookingView(ActiveGuide.Id, NavigationService);
            NavigationService.Navigate(page);
        }

        private bool CheckAllInputsPresentValid()
        {
            if (SelectedDate == null)
            {
                MessageBox.Show("Select date please!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!IsTimeSelected)
            {
                MessageBox.Show("Select time please!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (KeyPoints.Count < 2)
            {
                MessageBox.Show("Enter at least 2 Key Points!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (Duration <= 0)
            {
                MessageBox.Show("Enter duration!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            DateTime date = DateTime.ParseExact(SelectedDate, "G", new CultureInfo("en-US"));
            string dateTime = date.ToString("dd-MM-yyyy") + " " + SelectedTime.ToString("HH:mm");
            DateTime concreteDateTime = DateTime.ParseExact(dateTime, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));
            DateTime dT1 = TourRequest.MinDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            DateTime dT2 = TourRequest.MaxDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            if (concreteDateTime < dT1 || concreteDateTime > dT2)
            {
                MessageBox.Show("wrong date!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
