using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1ReadWriteForumViewModel2 : ViewModelBase, INotifyPropertyChanged
    {
        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand WriteCommentCommand { get; private set; }

        private ForumService _forumService;

        public User Guest { get; set; }
        private Forum _forum;
        private Comment _comment;
        private ObservableCollection<Comment> _comments;

        public Forum Forum
        {
            get => _forum;
            set
            {
                if (value != _forum)
                {
                    _forum = value;
                    OnPropertyChanged();
                }
            }
        }

        public Comment Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Comment> Comments
        {
            get => _comments;
            set
            {
                if (value != _comments)
                {
                    _comments = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1ReadWriteForumViewModel2(MyICommand<string> navigationCommand, User guest, Forum forum)
        {
            NavigationCommand = navigationCommand;
            WriteCommentCommand = new MyICommand(OnWriteComment);

            _forumService = new ForumService();

            Guest = guest;
            Forum = forum;

            InitializeData();
        }

        private void InitializeData()
        {
            InitializeComments();
        }

        private void InitializeComments()
        {
            Comment = new Comment(Forum, Guest);
            Comments = new ObservableCollection<Comment>(Forum.Comments);
        }

        private void OnWriteComment()
        {
            if (Comment.IsValid)
            {
                _forumService.PostCommentByGuest(Forum, Comment);
                InitializeComments();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
