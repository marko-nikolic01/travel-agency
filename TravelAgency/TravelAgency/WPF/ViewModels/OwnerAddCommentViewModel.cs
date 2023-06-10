using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAddCommentViewModel : ViewModelBase
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand AddCommentCommand { get; set; }

        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        public string PageHeader { get; set; }

        public OwnerForumOverviewView BackPage { get; set; }

        private NavigationService navigationService;

        public Forum SelectedForum { get; set; }
        private string commentText;
        public string CommentText
        {
            get { return commentText; }
            set
            {
                commentText = value;
                OnPropertyChanged(nameof(CommentText));
            }
        }

        public OwnerAddCommentViewModel(Forum selectedForum, OwnerForumOverviewView backPage, NavigationService navigationService)
        {
            userService = new UserService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();

            this.navigationService = navigationService;

            SelectedForum = selectedForum;
            BackPage = backPage;

            PageHeader = "Forum > " + selectedForum.Location.City + " > " + selectedForum.Title + " > Add comment";
            CommentText = string.Empty;

            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            AddCommentCommand = new MyICommand(Execute_AddCommentCommand);
        }

        private void Execute_AddCommentCommand()
        {
            if (CommentText == string.Empty)
            {
                MessageBox.Show("You must write a comment.", "No comment given", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            forumService.AddCommentToForum(CommentText, SelectedForum, loggedInUser);
            BackPage = new OwnerForumOverviewView(new OwnerForumOverviewViewModel(SelectedForum));
            MessageBox.Show("Comment submited successfully.", "Comment submission", MessageBoxButton.OK, MessageBoxImage.Information);
            Execute_NavigateBackCommand();
        }

        private void Execute_NavigateBackCommand()
        {
            this.navigationService.Navigate(BackPage);
        }
    }
}
