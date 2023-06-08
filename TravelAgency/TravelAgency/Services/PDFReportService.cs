using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private XFont boldFont;
        private XFont titleFont;
        private XStringFormat format;

        public PDFReportService() 
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            regularFont = new XFont("Times New Roman", 13, XFontStyle.Regular);
            boldFont = new XFont("Times New Roman", 13, XFontStyle.Bold);
            titleFont = new XFont("Times New Roman", 24, XFontStyle.Regular);
            format = new XStringFormat();
            format.LineAlignment = XLineAlignment.Near;
            format.Alignment = XStringAlignment.Near;
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
    }
}
