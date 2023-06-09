using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Pages;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerAccommodationSuggestions.xaml
    /// </summary>
    public partial class OwnerAccommodationSuggestionsView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand NavigateToAddAccommodationCommand { get; set; }
        public OwnerAccommodationSuggestionsViewModel ViewModel { get; set; }
        public MyICommand FocusOtherDataGrid { get; set; }

        public OwnerAccommodationSuggestionsView()
        {
            FocusOtherDataGrid = new MyICommand(Execute_FocusOtherDataGrid);
            NavigateBackCommand = new MyICommand(Execute_NavigateBack);
            NavigateToAddAccommodationCommand = new MyICommand(Execute_AddAccommodation);

            InitializeComponent();
            ViewModel = new OwnerAccommodationSuggestionsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            BestLocationsDataGrid.Loaded += FocusFirstDataGrid;
        }

        private void FocusFirstDataGrid(object sender, RoutedEventArgs e)
        {
            if (BestLocationsDataGrid.Items.Count > 0)
            {
                BestLocationsDataGrid.SelectedIndex = 0;
                BestLocationsDataGrid.ScrollIntoView(BestLocationsDataGrid.Items[0]);
                BestLocationsDataGrid.Focus();
            }
        }

        private void FocusSecondDataGrid(object sender, RoutedEventArgs e)
        {
            if (WorstAccommodaionsDataGrid.Items.Count > 0)
            {
                WorstAccommodaionsDataGrid.SelectedIndex = 0;
                WorstAccommodaionsDataGrid.ScrollIntoView(WorstAccommodaionsDataGrid.Items[0]);
                WorstAccommodaionsDataGrid.Focus();
            }
        }

        private void Execute_FocusOtherDataGrid()
        {
            if (BestLocationsDataGrid.IsFocused)
            {
                FocusSecondDataGrid(null, null);
            }
            else
            {
                FocusFirstDataGrid(null, null);
            }
        }

        private void Execute_NavigateBack()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerManageAccommodationsView.xaml", UriKind.Relative));
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBack();
        }

        private void Execute_AddAccommodation()
        {
            if (BestLocationsDataGrid.SelectedItem ==  null)
            {
                MessageBox.Show("Select a location.");
                return;
            }

            OwnerAddAccommodationViewModel vm = new OwnerAddAccommodationViewModel(this.NavigationService, ViewModel.SelectedLocation.Location.Country, ViewModel.SelectedLocation.Location.City, "WPF/Views/OwnerAccommodationSuggestionsView.xaml");
            OwnerAddAccommodationView ownerAddAccommodationPage = new OwnerAddAccommodationView(vm, ViewModel.SelectedLocation.Location.Country, ViewModel.SelectedLocation.Location.City);
            this.NavigationService.Navigate(ownerAddAccommodationPage);
        }

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Execute_AddAccommodation();
        }

        private void DeleteSelectedAccommodationCommand_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Execute_DeleteSelectedAccommodationCommand();
        }
    }
}
