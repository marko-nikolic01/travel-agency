using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    
    class Guest2NotificationsViewModel: INotifyPropertyChanged
    {
        private string presenceString;
        private string confirmationString;
        private string rejectionString;
        public string TourPresenceString
        {
            get => presenceString;
            set { if (value != presenceString) { presenceString = value; OnPropertyChanged(); } }
        }
        public string ConfirmationString
        {
            get => confirmationString;
            set { if (value != confirmationString) { confirmationString = value; OnPropertyChanged(); } }
        }
        public string RejectionString
        {
            get => rejectionString;
            set { if (value != rejectionString) { rejectionString = value; OnPropertyChanged(); } }
        }
        public int currentGuestId;
        private TourOccurrenceAttendance attendance;
        TourOccurrenceAttendanceService tourOccurrenceAttendanceService;
        TourRequestService tourRequestService;
        public List<RequestAcceptedNotification> RequestAcceptedNotifications { get; set; }
        public RequestAcceptedNotification Notification { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Guest2NotificationsViewModel(int id)
        {
            TourPresenceString = "";
            ConfirmationString = "";
            RejectionString = "";
            currentGuestId = id;
            tourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            tourRequestService = new TourRequestService();
            RequestAcceptedNotifications = new List<RequestAcceptedNotification>();
            AllertIfSelectеd();
            IsSomeTourAccepted();
        }

        private void IsSomeTourAccepted()
        {
            TourRequestService requestService = new TourRequestService();
            RequestAcceptedNotifications = requestService.GetNewAcceptedRequests(currentGuestId);
        }
        private void AllertIfSelectеd()
        {
            if( (attendance = tourOccurrenceAttendanceService.GetAttendance(currentGuestId)) != null)
            {
                TourPresenceString = "You have been selected as present on tour, do you confirm?";
                ConfirmationString = "YES";
                RejectionString = "NO";

            }
        }
        public void ConfirmPresence()
        {
            if (attendance != null)
            {
                tourOccurrenceAttendanceService.SaveAnswer(true, attendance);
                TourPresenceString = "";
                ConfirmationString = "";
                RejectionString = "";
                tourOccurrenceAttendanceService.NotifyObservers();
            }
        }
        public void RejectPresence()
        {
            if (attendance != null)
            {
                tourOccurrenceAttendanceService.SaveAnswer(false, attendance);
                TourPresenceString = "";
                ConfirmationString = "";
                RejectionString = "";
                tourOccurrenceAttendanceService.NotifyObservers();
            }
        }
        public void RemoveNotification()
        {
            Notification.IsSeen = true;
            tourRequestService.UpdateNotification(Notification);
            RequestAcceptedNotifications.Remove(Notification);
            tourOccurrenceAttendanceService.NotifyObservers();
        }
    }
}
