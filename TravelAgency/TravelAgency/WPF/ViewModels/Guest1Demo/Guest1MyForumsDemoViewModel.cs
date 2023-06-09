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
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    class Guest1MyForumsDemoViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;
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

        private ObservableCollection<Forum> _forums;

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

        public Guest1MyForumsDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            string text = "Ovde možete videti svoje forume.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Zatvaranje foruma: Forum zatvaramo pritiskom na dugme \"Zatvori\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnCloseForum();

            text = "Čitanje i pisanje komentara: Biramo forum  i pritiskom na dugme \"Čitaj i komentariši/Čitaj\" ulazimo u forum gde možemo učestvovati u diskusiji sa drugim korisnicima.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        private void InitializeData()
        {
            InitializeForums();
        }

        private void InitializeForums()
        {
            Forums = new ObservableCollection<Forum>();
            Forum forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Serbia";
            forum.Location.City = "Novi Sad";
            Forums.Add(forum);
            forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Croatia";
            forum.Location.City = "Zagreb";
            Forums.Add(forum);
        }

        private void OnCloseForum()
        {
            Forums = new ObservableCollection<Forum>();
            Forum forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Serbia";
            forum.Location.City = "Novi Sad";
            Forums.Add(forum);
            forum = new Forum();
            forum.Location = new Location();
            forum.Location.Country = "Croatia";
            forum.Location.City = "Zagreb";
            forum.Close();
            Forums.Add(forum);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
