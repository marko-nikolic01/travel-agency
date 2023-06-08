using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    internal class NotificationService
    {
        public IUserRepository UserRepository { get; set; }
        public INotificationRepository NotificationRepository { get; set; }

        public NotificationService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            NotificationRepository = Injector.Injector.CreateInstance<INotificationRepository>();
            NotificationRepository.LinkUsers(UserRepository.GetAll());
        }

        public List<Notification> GetNotificationsByUser(User user)
        {
            return NotificationRepository.GetByUser(user);
        }

        public void NotifyReservationMoveRequestAccepted(User guest)
        {
            string notificationText = "Vaš zahtev za izmenu rezervacije je prihvaćen.";
            Notification notification = new Notification(guest, notificationText);
            NotificationRepository.Save(notification);
        }

        public void NotifyReservationMoveRequestRejected(User guest)
        {
            string notificationText = "Vaš zahtev za izmenu rezervacije je odbijen.";
            Notification notification = new Notification(guest, notificationText);
            NotificationRepository.Save(notification);
        }

        public void MarkAllAsSeen(List<Notification> notifications)
        {
            foreach (Notification notification in notifications)
            {
                notification.Seen = true;                
            }
            NotificationRepository.SaveAll();
        }
        
    }
}
