using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerNotificationsViewModel
    {
        public MyICommand MarkAsReadCommand { get; set; }
        public MyICommand MarkAllAsReadCommand { get; set; }

        private User loggedInUser;

        private UserService userService;
        private NotificationService notificationService;

        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }

        public OwnerNotificationsViewModel()
        {
            userService = new UserService();
            notificationService = new NotificationService();

            loggedInUser = userService.GetLoggedInUser();

            Notifications = new ObservableCollection<Notification>(notificationService.GetNotificationsByUser(loggedInUser));

            MarkAsReadCommand = new MyICommand(Execute_MarkAsReadCommand);
            MarkAllAsReadCommand = new MyICommand(Execute_MarkAllAsReadCommand);
        }

        public void Execute_MarkAsReadCommand()
        {
            if (SelectedNotification == null)
            {
                MessageBox.Show("Select a notification.");
                return;
            }

            notificationService.MarkAsSeen(SelectedNotification);
            UpdateNotifications();
        }

        public void Execute_MarkAllAsReadCommand()
        {
            notificationService.MarkAllAsSeen(Notifications.ToList());
            UpdateNotifications();
        }

        private void UpdateNotifications()
        {
            Notifications.Clear();

            foreach (var notification in notificationService.GetNotificationsByUser(loggedInUser))
            {
                Notifications.Add(notification);
            }
        }
    }
}
