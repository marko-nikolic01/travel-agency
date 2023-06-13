using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerManageAccommodationsPage.xaml
    /// </summary>
    public partial class OwnerManageAccommodationsView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand AddAccommodationCommand { get; set; }
        public MyICommand NavigateToManagingSuggestions { get; set; }
        public OwnerManageAccommodationsViewModel ViewModel { get; set; }

        public OwnerManageAccommodationsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBack);
            AddAccommodationCommand = new MyICommand(Execute_AddAccommodation);
            NavigateToManagingSuggestions = new MyICommand(Execute_NavigateToManagingSuggestions);

            InitializeComponent();
            ViewModel = new OwnerManageAccommodationsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            accommodationsListView.Loaded += PreselectFirstItem;
        }

        private void Execute_NavigateToManagingSuggestions()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationSuggestionsView.xaml", UriKind.Relative));
        }

        private void Execute_AddAccommodation()
        {
            OwnerAddAccommodationViewModel vm = new OwnerAddAccommodationViewModel(this.NavigationService);
            OwnerAddAccommodationView ownerAddAccommodationPage = new OwnerAddAccommodationView(vm);
            this.NavigationService.Navigate(ownerAddAccommodationPage);
        }

        private void Execute_NavigateBack()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBack();
        }

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            Execute_AddAccommodation();
        }

        private void NavigateToManagingSuggestions_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateToManagingSuggestions();
        }

        private void DeleteSelectedAccommodationCommand_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Execute_DeleteSelectedAccommodationCommand();
        }

        private void PreselectFirstItem(object sender, RoutedEventArgs e)
        {
            if (accommodationsListView.Items.Count > 0)
            {
                accommodationsListView.SelectedItem = accommodationsListView.Items[0];
                ListBoxItem selectedItem = (ListBoxItem)accommodationsListView.ItemContainerGenerator.ContainerFromItem(accommodationsListView.SelectedItem);
                selectedItem.Focus();
            }
        }
    }
}
