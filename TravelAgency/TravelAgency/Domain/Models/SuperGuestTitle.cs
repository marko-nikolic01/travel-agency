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
    public class SuperGuestTitle : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        private DateOnly _startDate;
        private int _points;

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
            get => _startDate.AddYears(1);
        }

        public int Points
        {
            get => _points;
            set
            {
                if (value != _points)
                {
                    _points = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                StartDate.ToString("dd/MM/yyyy"),
                Points.ToString()
            };
            return csvValues;
        }

        public SuperGuestTitle()
        {
            Id = -1;
            GuestId = -1;
            StartDate = DateOnly.FromDateTime(DateTime.Now);
            Points = 5;
        }

        public SuperGuestTitle(User guest)
        {
            Id = -1;
            GuestId = guest.Id;
            StartDate = DateOnly.FromDateTime(DateTime.Now);
            Points = 5;
        }

        public void DeductPoint()
        {
            if (Points > 0)
            {
                Points--;
            }
        }

        public void ReturnPoint()
        {
            if (Points < 5)
            {
                Points++;
            }
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            StartDate = DateOnly.ParseExact(values[2], "dd/MM/yyyy");
            Points = Convert.ToInt32(values[3]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
