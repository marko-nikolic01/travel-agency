using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IAccommodationPhotoRepository
    {
        public List<AccommodationPhoto> GetAll();
        public AccommodationPhoto Save(AccommodationPhoto photo);
        public void SaveAll(List<AccommodationPhoto> photos);
        public int NextId();
    }
}
