using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumsForLocationViewModel : ViewModelBase
    {
        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        private Location selectedLocation;

        public string PageHeader { get; set; }

        public ObservableCollection<ForumWithStatsDTO> Forums { get; set; }
        public ForumWithStatsDTO SelectedForum { get; set; }

        public OwnerForumsForLocationViewModel(Location selectedLocation)
        {
            userService = new UserService();
            forumService = new ForumService();

            this.selectedLocation = selectedLocation;

            loggedInUser = userService.GetLoggedInUser();
            Forums = new ObservableCollection<ForumWithStatsDTO>(forumService.GetForumsWithStatsForLocation(this.selectedLocation));

            PageHeader = "Forum > " + this.selectedLocation.City;
        }
    }
}
