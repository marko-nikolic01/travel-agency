using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class Forum : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public User Admin { get; set; }
        public Location Location { get; set; }
        private List<Comment> _comments;
        private bool _closed;

        public List<Comment> Comments
        {
            get => _comments;
            set
            {
                if (value != _comments)
                {
                    _comments = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Closed
        {
            get => _closed;
            set
            {
                if (value != _closed)
                {
                    _closed = value;
                    OnPropertyChanged();
                }
            }
        }

        public Forum() 
        {
            Admin = new User();
            Location = new Location();
            Comments = new List<Comment>();
            Closed = false;
        }

        public void Close()
        {
            Closed = true;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Admin.Id.ToString(),
                Location.Id.ToString(),
                Convert.ToInt32(Closed).ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Admin.Id = Convert.ToInt32(values[1]);
            Location.Id = Convert.ToInt32(values[2]);
            Closed = Convert.ToBoolean(Convert.ToInt32(values[3]));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
