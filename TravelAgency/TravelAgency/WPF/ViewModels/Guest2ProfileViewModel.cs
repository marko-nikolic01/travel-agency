using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Commands;
using TravelAgency.Domain.DTOs;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest2ProfileViewModel : INotifyPropertyChanged
    {
        private string changeUsernameVisibility;
        private string changePasswordVisibility;
        private string newUsername;
        private string username;
        public string ChangeUsernameVisibility
        {
            get { return changeUsernameVisibility; }
            set { changeUsernameVisibility = value; OnPropertyChanged(); }
        }
        public string ChangePasswordVisibility
        {
            get { return changePasswordVisibility; }
            set { changePasswordVisibility = value; OnPropertyChanged(); }
        }
        public string NewUsername
        {
            get { return newUsername; }
            set { newUsername = value; OnPropertyChanged(); }
        }
        public ButtonCommandNoParameter ConfirmUsernameCommand { get; set; }
        public ButtonCommandNoParameter ExitCommand { get; set; }
        public int GuestId { get; set; }    
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }
        public string ReservationsNumber { get; set; }    
        public string AttendancesNumber { get; set; }    
        public string RequestsNumber { get; set; }    
        public string RatingsNumber { get; set; }    
        public string VouchersNumber { get; set; }    
        public DateTime StartDate { get; set; }    
        public DateTime EndDate { get; set; }
        public GuestAttendanceForPeriodService service;
        public UserService userService;
        public List<TourOccurrenceAttendanceDTO> tourOccurrenceAttendanceDTOs;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Guest2ProfileViewModel(int guestId) 
        {
            ChangeUsernameVisibility = "Hidden";
            ChangePasswordVisibility = "Hidden";
            new TourOccurrenceService();
            service = new GuestAttendanceForPeriodService();
            userService = new UserService();
            StartDate = new DateTime(2022, 1, 1);
            EndDate = new DateTime(2023, 1, 1);
            GuestId = guestId;
            LoadUserData();
            ConfirmUsernameCommand = new ButtonCommandNoParameter(ConfirmNewUsername);
            ExitCommand = new ButtonCommandNoParameter(Exit);
        }
        private void LoadUserData()
        {
            TourReservationService reservationService = new TourReservationService();
            TourOccurrenceAttendanceService tourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            SpecialTourRequestService requestService = new SpecialTourRequestService();
            TourRatingService ratingService = new TourRatingService();
            VoucherService voucherService = new VoucherService();
            Username = userService.GetById(GuestId).Username;
            ReservationsNumber = reservationService.GetForGuest(GuestId).Count.ToString();
            AttendancesNumber = tourOccurrenceAttendanceService.GetNumberOfGuestsAttendances(GuestId).ToString();
            RequestsNumber = requestService.GetNumberOfAllRequests(GuestId).ToString();
            RatingsNumber = ratingService.GetNumberForGuest(GuestId).ToString();
            VouchersNumber = voucherService.GetGuestVouchers(GuestId).Count.ToString();
        }
        public void PrepareData()
        {
            tourOccurrenceAttendanceDTOs = service.GetAttendancesForPeriod(GuestId, StartDate, EndDate);
        }
        public void ChangeUsername()
        {
            ChangePasswordVisibility = "Hidden";
            ChangeUsernameVisibility = "Visible";
        }
        public void ChangePassword()
        {
            ChangeUsernameVisibility = "Hidden";
            ChangePasswordVisibility = "Visible";
        }
        private void ConfirmNewUsername()
        {
            if(NewUsername != null || NewUsername.Length != 0)
            {
                userService.UpdateNewUsername(GuestId, NewUsername);
                Username = NewUsername;
                ChangeUsernameVisibility = "Hidden";
            }
        }
        public void ConfirmNewPassword(string oldPassword, string newPassword)
        {
            if (CanChangePassword(oldPassword, newPassword))
            {
                userService.UpdateNewPassword(GuestId, newPassword);
                ChangePasswordVisibility = "Hidden";
            }
        }
        private bool CanChangePassword(string oldPassword, string newPassword)
        {
            bool oldPasswordCorrect = userService.CheckPassword(GuestId, oldPassword);
            return newPassword != null && newPassword.Length > 0 && oldPasswordCorrect;
        }
        private void Exit()
        {
            ChangeUsernameVisibility = "Hidden";
            ChangePasswordVisibility = "Hidden";
        }
    }
}
