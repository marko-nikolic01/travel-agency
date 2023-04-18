using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Services;
using LiveCharts.Defaults;

namespace TravelAgency.ViewModel
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
        public TourStatisticsDetailsViewModel(TourOccurrence tourOccurrence)
        {
            SelectedTourOccurrence = tourOccurrence;
            var attendanceService = new TourOccurrenceAttendanceService();
            GuestsUnder18 = attendanceService.GetGuestsUnder18(SelectedTourOccurrence.Id);
            Guests18to50 = attendanceService.GetGuest18to50(SelectedTourOccurrence.Id);
            GestsAbove50 = attendanceService.GetGuestsAbove50(SelectedTourOccurrence.Id);
            var voucherService = new VoucherService();
            SeriesCollectionVouchers = new SeriesCollection();
            int used = voucherService.GetUsedVoucherByTour(tourOccurrence.Id);
            SeriesCollectionVouchers.Add(new PieSeries{ Title = "Used voucher", Values = new ChartValues<ObservableValue> { new ObservableValue(used) } });
            int notUsed = attendanceService.GetGuestsNumberByTour(tourOccurrence.Id) - voucherService.GetUsedVoucherByTour(tourOccurrence.Id);
            SeriesCollectionVouchers.Add(new PieSeries{ Title = "Used voucher", Values = new ChartValues<ObservableValue> { new ObservableValue(notUsed) } });
            GuestsUsedVoucher = (double)used / attendanceService.GetGuestsNumberByTour(SelectedTourOccurrence.Id);
            GuestsNotUsedVoucher = (1 - GuestsUsedVoucher);
            SeriesCollectionAges = new SeriesCollection{ new ColumnSeries{Values = new ChartValues<int> { GuestsUnder18, Guests18to50, GestsAbove50}} };
        }
    }
}
