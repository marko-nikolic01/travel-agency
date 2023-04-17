using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class UserService
    {
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public ITourReservationRepository ITourReservationRepository { get; set; }
        public IUserRepository IUserRepository { get; set; }
        public UserService()
        {
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            ITourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
        }

        public User GetById(int id)
        {
            return IUserRepository.GetById(id);
        }
    }
}
