using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1MyForumsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private ForumService _forumService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public MyICommand ReadWriteCommand { get; private set; }
        public MyICommand ReadCommand { get; private set; }
        public MyICommand CloseForumCommand { get; private set; }

        public User Guest { get; set; }
        private ObservableCollection<Forum> _forums;
        private Forum _selectedForum;

        public ObservableCollection<Forum> Forums
        {
            get => _forums;
            set
            {
                if (value != _forums)
                {
                    _forums = value;
                    OnPropertyChanged();
                }
            }
        }

        public Forum SelectedForum
        {
            get => _selectedForum;
            set
            {
                if (value != _selectedForum)
                {
                    _selectedForum = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1MyForumsViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;
            CloseForumCommand = new MyICommand(OnCloseForum);

            _forumService = new ForumService();

            Guest = guest;
            InitializeData();
        }

        private void InitializeData()
        {
            InitializeForums();
        }

        private void InitializeForums()
        {
            List<Forum> forums = _forumService.GetForumsByAdmin(Guest);
            forums = ReverseForums(forums);
            Forums = new ObservableCollection<Forum>(forums);
        }

        private List<Forum> ReverseForums(List<Forum> forums)
        {
            List<Forum> reversedForums = new List<Forum>();
            for (int count = forums.Count() - 1; count >= 0; count--)
            {
                reversedForums.Add(forums[count]);
            }
            return reversedForums;
        }

        private void OnCloseForum()
        {
            _forumService.CloseForum(SelectedForum);
            InitializeForums();

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
