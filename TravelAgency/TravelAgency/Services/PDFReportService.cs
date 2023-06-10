using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TravelAgency.Domain.Models;

namespace TravelAgency.Services
{
    public class PDFReportService
    {
        private XFont regularFont;
        private XFont regularGuideFont;
        private XFont boldFont;
        private XFont boldGuideFont;
        private XFont titleFont;
        private XStringFormat format;

        public PDFReportService()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            regularFont = new XFont("Times New Roman", 13, XFontStyle.Regular);
            boldFont = new XFont("Times New Roman", 13, XFontStyle.Bold);
            boldGuideFont = new XFont("Times New Roman", 16, XFontStyle.Bold);
            titleFont = new XFont("Times New Roman", 24, XFontStyle.Regular);
            format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
            regularGuideFont = new XFont("Times New Roman", 16, XFontStyle.Regular);
        }

        private void SaveReport(PdfDocument report)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All files (*.*) | *.*";
            sfd.InitialDirectory = "c:\\";
            sfd.FileName = "report.pdf";
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filename = sfd.FileName;
                report.Save(filename);
                new Process { StartInfo = new ProcessStartInfo(filename) { UseShellExecute = true } }.Start();
            }
        }

        public void WriteAccommodationReservationReport(User guest, List<AccommodationReservation> reservations, DateTime startDate, DateTime endDate)
        {
            PdfDocument PDFReport = new PdfDocument();
            PdfPage page = PDFReport.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            gfx.DrawImage(XImage.FromFile("../../../Resources/Images/International-Travel_43678.ico"), 10, 5, 70, 70);
            gfx.DrawString("Izveštaj o zakazanim rezervacijama", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 20), XStringFormats.Center);
            gfx.DrawString("Datum izdavanja izveštaja: " + DateOnly.FromDateTime(DateTime.Now).ToShortDateString(), regularFont, XBrushes.Black, new XPoint(10, 70), XStringFormats.TopLeft);
            gfx.DrawString("Izdavač izveštaja: Turistička agencija", regularFont, XBrushes.Black, new XPoint(10, 90), XStringFormats.TopLeft);
            gfx.DrawString("Primalac izveštaja (korisničko ime): " + guest.Username, regularFont, XBrushes.Black, new XPoint(10, 110), XStringFormats.TopLeft);
            tf.DrawString("U vremenskom periodu od " + DateOnly.FromDateTime(startDate).ToShortDateString() + " do " + DateOnly.FromDateTime(endDate).ToShortDateString() +
                ", korisnik " + guest.Username + " ima " + reservations.Count().ToString() + " zabeleženih zakazanih rezervacija. Detalji o rezervacijama su prikazani u tabeli. Rezervacije su sortirane po početnom datumu.", regularFont, XBrushes.Black, new XRect(10, 160, page.Width - 10, 40), format);
            gfx.DrawString("Termin", boldFont, XBrushes.Black, new XRect(10, 230, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Smeštaj", boldFont, XBrushes.Black, new XRect(180, 230, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Lokacija", boldFont, XBrushes.Black, new XRect(330, 230, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Broj gostiju", boldFont, XBrushes.Black, new XRect(500, 230, 50, 20), XStringFormats.TopLeft);
            int index = 1;
            foreach (AccommodationReservation reservation in reservations)
            {
                gfx.DrawString(reservation.DateSpan.StartDate.ToShortDateString() + " - " + reservation.DateSpan.EndDate.ToShortDateString(), regularFont, XBrushes.Black, new XRect(10, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(reservation.Accommodation.Name, regularFont, XBrushes.Black, new XRect(180, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(reservation.Accommodation.Location.City + ", " + reservation.Accommodation.Location.Country, regularFont, XBrushes.Black, new XRect(330, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(reservation.NumberOfGuests.ToString(), regularFont, XBrushes.Black, new XRect(500, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                index++;
            }

            SaveReport(PDFReport);
        }

        public void WriteCanceledAccommodationReservationReport(User guest, List<AccommodationReservation> reservations, DateTime startDate, DateTime endDate)
        {

            PdfDocument PDFReport = new PdfDocument();
            PdfPage page = PDFReport.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            gfx.DrawImage(XImage.FromFile("../../../Resources/Images/International-Travel_43678.ico"), 10, 5, 70, 70);
            gfx.DrawString("Izveštaj o otkazanim rezervacijama", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 20), XStringFormats.Center);
            gfx.DrawString("Datum izdavanja izveštaja: " + DateOnly.FromDateTime(DateTime.Now).ToShortDateString(), regularFont, XBrushes.Black, new XPoint(10, 70), XStringFormats.TopLeft);
            gfx.DrawString("Izdavač izveštaja: Turistička agencija", regularFont, XBrushes.Black, new XPoint(10, 90), XStringFormats.TopLeft);
            gfx.DrawString("Primalac izveštaja (korisničko ime): " + guest.Username, regularFont, XBrushes.Black, new XPoint(10, 110), XStringFormats.TopLeft);
            tf.DrawString("U vremenskom periodu od " + DateOnly.FromDateTime(startDate).ToShortDateString() + " do " + DateOnly.FromDateTime(endDate).ToShortDateString() +
                ", korisnik " + guest.Username + " ima " + reservations.Count().ToString() + " zabeleženih otkazanih rezervacija. Detalji o rezervacijama su prikazani u tabeli. Rezervacije su sortirane po početnom datumu.", regularFont, XBrushes.Black, new XRect(10, 160, page.Width - 10, 40), format);
            gfx.DrawString("Termin", boldFont, XBrushes.Black, new XRect(10, 230, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Smeštaj", boldFont, XBrushes.Black, new XRect(180, 230, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Lokacija", boldFont, XBrushes.Black, new XRect(330, 230, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Broj gostiju", boldFont, XBrushes.Black, new XRect(500, 230, 50, 20), XStringFormats.TopLeft);
            int index = 1;
            foreach (AccommodationReservation reservation in reservations)
            {
                gfx.DrawString(reservation.DateSpan.StartDate.ToShortDateString() + " - " + reservation.DateSpan.EndDate.ToShortDateString(), regularFont, XBrushes.Black, new XRect(10, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(reservation.Accommodation.Name, regularFont, XBrushes.Black, new XRect(180, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(reservation.Accommodation.Location.City + ", " + reservation.Accommodation.Location.Country, regularFont, XBrushes.Black, new XRect(330, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(reservation.NumberOfGuests.ToString(), regularFont, XBrushes.Black, new XRect(500, 230 + (index * 20), 50, 20), XStringFormats.TopLeft);
                index++;
            }

            SaveReport(PDFReport);
        }
        public string WriteTourStatisticsReport(User guide, TourOccurrence selectedTourOccurrence, int guestsUnder18, int guests18to50, int guestsAbove50, double guestsUsedVoucher, double guestsNotUsedVoucher)
        {
            PdfDocument PDFReport = new PdfDocument();
            PdfPage page = PDFReport.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            gfx.DrawImage(XImage.FromFile("../../../Resources/Images/logotip.ico"), 20, 5, 70, 70);
            gfx.DrawString("Tour statistics report", titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 20), XStringFormats.Center);
            gfx.DrawString("Date: " + DateOnly.FromDateTime(DateTime.Now).ToShortDateString(), regularGuideFont, XBrushes.Black, new XPoint(20, 700), XStringFormats.TopLeft);
            gfx.DrawString("Guides name: " + guide.Name, regularGuideFont, XBrushes.Black, new XPoint(20, 90), XStringFormats.TopLeft);
            gfx.DrawString("Tour name: " + selectedTourOccurrence.Tour.Name, regularGuideFont, XBrushes.Black, new XPoint(20, 115), XStringFormats.TopLeft);
            gfx.DrawString("Tour date and time: " + selectedTourOccurrence.DateTime.ToString(), regularGuideFont, XBrushes.Black, new XPoint(20, 140), XStringFormats.TopLeft);
            gfx.DrawString("Language: " + selectedTourOccurrence.Tour.Language, regularGuideFont, XBrushes.Black, new XPoint(20, 165), XStringFormats.TopLeft);
            gfx.DrawString("Duration: " + selectedTourOccurrence.Tour.Duration, regularGuideFont, XBrushes.Black, new XPoint(20, 190), XStringFormats.TopLeft);
            gfx.DrawString("Location: " + selectedTourOccurrence.Tour.Location.City + ", " + selectedTourOccurrence.Tour.Location.Country, regularGuideFont, XBrushes.Black, new XPoint(20, 240), XStringFormats.TopLeft);
            gfx.DrawString("Description: " + selectedTourOccurrence.Tour.Description, regularGuideFont, XBrushes.Black, new XPoint(20, 215), XStringFormats.TopLeft);


            gfx.DrawString("|   Guests under 18   |", regularGuideFont, XBrushes.Black, new XPoint(50, 310), XStringFormats.TopLeft);
            gfx.DrawString("Guests from 18 to 50   |", regularGuideFont, XBrushes.Black, new XPoint(200, 310), XStringFormats.TopLeft);
            gfx.DrawString("Guests above 50   |", regularGuideFont, XBrushes.Black, new XPoint(360, 310), XStringFormats.TopLeft);
            gfx.DrawString("|", regularGuideFont, XBrushes.Black, new XPoint(50, 330), XStringFormats.TopLeft);
            gfx.DrawString("|", regularGuideFont, XBrushes.Black, new XPoint(183, 330), XStringFormats.TopLeft);
            gfx.DrawString("|", regularGuideFont, XBrushes.Black, new XPoint(345, 330), XStringFormats.TopLeft);
            gfx.DrawString("|", regularGuideFont, XBrushes.Black, new XPoint(480, 330), XStringFormats.TopLeft);
            gfx.DrawString(guestsUnder18.ToString(), boldGuideFont, XBrushes.Black, new XPoint(110, 330), XStringFormats.TopLeft);
            gfx.DrawString(guests18to50.ToString(), boldGuideFont, XBrushes.Black, new XPoint(260, 330), XStringFormats.TopLeft);
            gfx.DrawString(guestsAbove50.ToString(), boldGuideFont, XBrushes.Black, new XPoint(415, 330), XStringFormats.TopLeft);


            gfx.DrawString("Vouchers have been used by " + guestsUsedVoucher.ToString("0.00%") + " of the guests, and the rest of " + guestsNotUsedVoucher.ToString("0.00%") + " have not", regularGuideFont, XBrushes.Black, new XPoint(20, 390), XStringFormats.TopLeft);
            gfx.DrawString("used vouchers while paying for this tour.", regularGuideFont, XBrushes.Black, new XPoint(20, 410), XStringFormats.TopLeft);


            string link = "../../../ReportsPDF/GuideReport" + guide.Id.ToString() + selectedTourOccurrence.Id.ToString() + ".pdf";
            PDFReport.Save(link);
            return link;
        }

        public void GenerateAccommodationRenovationsReport(User owner, DateTime startDate, DateTime endDate)
        {
            GenerateAccommodationRenovationsReport(owner, DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
        }

        public void GenerateAccommodationRenovationsReport(User owner, DateOnly startDate, DateOnly endDate)
        {
            RenovationService renovationService = new RenovationService();

            var report = renovationService.GetRenovationsReport(owner, startDate, endDate);

            int margin = 30;

            PdfDocument PDFReport = new PdfDocument();
            PdfPage page = PDFReport.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            gfx.DrawImage(XImage.FromFile("../../../Resources/Images/International-Travel_43678.ico"), 20, 10, 70, 70);
            gfx.DrawString(report.Header, titleFont, XBrushes.Black, new XRect(0, 20, page.Width, 20), XStringFormats.Center);
            gfx.DrawString("Report date: " + report.ReportDate.ToShortDateString(), regularFont, XBrushes.Black, new XPoint(margin, 90), XStringFormats.TopLeft);
            gfx.DrawString("Owner: " + report.Owner.Name, regularFont, XBrushes.Black, new XPoint(margin, 110), XStringFormats.TopLeft);
            gfx.DrawString("Datespan: " + report.ReportDateSpan.StartDate.ToShortDateString() + " to " + report.ReportDateSpan.EndDate.ToShortDateString(), regularFont, XBrushes.Black, new XPoint(margin, 130), XStringFormats.TopLeft);
            gfx.DrawString("Renovations: " + report.RenovationsCount, regularFont, XBrushes.Black, new XPoint(margin, 150), XStringFormats.TopLeft);
            gfx.DrawString("Renovation days: " + report.RenovationDaysCount, regularFont, XBrushes.Black, new XPoint(margin, 170), XStringFormats.TopLeft);

            string sentence = "Between " + report.ReportDateSpan.StartDate.ToShortDateString() + " and " + report.ReportDateSpan.EndDate.ToShortDateString() + " there were " + report.RenovationsCount.ToString() + " renovations. " +
                "Total day count of those renovations is " + report.RenovationDaysCount.ToString() + ".";

            tf.DrawString(sentence, regularFont, XBrushes.Black, new XRect(margin, 220, page.Width - 10, 40), format);

            int tablePosition = 260;

            gfx.DrawString("Accommodation", boldFont, XBrushes.Black, new XRect(margin, tablePosition, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("No. renovations", boldFont, XBrushes.Black, new XRect(250, tablePosition, 50, 20), XStringFormats.TopLeft);
            gfx.DrawString("Renovation days", boldFont, XBrushes.Black, new XRect(450, tablePosition, 50, 20), XStringFormats.TopLeft);
            int index = 1;
            foreach (var accommodation in report.AccommodationStats)
            {
                gfx.DrawString(accommodation.Accommodation.Name, regularFont, XBrushes.Black, new XRect(margin, tablePosition + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(accommodation.ScheduledRenovationsCount.ToString(), regularFont, XBrushes.Black, new XRect(250, tablePosition + (index * 20), 50, 20), XStringFormats.TopLeft);
                gfx.DrawString(accommodation.RenovationDaysCount.ToString(), regularFont, XBrushes.Black, new XRect(450, tablePosition + (index * 20), 50, 20), XStringFormats.TopLeft);
                index++;
            }

            SaveReport(PDFReport);
        }
    }
}
