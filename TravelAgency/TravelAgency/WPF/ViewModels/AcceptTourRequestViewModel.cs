using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TravelAgency.WPF.ViewModels
{
    public class AcceptTourRequestViewModel : INotifyPropertyChanged
    {
        public TourRequest TourRequest { get; set; }
        private DateTime selectedTime;
        private bool IsTimeSelected { get; set; }
        private string selectedDate;
        private TimeSpan startTime;
        private TimeSpan endTime;
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
        public TimeSpan StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                OnPropertyChanged();
            }
        }
        public TimeSpan EndTime
        {
            get { return endTime; }
            set
            {
                endTime = value;
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
        public User ActiveGuide { get; set; }   
        public ButtonCommand<Window> ConfirmCommand { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public TourRequestService TourRequestService { get; set; }
        public AcceptTourRequestViewModel(Domain.Models.TourRequest selectedRequest, int id)
        {
            ActiveGuide = new UserService().GetById(id);
            TourRequest = selectedRequest;
            ConfirmCommand = new ButtonCommand<Window>(Confirm);
            TourRequestService = new TourRequestService();
            TourOccurrenceService = new TourOccurrenceService();
            IsTimeSelected = false;
        }
        private void Confirm(Window window)
        {
            if(SelectedDate == null)
            {
                MessageBox.Show("Select date please!");
                return;
            }
            if (!IsTimeSelected)
            {
                MessageBox.Show("Select time please!");
                return;
            }
            DateTime date = DateTime.ParseExact(SelectedDate, "G", new CultureInfo("en-US"));
            string dateTime = date.ToString("dd-MM-yyyy") + " " + SelectedTime.ToString("HH:mm");
            DateTime concreteDateTime = DateTime.ParseExact(dateTime, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));
            TourOccurrenceService.AcceptRequest(TourRequest, concreteDateTime, ActiveGuide.Id);
            TourRequestService.UpdateRequestStatus(TourRequest);
            window.Close();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
