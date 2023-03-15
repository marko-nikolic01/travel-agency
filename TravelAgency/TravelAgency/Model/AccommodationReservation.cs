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

namespace TravelAgency.Model
{
    public class AccommodationReservation : ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public int AccomodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }
        private int _numberOfGuests;
        private DateOnly _startDate;
        private DateOnly _endDate;


        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                if (value != _numberOfGuests)
                {
                    _numberOfGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly EndDate
        {
            get => _endDate;
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    OnPropertyChanged();
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
            AccomodationId = -1;
            GuestId = -1;
            NumberOfGuests = -1;
            StartDate = new DateOnly();
            EndDate = new DateOnly();
        }

        public AccommodationReservation(int id, int accommodationId, int guestId, int numberOfGuests, DateOnly startDate, DateOnly endDate)
        {
            Id = id;
            AccomodationId = accommodationId;
            GuestId = guestId;
            NumberOfGuests = numberOfGuests;
            StartDate = startDate;
            EndDate = endDate;
        }

        public AccommodationReservation(int accommodationId, Accommodation accommodation, int guestId, User guest)
        {
            Id = -1;
            AccomodationId = accommodationId;
            Accommodation = accommodation;
            GuestId = guestId;
            Guest = guest;
            NumberOfGuests = -1;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccomodationId.ToString(),
                GuestId.ToString(),
                NumberOfGuests.ToString(),
                StartDate.ToString(),
                EndDate.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccomodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            NumberOfGuests = Convert.ToInt32(values[3]);
            StartDate = DateOnly.Parse(values[4]);
            EndDate = DateOnly.Parse(values[5]);
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
                else if (columnName == "StartDate")
                {
                    if (StartDate == null)
                    {
                        return "* Start date is required";
                    }
                }
                else if (columnName == "EndDate")
                {
                    if (EndDate == null)
                    {
                        return "* End date is required";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "NumberOfGuests", "StartDate", "EndDate"};

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
