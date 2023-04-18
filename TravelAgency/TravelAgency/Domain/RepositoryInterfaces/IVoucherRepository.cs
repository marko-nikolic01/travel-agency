using System.Collections.Generic;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
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
