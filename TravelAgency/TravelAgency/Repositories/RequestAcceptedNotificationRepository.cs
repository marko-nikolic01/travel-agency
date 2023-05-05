using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class RequestAcceptedNotificationRepository : IRequestAcceptedNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/requestAcceptedNotifications.csv";
        private readonly Serializer<RequestAcceptedNotification> _serializer;
        private List<RequestAcceptedNotification> notifications;

        public RequestAcceptedNotificationRepository()
        {
            _serializer = new Serializer<RequestAcceptedNotification>();
            notifications = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            if (notifications.Count == 0)
            {
                return 1;
            }
            return notifications[notifications.Count - 1].Id + 1;
        }

        public List<RequestAcceptedNotification> GetAll()
        {
            return notifications;
        }

        public RequestAcceptedNotification Save(RequestAcceptedNotification notification)
        {
            notification.Id = NextId();
            notifications.Add(notification);
            _serializer.ToCSV(FilePath, notifications);
            return notification;
        }
        public void Update(RequestAcceptedNotification notification)
        {
            RequestAcceptedNotification oldNotification = notifications.Find(t => t.Id == notification.Id);
            oldNotification.IsSeen = notification.IsSeen;
            _serializer.ToCSV(FilePath, notifications);
        }
        public List<RequestAcceptedNotification> GetNewAcceptedRequests(int guestId)
        {
            List<RequestAcceptedNotification> result = new List<RequestAcceptedNotification>();
            foreach (RequestAcceptedNotification notification in notifications)
            {
                if (!notification.IsSeen && guestId == notification.GuestId)
                {
                    result.Add(notification);
                }
            }
            return result;
        }
        public bool NewAcceptedRequestExists(int guestId)
        {
            foreach (RequestAcceptedNotification notification in notifications)
            {
                if (!notification.IsSeen && guestId == notification.GuestId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
