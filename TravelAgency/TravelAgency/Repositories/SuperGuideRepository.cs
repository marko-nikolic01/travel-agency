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
    public class SuperGuideRepository : ISuperGuideRepository
    {
        private const string FilePath = "../../../Resources/Data/superGuides.csv";
        private readonly Serializer<SuperGuide> serializer;
        private List<SuperGuide> superGuides;

        public SuperGuideRepository()
        {
            serializer = new Serializer<SuperGuide>();
            superGuides = serializer.FromCSV(FilePath);
        }

        public List<SuperGuide> GetAll()
        {
            return superGuides;
        }
        public SuperGuide GetByUserId(int id)
        {
            foreach (SuperGuide superGuide in superGuides)
            {
                if(superGuide.GuidesId == id)
                {
                    return superGuide;
                }
            }
            return null;
        }

        public int NextId()
        {
            if (superGuides.Count < 1)
            {
                return 1;
            }
            return superGuides.Max(c => c.Id) + 1;
        }

        public void Save(SuperGuide superGuide)
        {
            SuperGuide oldSuperGuide = superGuides.Find(k => k.GuidesId == superGuide.GuidesId);
            if (oldSuperGuide != null)
            {
                oldSuperGuide.EndDate = superGuide.EndDate;
                oldSuperGuide.GuidesId = superGuide.GuidesId;
            }
            else
            {
                superGuide.Id = NextId();
                superGuides.Add(superGuide);
            }
            serializer.ToCSV(FilePath, superGuides);
        }

        public void SaveAll()
        {
            serializer.ToCSV(FilePath, superGuides);
        }
    }
}
