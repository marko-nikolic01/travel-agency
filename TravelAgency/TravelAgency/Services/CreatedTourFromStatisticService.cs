using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class CreatedTourFromStatisticService
    {
        public INewTourNotificationRepository INewTourNotificationRepository { get; set; }
        public IUserRepository IUserRepository { get; set; }
        public ITourRequestRepository ITourRequestRepository { get; set; }
        public ITourRepository ITourRepository { get; set; }
        public CreatedTourFromStatisticService() 
        {
            INewTourNotificationRepository = Injector.Injector.CreateInstance<INewTourNotificationRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            ITourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            LinkTourNotifications();
        }
        private void LinkTourNotifications()
        {
            foreach(NewTourNotification notification in INewTourNotificationRepository.GetAll()) 
            {
                if (notification.Tour == null)
                {
                    notification.Tour = ITourRepository.GetById(notification.TourId);
                }
            }
        }
        public void MakeNotifications(Tour tour, string notificationType)
        {
            foreach (Domain.Models.User user in IUserRepository.GetGuests2())
            {
                if (notificationType == "language")
                {
                    if (ITourRequestRepository.ShouldNotifyGuestForLanguage(user.Id, tour.Language))
                    {
                        NewTourNotification createdTour = new NewTourNotification(tour.Id, user.Id, false, tour, true, false);
                        INewTourNotificationRepository.Save(createdTour);
                    }
                }
                if (notificationType == "location")
                {
                    if (ITourRequestRepository.ShouldNotifyGuestForLocation(user.Id, tour.Location.Id))
                    {
                        NewTourNotification createdTour = new NewTourNotification(tour.Id, user.Id, false, tour, false, true);
                        INewTourNotificationRepository.Save(createdTour);
                    }
                }
            }
        }
        public bool NewTourNotificationExists(int guestId)
        {
            return INewTourNotificationRepository.NewTourNotificationExists(guestId);
        }
        public List<NewTourNotification> GetNewTourNotifications(int guestId)
        {
            return INewTourNotificationRepository.GetNewTourNotifications(guestId);
        }
        public void UpdateNotification(NewTourNotification notification)
        {
            INewTourNotificationRepository.Update(notification);
        }
    }
}
