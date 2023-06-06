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
        private AccommodationService accommodationService;

        private User loggedInUser;

        private NavigationService navigationService;

        public AccommodationRenovation? SelectedScheduledRenovation { get; set; }

        public MyICommand ScheduleRenovationCommand { get; set; }
        public MyICommand CancelRenovationCommand { get; set; }

        public ObservableCollection<AccommodationRenovation> ScheduledRenovations { get; set; }
        public ObservableCollection<AccommodationRenovation> PastRenovations { get; set; }

        public OwnerRenovationsViewModel(NavigationService navigationService)
        {
            userService = new UserService();
            renovationService = new RenovationService();
            accommodationService = new AccommodationService();

            loggedInUser = userService.GetLoggedInUser();

            this.navigationService = navigationService;

            ScheduleRenovationCommand = new MyICommand(Execute_ScheduleRenovationCommand);
            CancelRenovationCommand = new MyICommand(Execute_CancelRenovationCommand);

            ScheduledRenovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetScheduledRenovationsByOwner(loggedInUser));
            PastRenovations = new ObservableCollection<AccommodationRenovation>(renovationService.GetPastRenovationsByOwner(loggedInUser));
        }

        private void Execute_ScheduleRenovationCommand()
        {
            MessageBox.Show("Ne radi još :(");
        }

        private void Execute_CancelRenovationCommand()
        {
            if (SelectedScheduledRenovation == null)
            {
                MessageBox.Show("Select a renovation!");
                return;
            }

            if (renovationService.CanRenovationBeCancelled(SelectedScheduledRenovation))
            {
                var result = MessageBox.Show("Are you sure you want to cancel this renovation?", "Canceling renovation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    renovationService.CancelRenovation(SelectedScheduledRenovation);
                    UpdateScheduledRenovations();
                }
            }
            else
            {
                MessageBox.Show("Selected renovation can't be cancelled because it's less than 5 days due.");
            }            
        }

        private void UpdateScheduledRenovations()
        {
            ScheduledRenovations.Clear();

            foreach (var renovation in renovationService.GetScheduledRenovationsByOwner(loggedInUser))
            {
                ScheduledRenovations.Add(renovation);
            }
        }

        public bool OwnerHasAccommodations()
        {
            return accommodationService.GetActiveByOwner(loggedInUser).Count > 0;
        }
    }
}
