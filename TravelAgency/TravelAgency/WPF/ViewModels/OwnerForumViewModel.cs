using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumViewModel : ViewModelBase
    {

        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        public List<Location> Locations { get; set; }
        public Location SelectedLocation { get; set; }

        public OwnerForumViewModel()
        {
            userService = new UserService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();

            Locations = new List<Location>(forumService.GetLocationsForForumsByOwner(loggedInUser));
        }
    }
}
