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
    public class Guest1WhereverWheneverSearchDemoViewModel : ViewModelBase, INotifyPropertyChanged, IDataErrorInfo
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

        private ObservableCollection<Accommodation> _accommodations;
        public WhereverWheneverSearchFilter SearchFilter { get; set; }
        private int _dayNumber;
        private DateTime _firstDate;
        private DateTime _lastDate;
        private bool _shouldValidate;
        private DateTime _tomorrow { get; set; }

        public ObservableCollection<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (value != _accommodations)
                {
                    _accommodations = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DayNumber
        {
            get => _dayNumber;
            set
            {
                if (value != _dayNumber)
                {
                    _dayNumber = value;
                    TriggerValidationMessage();
                    SearchFilter.DayNumber = value;
                    OnPropertyChanged();
                }
            }
        }

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
                    SearchFilter.FirstDate = value;
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
                    SearchFilter.LastDate = value;
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

        public Guest1WhereverWheneverSearchDemoViewModel(MyICommand stopDemoCommand, CancellationTokenSource demoStopper)
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
            string text = "Pretraga smeštaja: Unosimo parametre pretrage.";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.GuestNumber = 1; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.DayNumber = 2; Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.SearchInsideDateSpan = true;
            FirstDate = new DateTime(3000, 1, 1); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            LastDate = new DateTime(3000, 1, 2); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;

            text = "Pretraga smeštaja: Pretragu obavljamo pritiskom na dugme \"Pretraži\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnSearch();

            text = "Pretraga smeštaja: Pretragu otkazujemo pritiskom na dugme \"Otkaži pretragu\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            OnCancelSearch(); Delay(3000);

            Visibility = false;
            text = "Rezervacija smeštaja: Ponovo pretražujemo i biramo smeštaj. Kada odaberemo smeštaj nastavljamo na rezervaciju pritiskom na dugme \"Rezerviši\".";
            Instruction.UpdateInstruction(0, 0, 0, 0, text); Delay(3000); if (_demoStopper.Token.IsCancellationRequested) return;
            SearchFilter.GuestNumber = 1;
            SearchFilter.DayNumber = 2;
            SearchFilter.SearchInsideDateSpan = true;
            FirstDate = new DateTime(3000, 1, 1);
            LastDate = new DateTime(3000, 1, 2);
            OnSearch(); Delay(3000);
        }

        private void InitializeData()
        {
            SearchFilter = new WhereverWheneverSearchFilter();
            DayNumber = 1;
            Tomorrow = DateTime.Now.Date.AddDays(1);
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
            _shouldValidate = true;
            InitializeAccommodations();
        }

        private void InitializeAccommodations()
        {
            Accommodations = new ObservableCollection<Accommodation>();
        }

        public void OnSearch()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            Accommodation accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Novi Sad";
            accommdation.Location.Country = "Serbia";
            Accommodations.Add(accommdation);
            accommdation = new Accommodation();
            accommdation.Name = "Smeštaj";
            accommdation.Location = new Location();
            accommdation.Location.City = "Zagreb";
            accommdation.Location.Country = "Croatia";
            Accommodations.Add(accommdation);
        }

        public void OnCancelSearch()
        {
            Accommodations = new ObservableCollection<Accommodation>();
            DayNumber = 1;
            SearchFilter.GuestNumber = 1;
            SearchFilter.SearchInsideDateSpan = false;
            FirstDate = DateTime.Now.Date.AddDays(1);
            LastDate = DateTime.Now.Date.AddDays(1);
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
                    bool isFutureDate = FirstDate.CompareTo(DateTime.Now) > 0;

                    if (!isFutureDate)
                    {
                        return "* First date must be a future date";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*First date can't be after last date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified number of days";
                    }

                }
                else if (columnName == "LastDate")
                {
                    bool isFutureDate = LastDate.CompareTo(DateTime.Now) > 0;
                    if (!isFutureDate)
                    {
                        return "* Last date must be a future date";
                    }

                    int dateSpanLength = (DateOnly.FromDateTime(LastDate)).DayNumber - (DateOnly.FromDateTime(FirstDate)).DayNumber + 1;
                    if (dateSpanLength <= 0)
                    {
                        return "*Last date can't be before first date";
                    }
                    else if (dateSpanLength < DayNumber)
                    {
                        return "*Date span can't be shorter than specified number of days";
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
