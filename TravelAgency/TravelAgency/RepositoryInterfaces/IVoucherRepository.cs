using System.Collections.Generic;
using TravelAgency.Model;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IVoucherRepository
    {
        List<Voucher> GetAll();
        int NextId();
        Voucher Save(Voucher voucher);
        Voucher Update(Voucher voucher);
        public List<Voucher> GetByTourOccurrenceId(int id);
    }
}
