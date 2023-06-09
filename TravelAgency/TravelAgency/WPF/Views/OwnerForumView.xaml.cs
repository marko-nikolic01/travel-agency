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

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerForumView.xaml
    /// </summary>
    public partial class OwnerForumView : Page
    {
        public MyICommand NavigateToForumForLocationCommand { get; set; }
        public OwnerForumViewModel ViewModel { get; set; }

        public OwnerForumView()
        {
            NavigateToForumForLocationCommand = new MyICommand(Execute_NavigateToForumForLocationCommand);
            InitializeComponent();
            ViewModel = new OwnerForumViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            forumLocationsListView.Loaded += PreselectFirstItem;
        }

        private void NavigateToForumsForLocation_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateToForumForLocationCommand();
        }

        private void Execute_NavigateToForumForLocationCommand()
        {
            if (ViewModel.SelectedLocation == null)
            {
                MessageBox.Show("Select a location.");
                return;
            }

            OwnerForumsForLocationViewModel vm = new OwnerForumsForLocationViewModel(ViewModel.SelectedLocation);
            OwnerForumsForLocation page = new OwnerForumsForLocation(vm);
            this.NavigationService.Navigate(page);
        }

        private void PreselectFirstItem(object sender, RoutedEventArgs e)
        {
            if (forumLocationsListView.Items.Count > 0)
            {
                forumLocationsListView.SelectedItem = forumLocationsListView.Items[0];
                ListBoxItem selectedItem = (ListBoxItem)forumLocationsListView.ItemContainerGenerator.ContainerFromItem(forumLocationsListView.SelectedItem);
                selectedItem.Focus();
            }
        }
    }
}
