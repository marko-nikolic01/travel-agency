using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class KeyPointService
    {
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public IKeyPointRepository IKeyPointRepository { get; set; }
        public KeyPointService()
        {
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IKeyPointRepository = Injector.Injector.CreateInstance<IKeyPointRepository>();
        }

        internal void UpdateKeyPoint(KeyPoint keyPoint)
        {
            IKeyPointRepository.UpdateKeyPoint(keyPoint);
        }
    }
}
