using System;
using System.Collections.Generic;
using System.IO;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
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
            UpdateHelpText();
            GuestId = guestId;
            voucherService = new VoucherService();
            Vouchers = voucherService.GetGuestVouchers(GuestId);
            PrintVouchersNumber();
        }
        private void PrintVouchersNumber()
        {
            if (Vouchers == null)
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
            if (SelectedVoucher != null)
            {
                voucherService.DisableVoucher(SelectedVoucher, tourOccurrenceId);
            }
        }
        private void UpdateHelpText()
        {
            string file = @"../../../Resources/HelpTexts/VouchersHelp.txt";
            Guest2MainViewModel.HelpText = File.ReadAllText(file);
        }
    }
}
