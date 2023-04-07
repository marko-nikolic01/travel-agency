using System.Collections.Generic;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Services;

namespace TravelAgency.ViewModel
{
    public class VoucherViewModel
    {
        public int GuestId { get; set; }
        public List<Voucher> Vouchers { get; set; }
        private VoucherService voucherService;
        public VoucherViewModel(int guestId) 
        {
            GuestId = guestId;
            VoucherRepository voucherRepository = new VoucherRepository();
            voucherService = new VoucherService(voucherRepository);
            Vouchers = voucherService.GetGuestVouchers(GuestId);
        }

    }
}
