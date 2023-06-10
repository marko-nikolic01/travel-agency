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
    public class Forum : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public User Admin { get; set; }
        
        public Location Location { get; set; }
        private string _title;
        private List<Comment> _comments;
        private bool _closed;
        private int _commentsByVisitors;
        private int _commentsByAccommodationOwners;
        private bool _useful;

        public string Title
        {
            get => _title;
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public int CommentsByVisitors
        {
            get => _commentsByVisitors;
            set
            {
                if (value != _commentsByVisitors)
                {
                    _commentsByVisitors = value;
                    MarkAsUseful();
                    OnPropertyChanged();
                }
            }
        }

        public int CommentsByAccommodationOwners
        {
            get => _commentsByAccommodationOwners;
            set
            {
                if (value != _commentsByAccommodationOwners)
                {
                    _commentsByAccommodationOwners = value;
                    MarkAsUseful();
                    OnPropertyChanged();
                }
            }
        }

        public bool Useful
        {
            get => _useful;
            set
            {
                if (value != _useful)
                {
                    _useful = value;
                    OnPropertyChanged();
                }
            }
        }

        public Forum() 
        {
            Admin = new User();
            Location = new Location();
            Title = "";
            Comments = new List<Comment>();
            Closed = false;
            CommentsByVisitors = 0;
            CommentsByAccommodationOwners = 0;
            Useful = false;
        }

        public Forum(User admin, Location location)
        {
            Admin = admin;
            Location = location;
            Title = "";
            Comments = new List<Comment>();
            Closed = false;
            CommentsByVisitors = 0;
            CommentsByAccommodationOwners = 0;
            Useful = false;
        }

        public void Close()
        {
            Closed = true;
        }

        private void MarkAsUseful()
        {
            if ((CommentsByVisitors >= 20) || (CommentsByAccommodationOwners >= 10))
            {
                Useful = true;
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Admin.Id.ToString(),
                Location.Id.ToString(),
                Convert.ToInt32(Closed).ToString(),
                CommentsByVisitors.ToString(),
                CommentsByAccommodationOwners.ToString(),
                Title
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Admin.Id = Convert.ToInt32(values[1]);
            Location.Id = Convert.ToInt32(values[2]);
            Closed = Convert.ToBoolean(Convert.ToInt32(values[3]));
            CommentsByVisitors = Convert.ToInt32(values[4]);
            CommentsByAccommodationOwners = Convert.ToInt32(values[5]);
            Title = values[6];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Title")
                {
                    if (Title == "")
                    {
                        return "* Naslov je obavezan";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Title" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
