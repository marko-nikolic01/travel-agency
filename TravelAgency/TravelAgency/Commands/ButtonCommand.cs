using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Services;
using TravelAgency.View;
using TravelAgency.ViewModel;

namespace TravelAgency.Commands
{
    public class ButtonCommand<TourReviewViewModel> : ICommand
    {
        Action<TourReviewViewModel> _onClick;
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _onClick((TourReviewViewModel)parameter);
        }

        public ButtonCommand(Action<TourReviewViewModel> onClick)
        {
            _onClick = onClick;
        }
    }
}
