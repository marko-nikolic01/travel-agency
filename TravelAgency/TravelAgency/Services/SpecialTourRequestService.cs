using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    class SpecialTourRequestService
    {

        public ITourRequestRepository ITourRequestRepository { get; set; }
        public ISpecialTourRequestRepository ISpecialTourRequestRepository { get; set; }
        public SpecialTourRequestService() 
        {
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            ISpecialTourRequestRepository = Injector.Injector.CreateInstance<ISpecialTourRequestRepository>();
            LinkTourRequests();
        }
        private void LinkTourRequests()
        {
            foreach(SpecialTourRequest specialRequest in ISpecialTourRequestRepository.GetAll()) 
            {
                if(specialRequest.TourRequests.Count == 0)
                {
                    specialRequest.TourRequests = ITourRequestRepository.GetBySpecialRequestId(specialRequest.Id);
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
    }
}
