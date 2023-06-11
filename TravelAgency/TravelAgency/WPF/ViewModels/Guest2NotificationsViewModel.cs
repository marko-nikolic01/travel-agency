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
        private string voucherWonString;
        private string showVoucherString;
        public string VoucherWonString
        {
            get => voucherWonString;
            set { if (value != voucherWonString) { voucherWonString = value; OnPropertyChanged(); } }
        }
        public string ShowVoucherString
        {
            get => showVoucherString;
            set { if (value != showVoucherString) { showVoucherString = value; OnPropertyChanged(); } }
        }
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
        CreatedTourFromStatisticService service;
        public List<RequestAcceptedNotification> RequestAcceptedNotifications { get; set; }
        public List<NewTourNotification> NewTourNotifications { get; set; }
        public RequestAcceptedNotification RequestNotification { get; set; }
        public NewTourNotification NewTourNotification { get; set; }
        public WonVoucherNotification VoucherNotification { get; set; }
        public TourOccurrence TourOccurrence;
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
            VoucherWonString = "";
            ShowVoucherString = "";
            currentGuestId = id;
            tourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            tourRequestService = new TourRequestService();
            service = new CreatedTourFromStatisticService();
            RequestAcceptedNotifications = new List<RequestAcceptedNotification>();
            NewTourNotifications = new List<NewTourNotification>();
            AllertIfSelectеd();
            IsSomeTourAccepted();
            GetNewToursFromStatistic();
            CheckIfVoucherWon();
        }
        private void GetNewToursFromStatistic()
        {
            new TourOccurrenceService();
            NewTourNotifications = service.GetNewTourNotifications(currentGuestId);
            foreach(var notification in NewTourNotifications) 
            {
                if (notification.IsForLanguage)
                    notification.NotificationText = "New tour has been created in " + notification.Tour.Language + " language";
                else
                    notification.NotificationText = "New tour has been created in " + notification.Tour.Location.Country + ", " + notification.Tour.Location.City;
            }
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
        public void RemoveRequestNotification()
        {
            TourOccurrenceService occurrenceService = new TourOccurrenceService();
            if (RequestNotification != null)
            {
                TourOccurrence = occurrenceService.GetById(RequestNotification.OccurrenceId);
                RequestNotification.IsSeen = true;
                tourRequestService.UpdateNotification(RequestNotification);
                RequestAcceptedNotifications.Remove(RequestNotification);
            }
            tourOccurrenceAttendanceService.NotifyObservers();
        }
        public void RemoveTourNotification()
        {
            TourOccurrenceService occurrenceService = new TourOccurrenceService();
            if (NewTourNotification != null)
            {
                TourOccurrence = occurrenceService.GetByTourId(NewTourNotification.TourId);
                NewTourNotification.Seen = true;
                service.UpdateNotification(NewTourNotification);
                NewTourNotifications.Remove(NewTourNotification);
            }
            tourOccurrenceAttendanceService.NotifyObservers();
        }
        private void CheckIfVoucherWon()
        {
            VoucherService voucherService = new VoucherService();
            VoucherNotification = voucherService.GetVoucherNotification(currentGuestId);
            if(VoucherNotification != null) 
            {
                if(!VoucherNotification.Seen)
                {
                    VoucherWonString = "You have won new voucher";
                    ShowVoucherString = "CLICK HERE TO SEE VOUCHERS";
                }
            }
        }
        public void RemoveVoucherNotification()
        {
            VoucherNotification.Seen = true;
            VoucherService voucherService = new VoucherService();
            voucherService.UpdateNotification(VoucherNotification);
            tourOccurrenceAttendanceService.NotifyObservers();
        }
    }
}
