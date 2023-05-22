using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class VoucherService
    {
        private IVoucherRepository IVoucherRepository;
        private ITourOccurrenceAttendanceRepository IAttendanceRepository { get; set; }
        private ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        private IWonVoucherNotificationRepository IWonVoucherNotificationRepository { get; set; }
        private IUserRepository IUserRepository { get; set; }

        public VoucherService()
        {
            IVoucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();
            IAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IWonVoucherNotificationRepository = Injector.Injector.CreateInstance<IWonVoucherNotificationRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
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
                if (attendance == null)
                {
                    result--;
                }
                else if (attendance.ResponseStatus != ResponseStatus.Accepted)
                {
                    result--;
                }
            }
            return result;
        }
        public void CheckIfGuestsWonVouchers()
        {
            foreach(User user in IUserRepository.GetAll())
            {
                if(user.Role == Roles.Guest2)
                {
                    if (!HasGuestWonVoucher(user.Id))
                        CheckIfVoucherWon(user.Id);
                }
            }
        }
        public bool HasGuestWonVoucher(int guestId)
        {
            return IWonVoucherNotificationRepository.HasGuestWonVoucher(guestId);
        }
        public void CheckIfVoucherWon(int guestId)
        {
            int toursPresence = 0;
            foreach(TourOccurrenceAttendance attendance in IAttendanceRepository.GetByGuestId(guestId))
            {
                int year = ITourOccurrenceRepository.GetById(attendance.TourOccurrenceId).DateTime.Year;
                if(year == DateTime.Now.Year && attendance.ResponseStatus == ResponseStatus.Accepted) 
                {
                    toursPresence++;
                    if (toursPresence == 5)
                    {
                        MakeVoucherNotification(guestId, year);
                        MakeVoucher(guestId);
                        break;
                    }
                }
            }
        }
        public void MakeVoucherNotification(int guestId, int year)
        {
            WonVoucherNotification WonVoucherNotification = new WonVoucherNotification();
            WonVoucherNotification.Year = year;
            WonVoucherNotification.GuestId = guestId;
            WonVoucherNotification.Seen = false;
            IWonVoucherNotificationRepository.Save(WonVoucherNotification);
        }
        public void MakeVoucher(int guestId)
        {
            Voucher voucher = new Voucher();
            voucher.GuestId = guestId;
            voucher.TourOccurrenceId = -1;
            voucher.GuideId = -1;
            voucher.IsUsed = false;
            voucher.CanceledTourOccurrenceId = -1;
            voucher.Deadline = DateTime.Now.AddMonths(6);
            IVoucherRepository.Save(voucher);
        }
        public WonVoucherNotification GetVoucherNotification(int guestId)
        {
            return IWonVoucherNotificationRepository.GetByGuestId(guestId);
        }
        public void UpdateNotification(WonVoucherNotification notification)
        {
            IWonVoucherNotificationRepository.Update(notification);
        }
    }
}
