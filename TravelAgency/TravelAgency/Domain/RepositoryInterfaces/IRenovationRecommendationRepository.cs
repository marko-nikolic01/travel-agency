using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IRenovationRecommendationRepository
    {
        public List<RenovationRecommendation> GetAll();

        public int NextId();

        public RenovationRecommendation Save(RenovationRecommendation recommendation);
    }
}
