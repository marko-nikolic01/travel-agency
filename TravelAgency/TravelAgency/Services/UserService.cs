using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

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

        public List<User> GetAllUsers()
        {
            return IUserRepository.GetUsers();
        }

        public void SaveUser(User user)
        {
            IUserRepository.SaveUser(user);
        }

        public void LogInUser(User user)
        {
            IUserRepository.LogInUser(user);
        }

        public User GetLoggedInUser()
        {
            return IUserRepository.GetLoggedInUser();
        }
    }
}
