using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;
using TravelAgency.Services;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        public List<User> GetAll();
        public User GetByUsername(string username);

        public User GetById(int id);

        public List<User> GetUsers();

        public void SaveUser(User user);

        public void UpdateSuperOwners();

        public List<User> GetOwners();
        public List<User> GetGuests2();
        public void LogInUser(User user);

        public User GetLoggedInUser();
        public void UpdateNewUsername(int userId, string newUsername);
        public void UpdateNewPassword(int userId, string newPassword);
        public bool CheckPassword(int userId, string Password);
        public void LinkSuperGuestTitles(List<SuperGuestTitle> titles);
        public void UpdateSuperGuideStatus(int userId, bool status, string language);
    }
}
