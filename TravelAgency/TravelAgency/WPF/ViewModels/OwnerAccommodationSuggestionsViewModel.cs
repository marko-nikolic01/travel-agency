using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAccommodationSuggestionsViewModel : ViewModelBase
    {
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationManagingSuggestionsService accommodationManagingSuggestionsService;
        private AccommodationService accommodationService;

        public MyICommand DeleteSelectedAccommodationCommand { get; set; }

        public ObservableCollection<LocationWithNumberOfBusyDaysDTO> Top3BestLocations { get; set; }
        public ObservableCollection<AccommodationWithNumberOfDaysBusyDTO> Top3WorstAccommodations { get; set; }

        public LocationWithNumberOfBusyDaysDTO SelectedLocation { get; set; }
        public AccommodationWithNumberOfDaysBusyDTO SelectedAccommodation { get; set; }

        public OwnerAccommodationSuggestionsViewModel()
        {
            userService = new UserService();
            accommodationManagingSuggestionsService = new AccommodationManagingSuggestionsService();
            accommodationService = new AccommodationService();

            LoggedInUser = userService.GetLoggedInUser();

            DeleteSelectedAccommodationCommand = new MyICommand(Execute_DeleteSelectedAccommodationCommand);

            Top3BestLocations = new ObservableCollection<LocationWithNumberOfBusyDaysDTO>(accommodationManagingSuggestionsService.GetTop3BestLocationsByOwner(LoggedInUser));
            Top3WorstAccommodations = new ObservableCollection<AccommodationWithNumberOfDaysBusyDTO>(accommodationManagingSuggestionsService.GetTop3WorstAccommodationsByOwner(LoggedInUser));
        }

        public void Execute_DeleteSelectedAccommodationCommand()
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Select an accommodation.");
                return;
            }

            var response = MessageBox.Show("Are you sure you want to remove this accommodation:\n" + SelectedAccommodation.Accommodation.Name, "Removing accommodation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (response == MessageBoxResult.Yes)
            {
                accommodationService.Delete(SelectedAccommodation.Accommodation);
                UpdateAccommodationsList();
            }
        }

        private void DeleteSelectedAccommodationCommand_Click(object sender, EventArgs e)
        {
            Execute_DeleteSelectedAccommodationCommand();
        }

        private void UpdateAccommodationsList()
        {
            Top3WorstAccommodations.Clear();

            foreach (var accommodation in accommodationManagingSuggestionsService.GetTop3WorstAccommodationsByOwner(LoggedInUser))
            {
                Top3WorstAccommodations.Add(accommodation);
            }
        }
    }
}
