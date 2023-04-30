using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class TourRequestService
    {
        public ILocationRepository ILocationRepository { get; set; }
        public ITourRequestRepository ITourRequestRepository { get; set; }
        public IRequestAcceptedNotificationRepository IRequestAcceptedNotificationRepository { get; set; }
        
        public TourRequestService()
        {
            ILocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            IRequestAcceptedNotificationRepository = Injector.Injector.CreateInstance<IRequestAcceptedNotificationRepository>();
            LinkRequestLocation();
        }

        private void LinkRequestLocation()
        {
            foreach (var request in ITourRequestRepository.GetAll())
            {
                Location location = ILocationRepository.GetAll().Find(l => l.Id == request.LocationId);
                if (location != null)
                {
                    request.Location = location;
                }
            }
        }
        public List<TourRequest> GetPendings()
        {
            List<TourRequest> pendings = new List<TourRequest>();
            foreach (var request in ITourRequestRepository.GetAll())
            {
                if(request.Status == RequestStatus.Pending)
                {
                    pendings.Add(request);
                }
            }
            return pendings;
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

        public void Subscribe(IObserver observer)
        {
            ITourRequestRepository.Subscribe(observer);
        }
        public void UpdateRequestStatus(TourRequest request)
        {
            ITourRequestRepository.UpdateRequestStatus(request);
        }
        public void SaveNotification(RequestAcceptedNotification requestAcceptedNotification)
        {
            IRequestAcceptedNotificationRepository.Save(requestAcceptedNotification);
        }
    }
}
