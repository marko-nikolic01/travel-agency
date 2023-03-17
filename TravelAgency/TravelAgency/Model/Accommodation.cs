using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TravelAgency.Model
{
    public enum AccommodationType { APARTMENT, HOUSE, HUT }
    public class Accommodation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public int OwnerId { get; set; }
        public int LocationId { get; set; }
        private AccommodationType _type;
        public AccommodationType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxGuests;
        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _minDays;
        public int MinDays
        {
            get => _minDays;
            set
            {
                if (value != _minDays)
                {
                    _minDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _daysToCancel;
        public int DaysToCancel
        {
            get => _daysToCancel;
            set
            {
                if (value != _daysToCancel)
                {
                    _daysToCancel = value;
                    OnPropertyChanged();
                }
            }
        }

        public User? Owner { get; set; }
        public Location? Location { get; set; }
        public List<AccommodationPhoto> Images { get; set; }

        public Accommodation()
        {
            Id = -1;
            Name = "";
            OwnerId = -1;
            LocationId = -1;
            Type = AccommodationType.APARTMENT;
            MaxGuests = 1;
            MinDays = 1;
            DaysToCancel = 0;

            Images = new List<AccommodationPhoto>();
        }

        public Accommodation(int id, string name, int ownerId, int locationId, AccommodationType type, int maxGuests, int minDays, int daysToCancel)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            LocationId = locationId;
            Type = type;
            MaxGuests = maxGuests;
            MinDays = minDays;
            DaysToCancel = daysToCancel;

            Images = new List<AccommodationPhoto>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            {
                Id.ToString(),
                Name,
                OwnerId.ToString(),
                LocationId.ToString(),
                Convert.ToInt32(Type).ToString(),
                MaxGuests.ToString(),
                MinDays.ToString(),
                DaysToCancel.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            OwnerId = int.Parse(values[2]);
            LocationId = int.Parse(values[3]);
            Type = (AccommodationType)Convert.ToInt32(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinDays = Convert.ToInt32(values[6]);
            DaysToCancel= Convert.ToInt32(values[7]);
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
                if (columnName == "Name")
                {
                    if (Name == "")
                    {
                        return "Naziv mora biti popunjen";
                    }
                }
                else if (columnName == "MaxGuests")
                {
                    if (MaxGuests < 1)
                    {
                        return "Maksimalan broj gostiju mora biti veći od 0";
                    }
                }
                else if (columnName == "MinDays")
                {
                    if (MinDays< 1)
                    {
                        return "Minimalan broj dana mora biti veći od 0";
                    }
                }
                else if (columnName == "DaysToCancel")
                {
                    if (DaysToCancel < 1)
                    {
                        return "Broj dana za otkazivanje mora biti veći od 0";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Name", "MaxGuests", "MinDays", "DaysToCancel" };

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
