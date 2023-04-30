using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IRequestAcceptedNotificationRepository
    {
        public List<RequestAcceptedNotification> GetAll();

        public RequestAcceptedNotification Save(RequestAcceptedNotification notification);
    }
}
