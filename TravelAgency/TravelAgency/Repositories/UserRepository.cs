using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    //This class was copied from the example uploaded on canvas
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;
        private User loggedInUser;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public void LinkSuperGuestTitles(List<SuperGuestTitle> titles) 
        {
            foreach (User user in _users)
            {
                foreach (SuperGuestTitle title in titles)
                {
                    if (title.GuestId == user.Id && title.IsActive())
                    {
                        user.IsSuperGuest = true;
                        user.SuperGuestTitle = title;
                    }
                }
            }
        }

        public List<User> GetAll()
        {
            return _users;
        }
        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            User user = _users.FirstOrDefault(u => u.Username == username);
            if (user.IsDeleted)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        public User GetById(int id)
        {
            User user = _users.Find(u => u.Id == id);
            return user;
        }

        private int GetNewId()
        {
            if (_users.Count == 0)
            {
                return 1;
            }
            return _users[_users.Count - 1].Id + 1;
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public void SaveUser(User user)
        {
            user.Id = GetNewId();
            _users.Add(user);
            _serializer.ToCSV(FilePath, _users);
        }

        public void UpdateSuperOwners()
        {
            _serializer.ToCSV(FilePath, _users);
        }

        public List<User> GetOwners()
        {
            return _users.FindAll(u => u.Role == Roles.Owner);
        }
        public List<User> GetGuests2()
        {
            return _users.FindAll(u => u.Role == Roles.Guest2);
        }
        public void LogInUser(User user)
        {
            loggedInUser = user;
        }

        public User GetLoggedInUser()
        {
            return loggedInUser;
        }
        public void UpdateNewUsername(int userId, string newUsername)
        {
            User oldUser = _users.Find(u => u.Id == userId);
            oldUser.Username = newUsername;
            _serializer.ToCSV(FilePath, _users);
        }
        public void UpdateNewPassword(int userId, string newPassword)
        {
            User oldUser = _users.Find(u => u.Id == userId);
            oldUser.Password = newPassword;
            _serializer.ToCSV(FilePath, _users);
        }
        public bool CheckPassword(int userId, string Password)
        {
            User user = _users.Find(u => u.Id == userId);
            return user.Password == Password;
        }
        public void UpdateSuperGuideStatus(int userId, bool status, string language)
        {
            User oldUser = _users.Find(u => u.Id == userId);
            oldUser.IsSuperGuide = status;
            oldUser.Language = language;
            _serializer.ToCSV(FilePath, _users);
        }
        public void DeleteUser(int userId)
        {
            User oldUser = _users.Find(u => u.Id == userId);
            oldUser.IsDeleted = true;
            _serializer.ToCSV(FilePath, _users);
        }
    }
}
