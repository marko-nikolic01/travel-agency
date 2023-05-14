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
    public partial class OwnerAccommodationsPage : Page
    {
        public MyICommand NavigateToManageAccommodationsPageCommand { get; set; }

        public OwnerAccommodationsViewModel ViewModel { get; set; }

        public OwnerAccommodationsPage()
        {
            NavigateToManageAccommodationsPageCommand = new MyICommand(Execute_ManageAccommodationsNavigationButton);
            InitializeComponent();
            ViewModel = new OwnerAccommodationsViewModel();
            DataContext = ViewModel;
            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_ManageAccommodationsNavigationButton(object sender, ExecutedRoutedEventArgs e)
        {
            Execute_ManageAccommodationsNavigationButton();
        }

        private void Execute_ManageAccommodationsNavigationButton()
        {
            NavigationService.Navigate(new Uri("WPF/Pages/OwnerManageAccommodationsPage.xaml", UriKind.Relative));
        }

        private void AccommodationsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Execute_ManageAccommodationsNavigationButton();
        }
    }
}
