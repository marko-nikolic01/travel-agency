using System;
using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class NewTourNotificationRepository : INewTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/newtournotifications.csv";
        private readonly Serializer<NewTourNotification> _serializer;
        private List<NewTourNotification> notifications;

        public NewTourNotificationRepository()
        {
            _serializer = new Serializer<NewTourNotification>();
            notifications = _serializer.FromCSV(FilePath);
        }

        public List<NewTourNotification> GetAll()
        {
            return notifications;
        }

        public NewTourNotification Save(NewTourNotification tour)
        {
            notifications.Add(tour);
            _serializer.ToCSV(FilePath, notifications);
            return tour;
        }
        public void Update(NewTourNotification notification)
        {
            NewTourNotification oldNotification = notifications.Find(t => t.TourId == notification.TourId && t.GuestId == notification.GuestId);
            oldNotification.Seen = notification.Seen;
            _serializer.ToCSV(FilePath, notifications);
        }
        public bool NewTourNotificationExists(int guestId)
        {
            foreach (NewTourNotification notification in notifications)
            {
                if (notification.GuestId == guestId && !notification.Seen)
                    return true;
            }
            return false;
        }
        public List<NewTourNotification> GetNewTourNotifications(int guestId)
        {
            List<NewTourNotification> result = new List<NewTourNotification>();
            foreach (NewTourNotification notification in notifications)
            {
                if (notification.GuestId == guestId && !notification.Seen)
                    result.Add(notification);
            }
            return result;
        }
    }
}
