using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class Comment : ISerializable, INotifyPropertyChanged
    {
       

        public string[] ToCSV()
        {
            string[] csvValues =
            {/*
                Id.ToString(),
                Name,
                OwnerId.ToString(),
                LocationId.ToString(),
                Convert.ToInt32(Type).ToString(),
                MaxGuests.ToString(),
                MinDays.ToString(),
                DaysToCancel.ToString()*/
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {/*
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            OwnerId = int.Parse(values[2]);
            LocationId = int.Parse(values[3]);
            Type = (AccommodationType)Convert.ToInt32(values[4]);
            MaxGuests = Convert.ToInt32(values[5]);
            MinDays = Convert.ToInt32(values[6]);
            DaysToCancel = Convert.ToInt32(values[7]);*/
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
