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
    /// Interaction logic for OwnerAccommodationSuggestions.xaml
    /// </summary>
    public partial class OwnerAccommodationSuggestionsView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand NavigateToAddAccommodationCommand { get; set; }
        public OwnerAccommodationSuggestionsViewModel ViewModel { get; set; }

        public OwnerAccommodationSuggestionsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBack);
            NavigateToAddAccommodationCommand = new MyICommand(Execute_AddAccommodation);

            InitializeComponent();
            ViewModel = new OwnerAccommodationSuggestionsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
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
