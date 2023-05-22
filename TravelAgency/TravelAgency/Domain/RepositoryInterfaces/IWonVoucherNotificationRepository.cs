using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IWonVoucherNotificationRepository
    {
        public List<WonVoucherNotification> GetAll();
        public void Save(WonVoucherNotification voucher);
        public void Update(WonVoucherNotification voucher);
        public WonVoucherNotification GetByGuestId(int guestId);
        public bool HasGuestWonVoucher(int guestId);
    }
}
