using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerRenovationsViewModel : ViewModelBase
    {
        private UserService userService;
        private RenovationService renovationService;

        private User loggedInUser;

        private NavigationService navigationService;

        public AccommodationRenovation SelectedScheduledRenovation { get; set; }

        public MyICommand ScheduleRenovationCommand { get; set; }

        public ObservableCollection<AccommodationRenovation> ScheduledRenovations { get; set; }
        public ObservableCollection<AccommodationRenovation> PastRenovations { get; set; }

        public OwnerRenovationsViewModel(NavigationService navigationService)
        {
            userService = new UserService();
            renovationService = new RenovationService();

            loggedInUser = userService.GetLoggedInUser();

            this.navigationService = navigationService;

            ScheduleRenovationCommand = new MyICommand(Execute_ScheduleRenovationCommand);

            ScheduledRenovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetScheduledRenovationsByOwner(loggedInUser));
            PastRenovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetPastRenovationsByOwner(loggedInUser));
        }

        private void Execute_ScheduleRenovationCommand()
        {
            throw new NotImplementedException();
        }

        private void Execute_CancelRenovationCommand()
        {
            if (SelectedScheduledRenovation == null)
            {
                MessageBox.Show("Select a renovation!");
                return;
            }

            renovationService.CancelRenovation(SelectedScheduledRenovation);
        }
    }
}
