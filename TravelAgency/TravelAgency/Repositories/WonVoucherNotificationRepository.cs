using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class WonVoucherNotificationRepository : IWonVoucherNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/wonvouchersnotifications.csv";
        private readonly Serializer<WonVoucherNotification> _serializer;
        private List<WonVoucherNotification> WonVoucherNotifications;

        public WonVoucherNotificationRepository()
        {
            _serializer = new Serializer<WonVoucherNotification>();
            WonVoucherNotifications = _serializer.FromCSV(FilePath);
        }

        public List<WonVoucherNotification> GetAll()
        {
            return WonVoucherNotifications;
        }

        public void Save(WonVoucherNotification WonVoucherNotification)
        {
            WonVoucherNotifications.Add(WonVoucherNotification);
            _serializer.ToCSV(FilePath, WonVoucherNotifications);
        }
        public void Update(WonVoucherNotification WonVoucherNotification)
        {
            WonVoucherNotification oldNotification = WonVoucherNotifications.Find(v => v.GuestId == WonVoucherNotification.GuestId && v.Year == WonVoucherNotification.Year);
            oldNotification.Seen = WonVoucherNotification.Seen;
            _serializer.ToCSV(FilePath, WonVoucherNotifications);
        }
        public WonVoucherNotification GetByGuestId(int guestId)
        {
            foreach(WonVoucherNotification notification in WonVoucherNotifications)
            {
                if (notification.GuestId == guestId && notification.Year == DateTime.Now.Year)
                    return notification;
            }
            return null;
        }

        public bool HasGuestWonVoucher(int guestId)
        {
            return WonVoucherNotifications.Exists(v => v.GuestId == guestId && v.Year == DateTime.Now.Year);
        }
    }
}
