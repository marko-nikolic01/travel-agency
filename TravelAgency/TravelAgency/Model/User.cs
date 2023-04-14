using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    //The idea for this class was mostly taken from the example uploaded on canvas
    public enum Roles { Owner, Guest1, Guide, Guest2 }
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSuperOwner { get; set; }
        public Roles Role { get; set; }
        public DateOnly BirthDay { get;set; }
        public User()
        {
            Id = -1;
            Username = "";
            Password = "";
            IsSuperOwner = false;
        }

        public User(string username, string password, Roles role, DateOnly birthDay, bool isSuperOwner = false)
        {
            Username = username;
            Password = password;
            Role = role;
            IsSuperOwner = isSuperOwner;
            BirthDay = birthDay;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, ((int)Role).ToString(), Convert.ToInt32(IsSuperOwner).ToString(), BirthDay.ToString("dd-MM-yyyy") };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Role = (Roles)Convert.ToInt32(values[3]);
            IsSuperOwner = Convert.ToBoolean(Convert.ToInt32(values[4]));
            BirthDay = DateOnly.ParseExact(values[5], "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }
    }
}
