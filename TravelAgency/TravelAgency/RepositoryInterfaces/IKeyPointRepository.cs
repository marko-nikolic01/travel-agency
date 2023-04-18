using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IKeyPointRepository
    {
        public List<KeyPoint> GetAll();
        public KeyPoint Save(KeyPoint keyPoint);
        public void UpdateKeyPoint(KeyPoint keyPoint);
        public KeyPoint GetById(int id);
        public List<KeyPoint> GetByTourOccurrence(int id);
    }
}
