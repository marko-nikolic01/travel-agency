using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/notifications.csv";

        private readonly Serializer<Notification> serializer;

        private List<Notification> notifications;

        public NotificationRepository()
        {
            serializer = new Serializer<Notification>();
            notifications = serializer.FromCSV(FilePath);
        }

        public void LinkUsers(List<User> users)
        {
            foreach (Notification notification in notifications)
            {
                foreach (User user in users)
                {
                    if (notification.User.Id == user.Id)
                    {
                        notification.User = user;
                        break;
                    }
                }
            }
        }

        public List<Notification> GetByUser(User user)
        {
            List<Notification> notificationsByUser = new List<Notification>();
            foreach (Notification notification in notifications)
            {
                if (notification.User == user)
                {
                    notificationsByUser.Add(notification);
                }
            }
            return notificationsByUser;
        }

        public int NextId()
        {
            if (notifications.Count < 1)
            {
                return 1;
            }
            return notifications.Max(c => c.Id) + 1;
        }

        public Notification Save(Notification notification)
        {
            notification.Id = NextId();
            notifications.Add(notification);
            serializer.ToCSV(FilePath, notifications);
            return notification;
        }

        public void SaveAll()
        {
            serializer.ToCSV(FilePath, notifications);
        }
    }
}
