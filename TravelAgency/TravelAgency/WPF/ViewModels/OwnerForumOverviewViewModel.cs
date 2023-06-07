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

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumOverviewViewModel : ViewModelBase
    {
        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        private Forum selectedForum;

        public string PageHeader { get; set; }

        public Page BackPage { get; set; }

        public ObservableCollection<CommentWithDataDTO> Comments { get; set; }

        public OwnerForumOverviewViewModel(Forum selectedForum, Page backPage)
        {
            userService = new UserService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();

            this.selectedForum = selectedForum;
            this.BackPage = backPage;

            PageHeader = "Forums > " + selectedForum.Location.City + " > " + selectedForum.Title;

            Comments = new ObservableCollection<CommentWithDataDTO>(forumService.GetCommentsWithDataByForum(selectedForum, loggedInUser));
        }
    }
}
