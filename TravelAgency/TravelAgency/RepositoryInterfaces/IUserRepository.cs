using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public User GetByUsername(string username);

        public User GetById(int id);

        public List<User> GetUsers();

        public void SaveUser(User user);

        public void UpdateSuperOwners();

        public List<User> GetOwners();
    }
}
