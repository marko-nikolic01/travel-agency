using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Models
{
    public class WhereverWheneverSearchFilter : INotifyPropertyChanged, IDataErrorInfo
    {
        private int _guestNumber;
        private int _dayNumber;
        private bool _searchInsideDateSpan;
        private DateTime _firstDate;
        private DateTime _lastDate;

        public int GuestNumber
        {
            get { return _guestNumber; }
            set
            {
                _guestNumber = value;
                OnPropertyChanged();
            }
        }

        public int DayNumber
        {
            get { return _dayNumber; }
            set
            {
                _dayNumber = value;
                OnPropertyChanged();
            }
        }

        public bool SearchInsideDateSpan
        {
            get { return _searchInsideDateSpan; }
            set
            {
                _searchInsideDateSpan = value;
                if (value) 
                {
                    ResetDateSpan();
                }
                OnPropertyChanged();
            }
        }

        public DateTime FirstDate
        {
            get { return _firstDate; }
            set
            {
                _firstDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime LastDate
        {
            get { return _lastDate; }
            set
            {
                _lastDate = value;
                OnPropertyChanged();
            }
        }

        public WhereverWheneverSearchFilter()
        {
            GuestNumber = 1;
            DayNumber = 1;
            SearchInsideDateSpan = false;
        }

        private void ResetDateSpan()
        {
            FirstDate = DateTime.Now.AddDays(1);
            LastDate = DateTime.Now.AddDays(1);
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
                if (columnName == "DayNumber")
                {
                    if (DayNumber < 0)
                    {
                        return "* Number of days can't be negative";
                    }
                    else if (DayNumber == 0)
                    {
                        return "* Number of days is required";
                    }
                }
                else if (columnName == "GuestNumber")
                {
                    if (DayNumber < 0)
                    {
                        return "* Number of guests can't be negative";
                    }
                    else if (DayNumber == 0)
                    {
                        return "* Number of days is required";
                    }
                }
                if (!SearchInsideDateSpan)
                {
                    return null;
                }
                if (columnName == "FirstDate")
                {
                    bool isFutureDate = FirstDate.CompareTo(DateTime.Now) > 0;

                    if (!isFutureDate)
                    {
                        return "* First date must be a future date";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified number of days";
                    }

                }
                else if (columnName == "LastDate")
                {
                    bool isFutureDate = LastDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* Last date must be a future date";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*Last date can't be before first date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified number of days";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "GuestNumber", "DayNumber", "FirstDate", "LastDate" };

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
