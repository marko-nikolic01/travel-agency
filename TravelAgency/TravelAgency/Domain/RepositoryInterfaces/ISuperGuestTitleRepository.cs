using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ISuperGuestTitleRepository
    {
        public List<SuperGuestTitle> GetAll();

        public SuperGuestTitle GetActiveByGuest(User guest);

        public int NextId();

        public SuperGuestTitle Save(SuperGuestTitle title);

        public void SaveAll();
    }
}
