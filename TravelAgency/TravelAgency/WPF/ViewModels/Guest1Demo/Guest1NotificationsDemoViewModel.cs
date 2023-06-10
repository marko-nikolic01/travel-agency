using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1NotificationsDemoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;

        public DemoInstruction Instruction
        {
            get => _instruction;
            set
            {
                if (value != _instruction)
                {
                    _instruction = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public Guest1NotificationsDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            string text = "Ovde možete videti sve svoje notifikacije.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text);    Delay(3000);
        }

        private void InitializeData()
        {
            InitializeNotifications();
        }

        private void InitializeNotifications()
        {
            Notifications = new ObservableCollection<Notification>();
            Notification notification = new Notification();
            notification.Text = "Notifikacija...";
            notification.Seen = false;
            Notifications.Add(notification); 
            notification = new Notification();
            notification.Text = "Notifikacija...";
            notification.Seen = true;
            Notifications.Add(notification);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
