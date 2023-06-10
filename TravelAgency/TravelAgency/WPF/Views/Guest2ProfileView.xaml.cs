using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Text;
using System.Windows.Forms;
using TravelAgency.Domain.DTOs;
using System.Windows;
using System.Windows.Navigation;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest2ProfileView.xaml
    /// </summary>
    public partial class Guest2ProfileView : Page
    {
        Guest2ProfileViewModel viewModel;
        public Guest2ProfileView(int id)
        {
            InitializeComponent();
            viewModel = new Guest2ProfileViewModel(id);
            DataContext = viewModel;
        }
        private void GenerateReport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(viewModel.StartDate > viewModel.EndDate)
            {

                System.Windows.MessageBox.Show("Start date must be lower than end date", "Tour attendance report", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            viewModel.PrepareData();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            XFont titleFont;
            XFont subTitleFont;
            XFont regularFont;
            titleFont = new XFont("Times New Roman", 24, XFontStyle.Regular);
            regularFont = new XFont("Times New Roman", 13, XFontStyle.Regular);
            subTitleFont = new XFont("Times New Roman", 13, XFontStyle.Bold);
            PdfDocument PDFReport = new PdfDocument();
            PdfPage page = PDFReport.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);
            gfx.DrawString("Tour attendance report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 20), XStringFormats.Center);
            gfx.DrawString("User: "+viewModel.Username, regularFont, XBrushes.Black, new XPoint(20, 70), XStringFormats.TopLeft);
            gfx.DrawString("Report creation date: "+ DateTime.Now.ToString("dd/MM/yyyy"), regularFont, XBrushes.Black, new XPoint(20, 90), XStringFormats.TopLeft);
            gfx.DrawString("Report on presence on the tours " +
                "from " + viewModel.StartDate.ToString("dd/MM/yyyy") + "  to " + viewModel.EndDate.ToString("dd/MM/yyyy"), subTitleFont, XBrushes.Black, new XPoint(100, 130), XStringFormats.TopLeft);
            gfx.DrawString("Tour name                       Date and time                          Status                               Arrived at ", 
                regularFont, XBrushes.Black, new XPoint(30, 160), XStringFormats.TopLeft);
            gfx.DrawString("----------------------------------------------------------------------------------------------------------------", 
                regularFont, XBrushes.Black, new XPoint(30, 175), XStringFormats.TopLeft);
            int y = 200;
            foreach (TourOccurrenceAttendanceDTO attendanceDTO in viewModel.tourOccurrenceAttendanceDTOs)
            {
                gfx.DrawString(attendanceDTO.TourName, regularFont, XBrushes.Black, new XPoint(30, y), XStringFormats.TopLeft);
                gfx.DrawString(attendanceDTO.TourDateTime.ToString(), regularFont, XBrushes.Black, new XPoint(140, y), XStringFormats.TopLeft);
                gfx.DrawString(attendanceDTO.Status, regularFont, XBrushes.Black, new XPoint(290, y), XStringFormats.TopLeft);
                gfx.DrawString(attendanceDTO.ArrivalKeyPoint, regularFont, XBrushes.Black, new XPoint(450, y), XStringFormats.TopLeft);
                gfx.DrawString("----------------------------------------------------------------------------------------------------------------",
                    regularFont, XBrushes.Black, new XPoint(30, y+12), XStringFormats.TopLeft);
                y += 35;
            }
            PDFReport.Save(@"../../../ReportsPDF/Guest2Report.pdf");
            Guest2ReportView reportView = new Guest2ReportView();
            this.NavigationService.Navigate(reportView);
        }
        private void ChangeUsername_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.ChangeUsername();
        }
        private void ChangePassword_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.ChangePassword();
        }
        private void ConfirmPassword(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.ConfirmNewPassword(oldPwdBox.Password, newPwdBox.Password);
        }
    }
}
