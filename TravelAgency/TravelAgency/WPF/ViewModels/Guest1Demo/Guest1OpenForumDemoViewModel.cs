using System;
using System.Collections.Generic;
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
    public class Guest1OpenForumDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        private Forum _forum;
        private Comment _comment;

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

        public Guest1OpenForumDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
            InitializeData();
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemo()
        {
            Visibility = true;
            string text = "Otvaranje foruma: Unosimo naslov i inicijalni komentar.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Forum.Title = "Naslov"; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            Comment.Text = "Komentar..."; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Otvaranje foruma: Na kraju pritiskom na dugme \"Otvori forum\" završavate sa otvaranjem foruma.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        private void InitializeData()
        {
            InitializeForum();
        }

        private void InitializeForum()
        {
            Forum = new Forum();
            Comment = new Comment();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
