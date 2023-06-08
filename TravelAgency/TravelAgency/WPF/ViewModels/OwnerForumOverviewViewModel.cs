using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerForumOverviewViewModel : ViewModelBase
    {
        public MyICommand DislikeCommentCommand { get; set; }

        private User loggedInUser;

        private UserService userService;
        private ForumService forumService;

        public Forum SelectedForum { get; set; }

        public string PageHeader { get; set; }

        public Page BackPage { get; set; }

        public ObservableCollection<CommentWithDataDTO> Comments { get; set; }
        public CommentWithDataDTO SelectedComment { get; set; }

        public OwnerForumOverviewViewModel(Forum selectedForum)
        {
            userService = new UserService();
            forumService = new ForumService();

            loggedInUser = userService.GetLoggedInUser();

            SelectedForum = selectedForum;

            PageHeader = "Forum > " + selectedForum.Location.City + " > " + selectedForum.Title;

            Comments = new ObservableCollection<CommentWithDataDTO>(forumService.GetCommentsWithDataByForum(SelectedForum, loggedInUser));
            BackPage = new OwnerForumsForLocation(new OwnerForumsForLocationViewModel(selectedForum.Location));

            DislikeCommentCommand = new MyICommand(Execute_DislikeComment);
        }

        private void Execute_DislikeComment()
        {
            //var result = MessageBox.Show("Are you sure you want to dislike the comment?", "Disliking comment", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (SelectedForum == null)
            {
                MessageBox.Show("Select a comment.");
            }

            if (!forumService.CanOwnerDislikeComment(loggedInUser, SelectedComment.Comment))
            {
                MessageBox.Show("You cannot dislike this comment.");
                return;
            }

            forumService.DislikeComment(SelectedComment.Comment, loggedInUser);
            UpdateComments();
        }

        private void UpdateComments()
        {
            Comments.Clear();

            foreach (var item in forumService.GetCommentsWithDataByForum(SelectedForum, loggedInUser))
            {
                Comments.Add(item);
            }
        }
    }
}
