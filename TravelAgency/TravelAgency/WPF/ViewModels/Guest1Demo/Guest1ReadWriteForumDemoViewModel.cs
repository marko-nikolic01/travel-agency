using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1ReadWriteForumDemoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;
        private bool _visibility;
        public DemoInstruction Instruction
        {
            get => _instruction;
            set
            {
                if (value != _instruction)
                {
                    _instruction = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Visibility
        {
            get => _visibility;
            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public User Guest { get; set; }
        private Comment _comment;
        private ObservableCollection<Comment> _comments;

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

        public Guest1ReadWriteForumDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper, User guest)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
            Guest = guest;
            Comment = new Comment();

            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            string text = "Čitanje komentara: Ovde možete čitati komentare.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            Visibility = true;
            text = "Pisanje komentara: Popunjavamo tekst komentara.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Comment.Text = "Komentar..."; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pisanje komentara: Pritiskom na dugme \"Postavi komentar\" završavamo sa pisanjem komentara.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnWriteComment(); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        private void InitializeData()
        {
            InitializeComments();
        }

        private void InitializeComments()
        {
            Comments = new ObservableCollection<Comment>();
            Comment comment = new Comment();
            comment.User = new User();
            comment.User.Username = "Korisnik";
            Comments.Add(comment);
            Comment.Text = "";
        }

        private void OnWriteComment()
        {
            Comments = new ObservableCollection<Comment>();
            Comment comment = new Comment();
            comment.User = new User();
            comment.User.Username = "Korisnik";
            Comments.Add(comment);
            comment = new Comment();
            comment.User = Guest;
            Comments.Add(comment);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
