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
    internal class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationRecommendations.csv";
        private readonly Serializer<RenovationRecommendation> serializer;
        private List<RenovationRecommendation> recommendations;

        public RenovationRecommendationRepository()
        {
            serializer = new Serializer<RenovationRecommendation>();
            recommendations = serializer.FromCSV(FilePath);
        }

        public List<RenovationRecommendation> GetAll()
        {
            return recommendations;
        }

        public int NextId()
        {
            if (recommendations.Count < 1)
            {
                return 1;
            }
            return recommendations.Max(c => c.Id) + 1;
        }

        public RenovationRecommendation Save(RenovationRecommendation recommendation)
        {
            recommendation.Id = NextId();
            recommendations.Add(recommendation);
            serializer.ToCSV(FilePath, recommendations);
            return recommendation;
        }
    }
}
