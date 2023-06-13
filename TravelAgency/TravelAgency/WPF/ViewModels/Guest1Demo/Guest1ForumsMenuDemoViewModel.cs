using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1ForumsMenuDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        public Guest1ForumsMenuDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
        {
            Instruction = new DemoInstruction();
            StopDemoCommand = stopDemoCommand;
            _demoStopper = demoStopper;
        }

        private void Delay(int ms)
        {
            Thread.Sleep(ms);
        }

        public void ExecuteDemoStep1()
        {
            string text = "Ovo je meni za rad sa forumima.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pritiskom na obeleženo dugme nastavljate na otvaranje novih foruma.";
            Instruction.UpdateInstruction(3, 1, 1, 1, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        public void ExecuteDemoStep2()
        {
            string text = "Pritiskom na obeleženo dugme nastavljate na rad sa vašim forumima.";
            Instruction.UpdateInstruction(4, 1, 1, 1, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        public void ExecuteDemoStep3()
        {
            string text = "Pritiskom na obeleženo dugme nastavljate na čitanje foruma i pisanje komentara.";
            Instruction.UpdateInstruction(5, 1, 1, 1, text); Delay(3000);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
