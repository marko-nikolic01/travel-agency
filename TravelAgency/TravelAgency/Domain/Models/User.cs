using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    //The idea for this class was mostly taken from the example uploaded on canvas
    public enum Roles { Owner, Guest1, Guide, Guest2 }
    public class User : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSuperOwner { get; set; }
        private bool _isSuperGuest;
        public bool IsSuperGuest
        {
            get => _isSuperGuest;
            set
            {
                if (value != _isSuperGuest)
                {
                    _isSuperGuest = value;
                    OnPropertyChanged();
                }
            }
        }
        public SuperGuestTitle SuperGuestTitle { get; set; }
        public Roles Role { get; set; }
        public DateOnly BirthDay { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public User()
        {
            Id = -1;
            Username = "";
            Password = "";
            IsSuperOwner = false;
            IsSuperGuest = false;
            SuperGuestTitle = null;
            Name = Username;
            Gender = "Male";
        }

        public User(string username, string password, Roles role, DateOnly birthDay, bool isSuperOwner = false)
        {
            Username = username;
            Password = password;
            Role = role;
            IsSuperOwner = isSuperOwner;
            BirthDay = birthDay;
            SuperGuestTitle = null;
            Name = username;
            Gender = "Male";
        }


        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, ((int)Role).ToString(), Convert.ToInt32(IsSuperOwner).ToString(), BirthDay.ToString("dd-MM-yyyy"), Name, Gender };
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
            Name = values[6];
            Gender = values[7];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
