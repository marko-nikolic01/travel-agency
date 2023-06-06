using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumViewModel : ViewModelBase
    {
        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        public OwnerForumViewModel()
        {
            userService = new UserService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();
        }
    }
}
