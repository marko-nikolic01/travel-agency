using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ISuperGuideRepository
    {
        public List<SuperGuide> GetAll();

        public void Save(SuperGuide superGuide);
        public SuperGuide GetByUserId(int id);
    }
}
