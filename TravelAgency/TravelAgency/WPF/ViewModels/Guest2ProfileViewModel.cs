using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using TravelAgency.Commands;
using TravelAgency.Domain.DTOs;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

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
        public ButtonCommandNoParameter ChangeUsernameCommand { get; set; }
        public ButtonCommandNoParameter ChangePasswordCommand { get; set; }
        public ButtonCommandNoParameter GenerateReportCommand { get; set; }
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
        NavigationService NavigationService;
        public Guest2ProfileViewModel(int guestId, NavigationService navService) 
        {
            NavigationService = navService;
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
            ChangeUsernameCommand = new ButtonCommandNoParameter(ChangeUsername);
            ChangePasswordCommand = new ButtonCommandNoParameter(ChangePassword);
            GenerateReportCommand = new ButtonCommandNoParameter(SaveReport);
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
            List<TourOccurrenceAttendanceDTO> list = service.GetAttendancesForPeriod(GuestId, StartDate, EndDate);
            var attendances = from attendance in list
                               orderby attendance.TourDateTime
                               select attendance;
            tourOccurrenceAttendanceDTOs = attendances.ToList();
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
        public void SaveReport()
        {
            if(StartDate > EndDate)
            {

                System.Windows.MessageBox.Show("Start date must be lower than end date", "Tour attendance report", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PrepareData();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            XFont titleFont;
            XFont subTitleFont;
            XFont regularFont;
            titleFont = new XFont("Times New Roman", 24, XFontStyle.Regular);
            regularFont = new XFont("Times New Roman", 13, XFontStyle.Regular);
            subTitleFont = new XFont("Times New Roman", 15, XFontStyle.Bold);
            PdfDocument PDFReport = new PdfDocument();
            PdfPage page = PDFReport.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            gfx.DrawString("Tour attendance report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 20), XStringFormats.Center);
            gfx.DrawString("Report creator:          Travel agency", regularFont, XBrushes.Black, new XPoint(20, 70), XStringFormats.TopLeft);
            gfx.DrawString("Receiving user:          "+Username, regularFont, XBrushes.Black, new XPoint(20, 90), XStringFormats.TopLeft);
            gfx.DrawString("Report creation date: "+ DateTime.Now.ToString("dd/MM/yyyy"), regularFont, XBrushes.Black, new XPoint(20, 110), XStringFormats.TopLeft);
            gfx.DrawString("Report on presence on the tours " +
                "from " + StartDate.ToString("dd/MM/yyyy") + "  to " + EndDate.ToString("dd/MM/yyyy"), subTitleFont, XBrushes.Black, new XPoint(70, 160), XStringFormats.TopLeft);
            gfx.DrawString("Tour name                       Date and time                          Status                               Arrived at ", 
                regularFont, XBrushes.Black, new XPoint(30, 195), XStringFormats.TopLeft);
            gfx.DrawString("----------------------------------------------------------------------------------------------------------------", 
                regularFont, XBrushes.Black, new XPoint(30, 210), XStringFormats.TopLeft);
            int y = 235;
            foreach (TourOccurrenceAttendanceDTO attendanceDTO in tourOccurrenceAttendanceDTOs)
            {
                gfx.DrawString(attendanceDTO.TourName, regularFont, XBrushes.Black, new XPoint(30, y), XStringFormats.TopLeft);
                gfx.DrawString(attendanceDTO.TourDateTime.ToString(), regularFont, XBrushes.Black, new XPoint(140, y), XStringFormats.TopLeft);
                gfx.DrawString(attendanceDTO.Status, regularFont, XBrushes.Black, new XPoint(290, y), XStringFormats.TopLeft);
                gfx.DrawString(attendanceDTO.ArrivalKeyPoint, regularFont, XBrushes.Black, new XPoint(450, y), XStringFormats.TopLeft);
                gfx.DrawString("----------------------------------------------------------------------------------------------------------------",
                    regularFont, XBrushes.Black, new XPoint(30, y+12), XStringFormats.TopLeft);
                y += 35;
            }
            gfx.DrawString("Total reservations: " + tourOccurrenceAttendanceDTOs.Count, regularFont, XBrushes.Black, new XPoint(20, y+10), XStringFormats.TopLeft);
            int confirmedPresences = getConfirmedPresences(tourOccurrenceAttendanceDTOs);
            gfx.DrawString("Total confirmed presences: " + confirmedPresences, regularFont, XBrushes.Black, new XPoint(20, y + 25), XStringFormats.TopLeft);
            PDFReport.Save(@"../../../ReportsPDF/Guest2Report.pdf");
            Guest2ReportView reportView = new Guest2ReportView();
            this.NavigationService.Navigate(reportView);
        }
        private int getConfirmedPresences(List<TourOccurrenceAttendanceDTO> attendanceDTOs)
        {
            int cnt = 0;
            foreach(var attendanceDTO in attendanceDTOs) 
            {
                if (attendanceDTO.Status == "Was present on the tour")
                    cnt++;
            }
            return cnt;
        }
    }
}
