using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Services;

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
        public TourStatisticsDetailsViewModel(TourOccurrence tourOccurrence)
        {
            SelectedTourOccurrence = tourOccurrence;
            var attendanceService = new TourOccurrenceAttendanceService();
            GuestsUnder18 = attendanceService.GetGuestsUnder18(SelectedTourOccurrence.Id);
            Guests18to50 = attendanceService.GetGuest18to50(SelectedTourOccurrence.Id);
            GestsAbove50 = attendanceService.GetGuestsAbove50(SelectedTourOccurrence.Id);
            var voucherService = new VoucherService(new VoucherRepository());
            GuestsUsedVoucher = voucherService.GetUsedVoucherByTour(tourOccurrence.Id) / (double)attendanceService.GetGuestsNumberByTour(tourOccurrence.Id);
            GuestsNotUsedVoucher = 1 - GuestsUsedVoucher;
        }
    }
}
