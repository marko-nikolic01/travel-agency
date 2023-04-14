using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.Services
{
    public class TourRequestService
    {
        private LocationRepository locationRepository;
        public TourRequestService() 
        {
            locationRepository = new LocationRepository();
        }
        public List<string> getCountries()
        {
            return locationRepository.GetAllCountries();
        }
        public List<string> getCities(string country)
        {
            return locationRepository.GetCitiesByCountry(country);
        }

        public bool SaveRequest(string selectedCountry, string selectedCity, string language, string numberOfGuests, DateTime minDate, DateTime maxDate, string description, int guestId)
        {
            Location location = locationRepository.GetLocationForCountryAndCity(selectedCountry, selectedCity);
            TourRequest request = new TourRequest();
            if (request.Valid(language, numberOfGuests))
            {
                request.Location = location;
                request.LocationId = location.Id;
                request.MinDate = minDate;
                request.MaxDate = maxDate;
                request.Description = description;
                request.GuestId = guestId;
                request.Status = RequestStatus.Pending;
                TourRequestRepository repository = new TourRequestRepository();
                repository.Save(request);
                return true;
            }
            return false;
        }
    }
}
