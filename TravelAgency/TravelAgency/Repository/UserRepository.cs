using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    //This class was copied from the example uploaded on canvas
    public class UserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
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

        public List<User> GetAllGuests1()
        {
            List<User> guests1 = new List<User>();

            foreach (User user in _users)
            {
                if (user.Role == Roles.Guest1) 
                {
                    guests1.Add(user);
                }
            }

            return guests1;
        }
    }
}
