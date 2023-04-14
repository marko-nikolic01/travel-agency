using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.ViewModel;

namespace TravelAgency.Commands
{
    public class StatisticsButtonCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public Action _onClick;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _onClick();
        }

        public StatisticsButtonCommand(Action onClick)
        {
            _onClick = onClick;
        }
    }
}
