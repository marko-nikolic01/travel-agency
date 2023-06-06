using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1ReadForumViewModel2 : ViewModelBase, INotifyPropertyChanged
    {
        public MyICommand<string> NavigationCommand { get; private set; }

        private ForumService _forumService;

        private Forum _forum;

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

        public Guest1ReadForumViewModel2(MyICommand<string> navigationCommand, Forum forum)
        {
            NavigationCommand = navigationCommand;

            _forumService = new ForumService();

            Forum = forum;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
