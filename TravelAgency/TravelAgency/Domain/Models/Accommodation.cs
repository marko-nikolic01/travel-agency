﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Converters;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public enum AccommodationType { APARTMENT, HOUSE, HUT }
    public class Accommodation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationType type;
        public AccommodationType Type
        {
            get => type;
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }
        private int maxGuests;
        public int MaxGuests
        {
            get => maxGuests;
            set
            {
                if (value != maxGuests)
                {
                    maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private int minDays;
        public int MinDays
        {
            get => minDays;
            set
            {
                if (value != minDays)
                {
                    minDays = value;
                    OnPropertyChanged();
                }
            }
        }
        private int daysToCancel;
        public int DaysToCancel
        {
            get => daysToCancel;
            set
            {
                if (value != daysToCancel)
                {
                    daysToCancel = value;
                    OnPropertyChanged();
                }
            }
        }

        public User Owner { get; set; }
        public Location Location { get; set; }
        public List<AccommodationPhoto> Photos { get; set; }

        private bool isRenovated;

        public bool IsRenovated
        {
            get { return isRenovated; }
            set
            {
                isRenovated = value;
                OnPropertyChanged(nameof(IsRenovated));
            }
        }

        public bool IsOpen { get; set; }


        public Accommodation()
        {
            Id = -1;
            Name = "";
            Owner = new User();
            Location = new Location();
            Type = AccommodationType.APARTMENT;
            MaxGuests = 1;
            MinDays = 1;
            DaysToCancel = 1;

            Photos = new List<AccommodationPhoto>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                Owner.Id.ToString(),
                Location.Id.ToString(),
                Convert.ToInt32(Type).ToString(),
                MaxGuests.ToString(),
                MinDays.ToString(),
                DaysToCancel.ToString(),
                Convert.ToInt32(IsOpen).ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Owner.Id = int.Parse(values[2]);
            Location.Id = int.Parse(values[3]);
            Type = (AccommodationType)Convert.ToInt32(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinDays = Convert.ToInt32(values[6]);
            DaysToCancel = Convert.ToInt32(values[7]);
            IsOpen = Convert.ToBoolean(Convert.ToInt32(values[8]));
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
                        return "Name cannot be empty";
                    }
                }
                else if (columnName == "MaxGuests")
                {
                    if (MaxGuests < 1)
                    {
                        return "Max number of guests must be greater than 0";
                    }
                }
                else if (columnName == "MinDays")
                {
                    if (MinDays < 1)
                    {
                        return "Min number of days must be greater than 0";
                    }
                }
                else if (columnName == "DaysToCancel")
                {
                    if (DaysToCancel < 0)
                    {
                        return "Number of days to cancel must be greater than 0";
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
