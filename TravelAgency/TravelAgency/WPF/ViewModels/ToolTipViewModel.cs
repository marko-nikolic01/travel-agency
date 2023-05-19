using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class ToolTipViewModel : INotifyPropertyChanged
    {
        private bool firstHelpClicked;
        private bool secondHelpClicked;
        private bool thirdHelpClicked;
        public bool FirstHelpClicked
        {
            get { return firstHelpClicked; }
            set { firstHelpClicked = value; OnPropertyChanged(); }
        }
        public bool SecondHelpClicked
        {
            get { return secondHelpClicked; }
            set { secondHelpClicked = value; OnPropertyChanged(); }
        }
        public bool ThirdHelpClicked
        {
            get { return thirdHelpClicked; }
            set { thirdHelpClicked = value; OnPropertyChanged(); }
        }
        public ButtonCommandNoParameter FirstHelpCommand { get; set; }
        public ButtonCommandNoParameter SecondHelpCommand { get; set; }
        public ButtonCommandNoParameter ThirdHelpCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public ToolTipViewModel() 
        {
            FirstHelpCommand = new ButtonCommandNoParameter(FirstClick);
            SecondHelpCommand = new ButtonCommandNoParameter(SecondClick);
            ThirdHelpCommand = new ButtonCommandNoParameter(ThirdClick);
        }
        private void FirstClick()
        {
            FirstHelpClicked = !FirstHelpClicked;
        }
        private void SecondClick()
        {
            SecondHelpClicked = !SecondHelpClicked;
        }
        private void ThirdClick()
        {
            ThirdHelpClicked = !ThirdHelpClicked;
        }
    }
}
