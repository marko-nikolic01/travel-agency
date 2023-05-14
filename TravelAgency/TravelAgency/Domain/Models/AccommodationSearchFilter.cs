using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Models
{
    public class AccommodationSearchFilter : INotifyPropertyChanged
    {
        private string _nameFilter { get; set; }
        private string _countryFilter { get; set; }
        private string _cityFilter { get; set; }
        private string _typeFilter { get; set; }
        private int _guestNumberFilter { get; set; }
        private int _dayNumberFilter { get; set; }

        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                if (value != _nameFilter)
                {
                    _nameFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CountryFilter
        {
            get => _countryFilter;
            set
            {
                if (value != _countryFilter)
                {
                    _countryFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CityFilter
        {
            get => _cityFilter;
            set
            {
                if (value != _cityFilter)
                {
                    _cityFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TypeFilter
        {
            get => _typeFilter;
            set
            {
                if (value != _typeFilter)
                {
                    _typeFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public int GuestNumberFilter
        {
            get => _guestNumberFilter;
            set
            {
                if (value != _guestNumberFilter)
                {
                    _guestNumberFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DayNumberFilter
        {
            get => _dayNumberFilter;
            set
            {
                if (value != _dayNumberFilter)
                {
                    _dayNumberFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationSearchFilter()
        {
            NameFilter = "";
            CountryFilter = "";
            CityFilter = "";
            TypeFilter = "";
            GuestNumberFilter = 0;
            DayNumberFilter = 0;
        }

        
        public AccommodationSearchFilter(string nameFilter, string countryFilter, string cityFilter, string typeFilter, int guestNumberFilter, int dayNumberFilter)
        {
            NameFilter = nameFilter;
            CountryFilter = countryFilter;
            CityFilter = cityFilter;
            TypeFilter = typeFilter;
            GuestNumberFilter = guestNumberFilter;
            DayNumberFilter = dayNumberFilter;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
