using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TravelAgency.Domain.Models
{
    public class Notification : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        private User _user;
        private string _text;
        private bool _seen;

        public User User
        {
            get => _user;
            set
            {
                if (value != _user)
                {
                    _user = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Seen
        {
            get => _seen;
            set
            {
                if (value != _seen)
                {
                    _seen = value;
                    OnPropertyChanged();
                }
            }
        }

        public Notification()
        {
            Id = -1;
            User = new User();
            Text = "";
            Seen = false;
        }

        public Notification(User user, string text)
        {
            Id = -1;
            User = user;
            Text = text;
            Seen = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                User.Id.ToString(),
                Text,
                Convert.ToInt32(Seen).ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            User.Id = Convert.ToInt32(values[1]);
            Text = values[2];
            Seen = Convert.ToBoolean(Convert.ToInt32(values[3]));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
