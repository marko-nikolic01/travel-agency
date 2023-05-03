using System;
using System.Collections.Generic;
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
        private string requestAcceptedString;
        private string showToursString;
        public string ShowToursString
        {
            get => showToursString;
            set { if (value != showToursString) { showToursString = value; OnPropertyChanged(); } }
        }
        public string TourPresenceString
        {
            get => presenceString;
            set { if (value != presenceString) { presenceString = value; OnPropertyChanged(); } }
        }
        public string RequestAcceptedString
        {
            get => requestAcceptedString;
            set { if (value != requestAcceptedString) { requestAcceptedString = value; OnPropertyChanged(); } }
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
        private List<RequestAcceptedNotification> RequestAcceptedNotifications;
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
            RequestAcceptedString = "";
            ShowToursString = "";
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
            foreach(RequestAcceptedNotification notification in RequestAcceptedNotifications)
            {
                RequestAcceptedString = "Your tour request has been accepted";
                ShowToursString = "CLICK HERE TO SHOW NEW TOUR";
            }
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
            if (RequestAcceptedNotifications.Count != 0)
            {
                RequestAcceptedNotifications[0].IsSeen = true;
                tourRequestService.UpdateNotification(RequestAcceptedNotifications[0]);
                RequestAcceptedString = "";
                ShowToursString = "";
                RequestAcceptedNotifications.Remove(RequestAcceptedNotifications[0]);
                tourOccurrenceAttendanceService.NotifyObservers();
            }
        }
    }
}
