using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TravelAgency.Domain.Models
{
    public class Comment : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public Forum Forum { get; set; }
        public User User { get; set; }
        private string _text;
        private bool _locationVisited;
        private bool _ownsAccommodationOnLocation;

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

        public bool LocationVisited
        {
            get => _locationVisited;
            set
            {
                if (value != _locationVisited)
                {
                    _locationVisited = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool OwnsAccommodationOnLocation
        {
            get => _ownsAccommodationOnLocation;
            set
            {
                if (value != _ownsAccommodationOnLocation)
                {
                    _ownsAccommodationOnLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        public Comment() 
        {
            Forum = new Forum();
            User = new User();
            LocationVisited = false;
            OwnsAccommodationOnLocation = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Forum.Id.ToString(),
                User.Id.ToString(),
                Text,
                Convert.ToInt32(LocationVisited).ToString(),
                Convert.ToInt32(LocationVisited).ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Forum.Id = Convert.ToInt32(values[1]);
            User.Id = Convert.ToInt32(values[2]);
            Text = values[3];
            LocationVisited = Convert.ToBoolean(Convert.ToInt32(values[4]));
            OwnsAccommodationOnLocation = Convert.ToBoolean(Convert.ToInt32(values[5]));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
