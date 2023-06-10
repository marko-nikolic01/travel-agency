using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Models
{
    public class DateSpan : INotifyPropertyChanged
    {
        private DateOnly _startDate;
        private DateOnly _endDate;
        private int _dayCount;

        public DateOnly StartDate
        {
            get => _startDate;
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    DayCount = (EndDate.DayNumber - StartDate.DayNumber + 1);
                    OnPropertyChanged("StartDate");
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
                    DayCount = (EndDate.DayNumber - StartDate.DayNumber + 1);
                    OnPropertyChanged("EndDate");
                }
            }
        }

        public int DayCount
        {
            get => _dayCount;
            set
            {
                if (value != _dayCount)
                {
                    _dayCount = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DateSpan()
        {
            StartDate = new DateOnly();
            EndDate = new DateOnly();
        }

        public DateSpan(DateOnly startDate, DateOnly endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public int DaysCount()
        {
            return EndDate.DayNumber - StartDate.DayNumber;
        }
    }
}
