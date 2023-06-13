using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1NotificationsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private NotificationService _notificationService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public User Guest { get; set; }
        private ObservableCollection<Notification> _notifications;

        public ObservableCollection<Notification> Notifications
        {
            get => _notifications;
            set
            {
                if (value != _notifications)
                {
                    _notifications = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1NotificationsViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;

            _notificationService = new NotificationService();

            Guest = guest;
            InitializeData();
        }

        private void InitializeData()
        {
            InitializeNotifications();
        }

        private void InitializeNotifications()
        {
            List<Notification> notifications = _notificationService.GetNotificationsByUser(Guest);
            notifications = ReverseNotifications(notifications);
            Notifications = new ObservableCollection<Notification>(notifications);
        }

        private List<Notification> ReverseNotifications(List<Notification> notifications)
        {
            List<Notification> reversedNotificationss = new List<Notification>();
            for (int count = notifications.Count() - 1; count >= 0; count--)
            {
                reversedNotificationss.Add(notifications[count]);
            }
            return reversedNotificationss;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
