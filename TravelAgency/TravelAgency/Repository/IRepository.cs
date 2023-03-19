using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Repository
{
    public interface IRepository<T>
    {
        List<T> GetAll();

        T GetById(int id);

        int NextId();

        T Save(T entity);

        void SaveAll(IEnumerable<T> entities);

        void DeleteById(int id);

        void Delete(T entity);

        void DeleteAll();
    }
}
