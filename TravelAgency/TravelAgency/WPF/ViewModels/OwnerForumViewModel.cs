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

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumViewModel : ViewModelBase
    {
        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;
        private LocationService locationService;

        public ObservableCollection<ForumWithStatsDTO> Forums { get; set; }
        public List<Location> Locations { get; set; }

        public OwnerForumViewModel()
        {
            userService = new UserService();
            locationService = new LocationService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();

            Forums = new ObservableCollection<ForumWithStatsDTO>(forumService.GetForumsWithStatsByOwner(loggedInUser));
            Locations = new List<Location>(locationService.GetLocationsByOwner(loggedInUser));

            MessageBox.Show(Forums.Count.ToString());
        }
    }
}
