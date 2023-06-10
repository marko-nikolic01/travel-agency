using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;
using static System.Net.Mime.MediaTypeNames;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1HomeMenuDemoViewModel : ViewModelBase, INotifyPropertyChanged
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

        public Guest1HomeMenuDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            string text = "Dobrodošli u Demo režim rada aplikacije. Aplikacija će Vas sama sprovesti kroz prozore i funkcionalnosti. Demo možete zaustaviti u bilo kom trenutku pritiskom na dugme \"Zaustavi Demo\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pritiskom na obeleženo dugme nastavljate na rad sa smeštajima i rezervacijama.";
            Instruction.UpdateInstruction(3, 1, 1, 1, text); Delay(3000);
        }

        public void ExecuteDemoStep2()
        {
            string text = "Pritiskom na obeleženo dugme nastavljate na rad sa recenzijama.";
            Instruction.UpdateInstruction(4, 1, 1, 1, text); Delay(3000);
        }

        public void ExecuteDemoStep3()
        {
            string text = "Pritiskom na obeleženo dugme nastavljate na rad sa forumima.";
            Instruction.UpdateInstruction(5, 1, 1, 1, text); Delay(3000);
        }

        public void ExecuteDemoStep4()
        {
            string text = "Pritiskom na obeleženo dugme nastavljate na prikaz notifikacija.";
            Instruction.UpdateInstruction(6, 1, 1, 1, text); Delay(3000);
        }

        public void ExecuteDemoStep5()
        {
            string text = "Pritiskom na obeleženo dugme nastavljate na prikaz korisničkog naloga.";
            Instruction.UpdateInstruction(7, 1, 1, 1, text); Delay(3000);
        }

        public void ExecuteDemoStep6()
        {
            string text = "Ovo je kraj Demo prezentacije naše aplikacije. Demo će nastaviti da se izvršava sve dok ga ne zaustavite pritiskom na dugme \"Zaustavi Demo\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return; Delay(3000);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
