using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Pages;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerRenovationsView.xaml
    /// </summary>
    public partial class OwnerRenovationsView : Page
    {
        public OwnerRenovationsViewModel ViewModel { get; set; }

        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand CancelRenovationCommand { get; set; }
        public MyICommand ScheduleRenovationCommand { get; set; }

        public OwnerRenovationsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            CancelRenovationCommand = new MyICommand(Execute_CancelRenovationCommand);
            ScheduleRenovationCommand = new MyICommand(Execute_ScheduleRenovationCommand);

            InitializeComponent();

            ViewModel = new OwnerRenovationsViewModel(this.NavigationService);
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);

            scheduledRenovationsDataGrid.CommandBindings.Clear();
            pastRenovationsDataGrid.CommandBindings.Clear();
        }

        private void Execute_ScheduleRenovationCommand()
        {
            OwnerScheduleRenovationViewModel vm = new OwnerScheduleRenovationViewModel(this.NavigationService);
            OwnerScheduleRenovationView ownerScheduleRenovationView = new OwnerScheduleRenovationView(vm);
            this.NavigationService.Navigate(ownerScheduleRenovationView);
        }

        private void Execute_CancelRenovationCommand()
        {
            ViewModel.CancelRenovationCommand.Execute();
            scheduledRenovationsDataGrid.Focus();
        }

        private void Execute_NavigateBackCommand()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBackCommand();
        }

        private void CancelRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelRenovationCommand.Execute();
        }

        private void ScheduleRenovation_Click(object sender, RoutedEventArgs e)
        {
            Execute_ScheduleRenovationCommand();
        }
    }
}
