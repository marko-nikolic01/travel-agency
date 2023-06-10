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
        public MyICommand FocusOtherDataGrid { get; set; }
        public OwnerRenovationsViewModel ViewModel { get; set; }

        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand CancelRenovationCommand { get; set; }
        public MyICommand ScheduleRenovationCommand { get; set; }

        public OwnerRenovationsView()
        {
            FocusOtherDataGrid = new MyICommand(Execute_FocusOtherDataGrid);
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            CancelRenovationCommand = new MyICommand(Execute_CancelRenovationCommand);
            ScheduleRenovationCommand = new MyICommand(Execute_ScheduleRenovationCommand);

            InitializeComponent();

            ViewModel = new OwnerRenovationsViewModel(this.NavigationService);
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            scheduledRenovationsDataGrid.Loaded += FocusFirstDataGrid;
        }

        private void FocusFirstDataGrid(object sender, RoutedEventArgs e)
        {
            if (scheduledRenovationsDataGrid.Items.Count > 0)
            {
                scheduledRenovationsDataGrid.SelectedItem = scheduledRenovationsDataGrid.Items[0];
                scheduledRenovationsDataGrid.ScrollIntoView(scheduledRenovationsDataGrid.Items[0]);
                scheduledRenovationsDataGrid.Focus();
            }
        }

        private void FocusSecondDataGrid(object sender, RoutedEventArgs e)
        {
            if (pastRenovationsDataGrid.Items.Count > 0)
            {
                pastRenovationsDataGrid.SelectedItem = pastRenovationsDataGrid.Items[0];
                pastRenovationsDataGrid.ScrollIntoView(pastRenovationsDataGrid.Items[0]);
                pastRenovationsDataGrid.Focus();
            }
        }

        private void Execute_FocusOtherDataGrid()
        {
            if (scheduledRenovationsDataGrid.IsFocused)
            {
                FocusSecondDataGrid(null, null);
            }
            else
            {
                FocusFirstDataGrid(null, null);
            }
        }

        private void Execute_ScheduleRenovationCommand()
        {
            if (ViewModel.OwnerHasAccommodations())
            {
                OwnerScheduleRenovationViewModel vm = new OwnerScheduleRenovationViewModel(this.NavigationService);
                OwnerScheduleRenovationView ownerScheduleRenovationView = new OwnerScheduleRenovationView(vm);
                this.NavigationService.Navigate(ownerScheduleRenovationView);
            }
            else
            {
                MessageBox.Show("You have no accommodations.", "No accommodations", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
