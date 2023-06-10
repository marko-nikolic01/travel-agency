using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Services;
using LiveCharts.Defaults;
using TravelAgency.Domain.Models;
using TravelAgency.Commands;
using System.Windows;
using TravelAgency.WPF.Views;
using System.Windows.Navigation;
using System.Windows.Controls;

namespace TravelAgency.WPF.ViewModels
{
    public class TourStatisticsDetailsViewModel
    {
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public int GuestsUnder18 { get; set; }
        public int GestsAbove50 { get; set; }
        public int Guests18to50 { get; set; }
        public double GuestsUsedVoucher { get; set; }
        public double GuestsNotUsedVoucher { get; set; }
        public SeriesCollection SeriesCollectionVouchers { get; set; }
        public SeriesCollection SeriesCollectionAges { get; set; }
        public ButtonCommandNoParameter GenerateReportCommand { get; set; }
        public TourOccurrenceAttendanceService TourOccurrenceAttendanceService { get; set; }
        public PDFReportService PDFReportService { get; set; }
        public UserService UserService { get; set; }
        public NavigationService NavigationService { get; set; }
        public User ActiveGuide { get; set; }
        public TourStatisticsDetailsViewModel(TourOccurrence tourOccurrence, System.Windows.Navigation.NavigationService navService)
        {
            NavigationService = navService;
            SelectedTourOccurrence = tourOccurrence;
            TourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            GuestsUnder18 = TourOccurrenceAttendanceService.GetGuestsUnder18(SelectedTourOccurrence.Id);
            Guests18to50 = TourOccurrenceAttendanceService.GetGuest18to50(SelectedTourOccurrence.Id);
            GestsAbove50 = TourOccurrenceAttendanceService.GetGuestsAbove50(SelectedTourOccurrence.Id);
            var voucherService = new VoucherService();
            SeriesCollectionVouchers = new SeriesCollection();
            int used = voucherService.GetUsedVoucherByTour(tourOccurrence.Id);
            SeriesCollectionVouchers.Add(new PieSeries { Title = "Used voucher", Values = new ChartValues<ObservableValue> { new ObservableValue(used) } });
            int notUsed = TourOccurrenceAttendanceService.GetGuestsNumberByTour(tourOccurrence.Id) - voucherService.GetUsedVoucherByTour(tourOccurrence.Id);
            SeriesCollectionVouchers.Add(new PieSeries { Title = "Not used voucher", Values = new ChartValues<ObservableValue> { new ObservableValue(notUsed) } });
            GuestsUsedVoucher = (double)used / TourOccurrenceAttendanceService.GetGuestsNumberByTour(SelectedTourOccurrence.Id);
            GuestsNotUsedVoucher = 1 - GuestsUsedVoucher;
            SeriesCollectionAges = new SeriesCollection { new ColumnSeries { Values = new ChartValues<int> { GuestsUnder18, Guests18to50, GestsAbove50 } } };
            PDFReportService = new PDFReportService();
            UserService = new UserService();
            ActiveGuide = UserService.GetLoggedInUser();
            GenerateReportCommand = new ButtonCommandNoParameter(GenerateReport);
        }
        public void GenerateReport()
        {
            string link = PDFReportService.WriteTourStatisticsReport(ActiveGuide, SelectedTourOccurrence, GuestsUnder18, Guests18to50, GestsAbove50, GuestsUsedVoucher, GuestsNotUsedVoucher);
            Page w = new GuideReportView(link);
            NavigationService.Navigate(w);
        }
    }
}
