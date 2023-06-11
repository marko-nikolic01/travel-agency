using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Text.RegularExpressions;

namespace TravelAgency.Domain.Models
{
    public class AccommodationReservation : ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public Accommodation Accommodation { get; set; }
        public User Guest { get; set; }
        public bool Canceled { get; set; }
        private int _numberOfGuests;
        private DateSpan _dateSpan;


        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests)
                {
                    _numberOfGuests = value;
                    OnPropertyChanged("NumberOfGuests");
                }
            }
        }

        public DateSpan DateSpan
        {
            get => _dateSpan;
            set
            {
                if (value != _dateSpan)
                {
                    _dateSpan = value;
                    OnPropertyChanged("DateSpan");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservation()
        {
            Id = -1;
            Accommodation = new Accommodation();
            Guest = new User();
            NumberOfGuests = -1;
            DateSpan = new DateSpan();
            Canceled = false;
        }

        public AccommodationReservation(Accommodation accommodation,User guest)
        {
            Id = -1;
            Accommodation = accommodation;
            Guest = guest;
            NumberOfGuests = -1;
            Canceled = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Accommodation.Id.ToString(),
                Guest.Id.ToString(),
                NumberOfGuests.ToString(),
                DateSpan.StartDate.ToString("dd/MM/yyyy"),
                DateSpan.EndDate.ToString("dd/MM/yyyy"),
                Convert.ToInt32(Canceled).ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Accommodation.Id = Convert.ToInt32(values[1]);
            Guest.Id = Convert.ToInt32(values[2]);
            NumberOfGuests = Convert.ToInt32(values[3]);
            DateSpan.StartDate = DateOnly.ParseExact(values[4], "dd/MM/yyyy");
            DateSpan.EndDate = DateOnly.ParseExact(values[5], "dd/MM/yyyy");
            Canceled = Convert.ToBoolean(Convert.ToInt32(values[6]));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NumberOfGuests")
                {
                    if (NumberOfGuests < 0)
                    {
                        return "* Number of guests can't be negative";
                    }
                    else if (NumberOfGuests == 0)
                    {
                        return "* Number of guests is required";
                    }
                    else if (NumberOfGuests > Accommodation.MaxGuests)
                    {
                        return "* Number of guests is bigger than allowed";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "NumberOfGuests"};

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
