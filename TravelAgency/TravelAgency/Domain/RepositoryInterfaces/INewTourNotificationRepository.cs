using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface INewTourNotificationRepository
    {
        public List<NewTourNotification> GetAll();
        public void Update(NewTourNotification notification);
        public NewTourNotification Save(NewTourNotification tour);
        public bool NewTourNotificationExists(int guestId);
        public List<NewTourNotification> GetNewTourNotifications(int guestId);
    }
}
