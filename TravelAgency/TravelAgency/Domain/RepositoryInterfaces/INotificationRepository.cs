using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface INotificationRepository
    {
        public List<Notification> GetByUser(User user);
        public Notification Save(Notification notification);
        public void SaveAll();
        public int NextId();
        public void LinkUsers(List<User> users);
    }
}
