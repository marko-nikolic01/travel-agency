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
        public IUserRepository IUserRepository { get; set; }
        public UserService()
        {
            
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }

        public User GetById(int id)
        {
            return IUserRepository.GetById(id);
        }
    }
}
