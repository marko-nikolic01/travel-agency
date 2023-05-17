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
using TravelAgency.Commands;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerAccommodations.xaml
    /// </summary>
    public partial class OwnerAccommodationsView : Page
    {
        public MyICommand NavigateToManageAccommodationsPageCommand { get; set; }
        public MyICommand NavigateToRenovationsPageCommand { get; set; }

        public OwnerAccommodationsViewModel ViewModel { get; set; }

        public OwnerAccommodationsView()
        {
            NavigateToManageAccommodationsPageCommand = new MyICommand(Execute_ManageAccommodationsNavigationButton);
            NavigateToRenovationsPageCommand = new MyICommand(Execute_RenovationsNavigationButton);
            InitializeComponent();
            ViewModel = new OwnerAccommodationsViewModel();
            DataContext = ViewModel;
            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_RenovationsNavigationButton()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerRenovationsView.xaml", UriKind.Relative));
        }

        private void RenovationsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Execute_RenovationsNavigationButton();
        }

        private void ManageAccommodationsNavigationButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            Execute_ManageAccommodationsNavigationButton();
        }

        private void Execute_ManageAccommodationsNavigationButton()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerManageAccommodationsView.xaml", UriKind.Relative));
        }

        private void AccommodationsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Execute_ManageAccommodationsNavigationButton();
        }
    }
}
