using System.Windows.Controls;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using TravelAgency.WPF.ViewModels;
using Syncfusion.Pdf.Grid;
using System.Data;
using TravelAgency.Services;
using TravelAgency.Domain.DTOs;
using System.Collections.Generic;

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
        //Za generisanje i prikaz pdf dokumenata iskoriscen Syncfusion.Pdf.Wpf i Syncfusion.PdfViewer.Wpf NuGet
        //https://www.syncfusion.com/
        private void GenerateReport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.PrepareData();
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGraphics graphics = page.Graphics;
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
            graphics.DrawString("Report on your presence on the tours \n" +
                "from "+viewModel.StartDate.ToString()+"  to "+ viewModel.EndDate.ToString(), font, PdfBrushes.Black, new PointF(0, 0));
            PdfGrid pdfGrid = new PdfGrid();
            PdfGridStyle gridStyle = new PdfGridStyle();
            gridStyle.CellPadding.All = 5;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Tour Name");
            dataTable.Columns.Add("Date Time");
            dataTable.Columns.Add("Status");
            dataTable.Columns.Add("Key Point");
            foreach(TourOccurrenceAttendanceDTO attendanceDTO in  viewModel.tourOccurrenceAttendanceDTOs) 
            {
                dataTable.Rows.Add(new object[] { attendanceDTO.TourName, attendanceDTO.TourDateTime.ToString(), attendanceDTO.Status, attendanceDTO.ArrivalKeyPoint });
            }
            pdfGrid.DataSource = dataTable;
            pdfGrid.Style = gridStyle;
            pdfGrid.Draw(page, new PointF(0, 60));
            doc.Save(@"../../../ReportsPDF/Guest2Report.pdf");
            doc.Close(true);
            Guest2ReportView reportView = new Guest2ReportView();
            this.NavigationService.Navigate(reportView);
        }
    }
}
