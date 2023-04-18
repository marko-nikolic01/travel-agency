using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class VoucherService
    {
        private IVoucherRepository IVoucherRepository;
        private ITourOccurrenceAttendanceRepository IAttendanceRepository { get; set; }

        public VoucherService() 
        {
            IVoucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();
            IAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
        }

        public List<Voucher>? GetGuestVouchers(int guestId)
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach (Voucher voucher in IVoucherRepository.GetAll())
            {
                if (voucher.Deadline > DateTime.Now && voucher.GuestId == guestId && !voucher.IsUsed)
                {
                    voucher.BuildVoucherString();
                    vouchers.Add(voucher);
                }
            }
            return vouchers;
        }

        public void DisableVoucher(Voucher selectedVoucher, int tourOccurrenceId)
        {
            foreach (Voucher voucher in IVoucherRepository.GetAll())
            {
                if (voucher.Id == selectedVoucher.Id)
                {
                    voucher.TourOccurrenceId = tourOccurrenceId;
                    voucher.IsUsed = true;
                    IVoucherRepository.Update(voucher);
                    return;
                }
            }
        }

        public int GetUsedVoucherByTour(int tourOccurrenceId)
        {
            var vouchers = IVoucherRepository.GetByTourOccurrenceId(tourOccurrenceId);
            int result = vouchers.Count();
            foreach (var voucher in vouchers)
            {
                var attendance = IAttendanceRepository.GetByTourOccurrenceIdAndGuestId(tourOccurrenceId, voucher.GuestId);
                if(attendance == null)
                {
                    result--;
                }
                else if(attendance.ResponseStatus != ResponseStatus.Accepted)
                {
                    result--;
                }
            }
            return result;
        }
    }
}
