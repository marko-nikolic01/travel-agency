using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IPhotoRepository
    {
        public List<Photo> GetAll();

        public Photo Save(Photo photo);
        public List<Photo> GetByTour(int id);
    }
}
