using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class VoucherService
    {
        private IVoucherRepository voucherRepository;
        public VoucherService(IVoucherRepository repository) 
        {
            voucherRepository = repository;
        }

        public List<Voucher>? GetGuestVouchers(int guestId)
        {
            List<Voucher> vouchers = new List<Voucher>();
            foreach (Voucher voucher in voucherRepository.GetAll())
            {
                if (voucher.Deadline > DateTime.Now && voucher.GuestId == guestId && !voucher.IsUsed)
                {
                    vouchers.Add(voucher);
                }
            }
            return vouchers;
        }
    }
}
