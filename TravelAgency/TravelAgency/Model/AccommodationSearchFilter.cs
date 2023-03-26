using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class AccommodationSearchFilter
    {
        public string NameFilter { get; set; }
        public string CountryFilter { get; set; }
        public string CityFilter { get; set; }
        public string TypeFilter { get; set; }
        public int GuestNumberFilter { get; set; }
        public int DayNumberFilter { get; set; }

        public AccommodationSearchFilter(string nameFilter, string countryFilter, string cityFilter, string typeFilter, int guestNumberFilter, int dayNumberFilter)
        {
            NameFilter = nameFilter;
            CountryFilter = countryFilter;
            CityFilter = cityFilter;
            TypeFilter = typeFilter;
            GuestNumberFilter = guestNumberFilter;
            DayNumberFilter = dayNumberFilter;
        }
    }
}
