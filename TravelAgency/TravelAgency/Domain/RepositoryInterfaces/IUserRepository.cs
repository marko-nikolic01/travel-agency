using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
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
