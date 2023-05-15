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
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationService accommodationService;

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public OwnerManageAccommodationsViewModel()
        {

            userService = new UserService();
            accommodationService = new AccommodationService();

            LoggedInUser = userService.GetLoggedInUser();

            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetByOwner(LoggedInUser));
        }
    }
}
