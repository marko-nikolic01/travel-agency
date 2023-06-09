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
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels.Guest1Demo
{
    public class Guest1ReportDemoViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
    {
        private DemoInstruction _instruction;
        public MyICommand StopDemoCommand { get; private set; }
        private CancellationTokenSource _demoStopper;
        private bool _visibility1;
        private bool _visibility2;


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

        public bool Visibility1
        {
            get => _visibility1;
            set
            {
                if (value != _visibility1)
                {
                    _visibility1 = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Visibility2
        {
            get => _visibility2;
            set
            {
                if (value != _visibility2)
                {
                    _visibility2 = value;
                    OnPropertyChanged();
                }
            }
        }


        private DateTime _firstDate;
        private DateTime _lastDate;
        private bool _shouldValidate;

        private DateTime _tomorrow { get; set; }

        public DateTime FirstDate
        {
            get => _firstDate;
            set
            {
                if (value != _firstDate)
                {
                    _firstDate = value;
                    if (_shouldValidate)
                    {
                        TriggerValidationMessage();
                    }
                    OnPropertyChanged();
                }
            }
        }

        public DateTime LastDate
        {
            get => _lastDate;
            set
            {
                if (value != _lastDate)
                {
                    _lastDate = value;
                    if (_shouldValidate)
                    {
                        TriggerValidationMessage();
                    }
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Tomorrow
        {
            get => _tomorrow;
            set
            {
                if (value != _tomorrow)
                {
                    _tomorrow = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1ReportDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            Visibility1 = true;
            Visibility2 = false;
            string text = "Preuzimanje izveštaja: Prvo podesimo opseg datuma za koji hoćemo da preuzmemo izveštaj.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            LastDate = new DateTime(3000, 1, 2); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            FirstDate = new DateTime(3000, 1, 1); ; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            Visibility1 = false;
            Visibility2 = true;
            text = "Preuzimanje izveštaja: Kada podesimo opseg datuma imamo mogućnost da preuzmemo 2 tipa izveštaja. Prvi tip je izveštaj o zakazanim rezervacijama, a drugi tip je izveštaj o otkazanim rezervacijama. Pritiskom na dugme \"Preuzmi izveštaj\" možete preuzeti izveštaj u PDF formatu.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
        }

        private void InitializeData()
        {
            Tomorrow = DateTime.Now.Date.AddDays(1);
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
            _shouldValidate = true;
        }

        public void TriggerValidationMessage()
        {
            _shouldValidate = false;
            FirstDate = FirstDate.AddDays(1);
            FirstDate = FirstDate.AddDays(-1);
            LastDate = LastDate.AddDays(1);
            LastDate = LastDate.AddDays(-1);
            _shouldValidate = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "FirstDate")
                {
                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
                    }

                }
                else if (columnName == "LastDate")
                {
                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*Last date can't be before first date";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "FirstDate", "LastDate" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
