using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class TourRequestService
    {
        public ILocationRepository ILocationRepository { get; set; }
        public ITourRequestRepository ITourRequestRepository { get; set; }
        public TourRequestService()
        {
            ILocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            LinkLocationToRequests();
        }
        private void LinkLocationToRequests()
        {
            foreach(TourRequest request in ITourRequestRepository.GetAll()) 
            {
                if (request.Location == null) 
                {
                    request.Location = ILocationRepository.GetByID(request.LocationId);
                }
            }
        }
        public List<string> getCountries()
        {
            return ILocationRepository.GetAllCountries();
        }
        public List<string> getCities(string country)
        {
            return ILocationRepository.GetCitiesByCountry(country);
        }
        public List<TourRequest> GetRequestsByGuestId(int id)
        {
            return ITourRequestRepository.GetRequestsByGuestId(id);
        }
        public bool SaveRequest(string selectedCountry, string selectedCity, string language, string numberOfGuests, DateTime minDate, DateTime maxDate, string description, int guestId)
        {
            Location location = ILocationRepository.GetLocationForCountryAndCity(selectedCountry, selectedCity);
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
                ITourRequestRepository.Save(request);
                return true;
            }
            return false;
        }
    }
}
