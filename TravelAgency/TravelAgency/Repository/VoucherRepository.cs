using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
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
            foreach(var voucher in vouchers)
            {
                if(voucher.TourOccurrenceId == id)
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
            oldVoucher.IsUsed = voucher.IsUsed;
            _serializer.ToCSV(FilePath, vouchers);
            return oldVoucher;
        }
    }
}
