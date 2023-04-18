using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.RepositoryInterfaces
{
    public interface IPhotoRepository
    {
        public List<Photo> GetAll();

        public Photo Save(Photo photo);
        public List<Photo> GetByTour(int id);
    }
}
