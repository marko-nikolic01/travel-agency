using System;
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
        public Voucher SelectedVoucher { get; set; }
        public string VouchersNumber { get; set; }
        private VoucherService voucherService;
        public VoucherViewModel(int guestId)
        {
            GuestId = guestId;
            voucherService = new VoucherService();
            Vouchers = voucherService.GetGuestVouchers(GuestId);
            PrintVouchersNumber();
        }
        private void PrintVouchersNumber()
        {
            if(Vouchers == null)
            {
                VouchersNumber = "You don't have vouchers.";
            }
            else
            {
                VouchersNumber = "You have " + Vouchers.Count + " vouchers.";
            }
        }
        public void UpdateVoucher(int tourOccurrenceId)
        {
            if(SelectedVoucher != null)
            {
                voucherService.DisableVoucher(SelectedVoucher, tourOccurrenceId);
            }
        }
    }
}
