using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumOverviewViewModel : ViewModelBase
    {
        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        public Forum SelectedForum { get; set; }

        public string PageHeader { get; set; }

        public Page BackPage { get; set; }

        public ObservableCollection<CommentWithDataDTO> Comments { get; set; }

        public OwnerForumOverviewViewModel(Forum selectedForum)
        {
            userService = new UserService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();

            SelectedForum = selectedForum;

            PageHeader = "Forum > " + selectedForum.Location.City + " > " + selectedForum.Title;

            Comments = new ObservableCollection<CommentWithDataDTO>(forumService.GetCommentsWithDataByForum(selectedForum, loggedInUser));
            BackPage = new OwnerForumsForLocation(new OwnerForumsForLocationViewModel(selectedForum.Location));
        }
    }
}
