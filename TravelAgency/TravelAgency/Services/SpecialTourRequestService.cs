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
        }
        public int SaveSpecialTourRequest(int guestId)
        {
            SpecialTourRequest specialRequest = new SpecialTourRequest();
            specialRequest.GuestId = guestId;
            specialRequest.Status = SpecialRequestStatus.Pending;
            specialRequest = ISpecialTourRequestRepository.Save(specialRequest);
            return specialRequest.Id;
        }
    }
}
