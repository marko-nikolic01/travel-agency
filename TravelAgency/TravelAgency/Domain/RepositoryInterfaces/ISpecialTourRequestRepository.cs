using System.Collections.Generic;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ISpecialTourRequestRepository
    {
        public List<SpecialTourRequest> GetAll();

        public SpecialTourRequest Save(SpecialTourRequest request);
    }
}
