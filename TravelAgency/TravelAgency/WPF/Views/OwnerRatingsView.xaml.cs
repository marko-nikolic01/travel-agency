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
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerRatingsPage.xaml
    /// </summary>
    public partial class OwnerRatingsView : Page
    {
        public MyICommand FocusOtherDataGrid { get; set; }
        public MyICommand RateGuestCommand { get; set; }

        public OwnerRatingsViewModel ViewModel { get; set; }

        public OwnerRatingsView()
        {
            FocusOtherDataGrid = new MyICommand(Execute_FocusOtherDataGrid);
            RateGuestCommand = new MyICommand(Execute_RateGuestCommand);
            InitializeComponent();
            ViewModel = new OwnerRatingsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            ratingsDataGrid.Loaded += FocusFirstDataGrid;
        }

        private void Execute_RateGuestCommand()
        {
            if (reservationsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Select a reservation.", "No reservation selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            OwnerRateGuestViewModel vm = new OwnerRateGuestViewModel(ViewModel.SelectedReservation, this.NavigationService);
            OwnerRateGuestView page = new OwnerRateGuestView(vm);
            this.NavigationService.Navigate(page);
        }

        private void RateGuest_Click(object sender, RoutedEventArgs e)
        {
            Execute_RateGuestCommand();
        }

        private void FocusFirstDataGrid(object sender, RoutedEventArgs e)
        {
            if (ratingsDataGrid.Items.Count > 0)
            {
                ratingsDataGrid.SelectedItem = ratingsDataGrid.Items[0];
                ratingsDataGrid.ScrollIntoView(ratingsDataGrid.Items[0]);
                ratingsDataGrid.Focus();
            }
        }

        private void FocusSecondDataGrid(object sender, RoutedEventArgs e)
        {
            if (reservationsDataGrid.Items.Count > 0)
            {
                reservationsDataGrid.SelectedItem = reservationsDataGrid.Items[0];
                reservationsDataGrid.ScrollIntoView(reservationsDataGrid.Items[0]);
                reservationsDataGrid.Focus();
            }
        }

        private void Execute_FocusOtherDataGrid()
        {
            if (ratingsDataGrid.IsFocused)
            {
                FocusSecondDataGrid(null, null);
            }
            else
            {
                FocusFirstDataGrid(null, null);
            }
        }
    }
}
