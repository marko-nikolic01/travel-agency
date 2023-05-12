using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1ReviewsMenuViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public MyICommand<string> NavCommand { get; private set; }

        public Guest1ReviewsMenuViewModel(MyICommand<string> navCommand)
        {
            NavCommand = navCommand;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
