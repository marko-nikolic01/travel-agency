using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1OpenForumViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ForumService _forumService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand OpenForumCommand { get; private set; }

        public User Guest { get; set; }
        private Location _location;
        private Forum _forum;
        private Comment _comment;

        public Location Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public Guest1OpenForumViewModel(MyICommand<string> navigationCommand, User guest, Location location)
        {
            _forumService = new ForumService();

            NavigationCommand = navigationCommand;
            OpenForumCommand = new MyICommand(OnOpenForum);

            Guest = guest;
            Location = location;

            InitializeData();
        }

        private void InitializeData()
        {
            InitializeForum();
        }

        private void InitializeForum()
        {
            Forum = new Forum(Guest, Location);
            Comment = new Comment(Forum, Guest);
        }

        public void OnOpenForum()
        {
            if (Forum.IsValid && Comment.IsValid)
            {
                
            }

            if (!Forum.IsValid || !Comment.IsValid) return;
            string messageBoxText = "Da li ste sigurni da otvorite forum?\nNaslov: " + Forum.Title +
                "\nLokacija: " + Forum.Location.City + ", " + Forum.Location.Country;
            string caption = "Otvaranje foruma";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;
            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                _forumService.OpenForum(Forum, Comment);
                NavigationCommand.Execute("previousViewModel");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
