using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Pages;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerManageAccommodationsViewModel : ViewModelBase
    {
        public MyICommand DeleteSelectedAccommodationCommand { get; set; }
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationService accommodationService;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public OwnerManageAccommodationsViewModel()
        {
            userService = new UserService();
            accommodationService = new AccommodationService();

            DeleteSelectedAccommodationCommand = new MyICommand(Execute_DeleteSelectedAccommodationCommand);

            LoggedInUser = userService.GetLoggedInUser();

            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetActiveByOwner(LoggedInUser));
        }

        public void Execute_DeleteSelectedAccommodationCommand()
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Select an accommodation.", "No accommodation selected.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var response = MessageBox.Show("Are you sure you want to remove this accommodation:\n" + SelectedAccommodation.Name + "?", "Removing accommodation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (response == MessageBoxResult.Yes)
            {
                accommodationService.Delete(SelectedAccommodation);
                UpdateAccommodationsList();
            }
        }

        private void UpdateAccommodationsList()
        {
            Accommodations.Clear();

            foreach (var accommodation in accommodationService.GetActiveByOwner(LoggedInUser))
            {
                Accommodations.Add(accommodation);
            }
        }
    }
}
