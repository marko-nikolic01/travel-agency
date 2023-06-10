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
            CheckIfRequestAccepted();
        }
        private void LinkSpecialTourRequests()
        {
            foreach(SpecialTourRequest specialRequest in ISpecialTourRequestRepository.GetAll()) 
            {
                if(specialRequest.TourRequests.Count == 0)
                {
                    specialRequest.TourRequests = new System.Collections.ObjectModel.ObservableCollection<TourRequest>(ITourRequestRepository.GetBySpecialRequestId(specialRequest.Id));
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
        private void CheckIfRequestAccepted()
        {
            foreach (SpecialTourRequest specialRequest in ISpecialTourRequestRepository.GetAll())
            {
                if (specialRequest.Status != SpecialRequestStatus.Accepted)
                {
                    bool save = true;
                    foreach (TourRequest request in specialRequest.TourRequests)
                    {
                        if (request.Status != RequestStatus.Accepted)
                        {
                            save = false;
                            break;
                        }
                    }
                    if (save)
                    {
                        specialRequest.Status = SpecialRequestStatus.Accepted;
                        ISpecialTourRequestRepository.Update(specialRequest);
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
            return ISpecialTourRequestRepository.GetPendings();
        }
        public int GetNumberOfAllRequests(int guestId)
        {
            return ITourRequestRepository.GetNumberOfAllRequests(guestId);
        }
    }
}
