using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class GuideMainViewModel
    {
        public NavigationService NavService { get; set; }
        public RelayCommand NavigateToTeacherPageCommand { get; set; }
        public RelayCommand NavigateToCreateTourPageCommand { get; set; }
        public int ActiveGuideId { get; set; }
        public GuideMainViewModel(NavigationService navigationService, int id)
        {
            ActiveGuideId = id;
            NavService = navigationService;
            NavigateToTeacherPageCommand = new RelayCommand(Execute_NavigateToTeacherPageCommand, CanExecute_NavigateCommand);
            NavigateToCreateTourPageCommand = new RelayCommand(Execute_NavigateToCreateTourPageCommand, CanExecute_NavigateCommand);
        }
        private void Execute_NavigateToCreateTourPageCommand(object obj)
        {
            Page nastavnici = new CreateTourForm(ActiveGuideId, NavService);
            NavService.Navigate(nastavnici);
        }
        private void Execute_NavigateToTeacherPageCommand(object obj)
        {
            Page nastavnici = new Page1(3);
            NavService.Navigate(nastavnici);
        }
        private bool CanExecute_NavigateCommand(object obj)
        {
            return true;
        }
    }
}
