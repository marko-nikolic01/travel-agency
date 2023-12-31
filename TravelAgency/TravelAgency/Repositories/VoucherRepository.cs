﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private const string FilePath = "../../../Resources/Data/vouchers.csv";
        private readonly Serializer<Voucher> _serializer;
        private List<Voucher> vouchers;

        public VoucherRepository()
        {
            _serializer = new Serializer<Voucher>();
            vouchers = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (vouchers.Count == 0)
            {
                return 1;
            }
            return vouchers[vouchers.Count - 1].Id + 1;
        }

        public List<Voucher> GetAll()
        {
            return vouchers;
        }

        public List<Voucher> GetByTourOccurrenceId(int id)
        {
            List<Voucher> result = new List<Voucher>();
            foreach (var voucher in vouchers)
            {
                if (voucher.TourOccurrenceId == id && voucher.IsUsed == true)
                {
                    result.Add(voucher);
                }
            }
            return result;
        }

        public Voucher Save(Voucher voucher)
        {
            voucher.Id = NextId();
            vouchers.Add(voucher);
            _serializer.ToCSV(FilePath, vouchers);
            return voucher;
        }

        public Voucher Update(Voucher voucher)
        {
            Voucher oldVoucher = vouchers.Find(x => x.Id == voucher.Id);
            oldVoucher.GuideId = voucher.GuideId;
            oldVoucher.IsUsed = voucher.IsUsed;
            _serializer.ToCSV(FilePath, vouchers);
            return oldVoucher;
        }

        private void delete(List<int> ids)
        {
            foreach (int id in ids)
            {
                Voucher oldVoucher = vouchers.Find(x => x.Id == id);
                vouchers.Remove(oldVoucher);
                _serializer.ToCSV(FilePath, vouchers);
            }
        }
        public void DeleteByCanceledTourId(int canceledTour)
        {
            List<int> voucherIds = new List<int>();
            foreach(Voucher voucher in vouchers)
            {
                if(voucher.CanceledTourOccurrenceId == canceledTour)
                {
                    voucherIds.Add(voucher.Id);
                }
            }
            delete(voucherIds);
        }
    }
}
