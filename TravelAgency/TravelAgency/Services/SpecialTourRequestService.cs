using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class SpecialTourRequestService
    {

        public ITourRequestRepository ITourRequestRepository { get; set; }
        public ISpecialTourRequestRepository ISpecialTourRequestRepository { get; set; }
        public ILocationRepository ILocationRepository { get; set; }
        public SpecialTourRequestService() 
        {
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            ISpecialTourRequestRepository = Injector.Injector.CreateInstance<ISpecialTourRequestRepository>();
            ILocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            LinkSpecialTourRequests();
            LinkRequestLocation();
            UpdateSpecialRequestStatus();
        }
        private void LinkSpecialTourRequests()
        {
            foreach(SpecialTourRequest specialRequest in ISpecialTourRequestRepository.GetAll()) 
            {
                if(specialRequest.TourRequests.Count == 0)
                {
                    specialRequest.TourRequests = ITourRequestRepository.GetBySpecialRequestId(specialRequest.Id);
                }
            }
        }
        private void LinkRequestLocation()
        {
            foreach (var request in ITourRequestRepository.GetSpecialRequests())
            {
                Location location = ILocationRepository.GetAll().Find(l => l.Id == request.LocationId);
                if (location != null)
                {
                    request.Location = location;
                }
            }
        }
        private void UpdateSpecialRequestStatus()
        {
            foreach (SpecialTourRequest specialRequest in ISpecialTourRequestRepository.GetAll())
            {
                foreach(TourRequest request in specialRequest.TourRequests)
                {
                    if(request.Status == RequestStatus.Invalid)
                    {
                        specialRequest.Status = SpecialRequestStatus.Invalid;
                        break;
                    }
                }
            }
        }
        public int SaveSpecialTourRequest(int guestId)
        {
            SpecialTourRequest specialRequest = new SpecialTourRequest();
            specialRequest.GuestId = guestId;
            specialRequest.Status = SpecialRequestStatus.Pending;
            specialRequest = ISpecialTourRequestRepository.Save(specialRequest);
            return specialRequest.Id;
        }

        public List<SpecialTourRequest>? GetSpecialRequestForGuest(int guestId)
        {
            return ISpecialTourRequestRepository.GetByGuestId(guestId);
        }
        public List<SpecialTourRequest>? GetOpenSpecialRequest()
        {
            return ISpecialTourRequestRepository.GetAll();
        }
        public int GetNumberOfAllRequests(int guestId)
        {
            return ITourRequestRepository.GetNumberOfAllRequests(guestId);
        }
    }
}
