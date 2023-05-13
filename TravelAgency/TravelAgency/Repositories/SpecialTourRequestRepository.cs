using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    class SpecialTourRequestRepository : ISpecialTourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/specialtourrequests.csv";
        private readonly Serializer<SpecialTourRequest> _serializer;
        private List<SpecialTourRequest> specialRequests;

        public SpecialTourRequestRepository()
        {
            _serializer = new Serializer<SpecialTourRequest>();
            specialRequests = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (specialRequests.Count == 0)
            {
                return 1;
            }
            return specialRequests[specialRequests.Count - 1].Id + 1;
        }

        public List<SpecialTourRequest> GetAll()
        {
            return specialRequests;
        }
        public List<SpecialTourRequest> GetByGuestId(int guestId)
        {
            List<SpecialTourRequest> result = new List<SpecialTourRequest>();
            foreach(SpecialTourRequest request in specialRequests) 
            {
                if(request.GuestId == guestId) 
                {
                    result.Add(request);
                }
            }
            return result;
        }
        public SpecialTourRequest Save(SpecialTourRequest specialTourRequest)
        {
            specialTourRequest.Id = NextId();
            specialRequests.Add(specialTourRequest);
            _serializer.ToCSV(FilePath, specialRequests);
            return specialTourRequest;
        }
    }
}
