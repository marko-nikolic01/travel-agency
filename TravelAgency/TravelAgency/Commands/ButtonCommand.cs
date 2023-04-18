using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Services;
using TravelAgency.WPF.Views;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.Commands
{
    public class ButtonCommand<T> : ICommand
    {
        public Action<T> _onClick;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _onClick((T)parameter);
        }

        public ButtonCommand(Action<T> onClick)
        {
            _onClick = onClick;
        }
    }
}
