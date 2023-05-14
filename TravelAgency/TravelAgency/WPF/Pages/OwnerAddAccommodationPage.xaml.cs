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
    /// Interaction logic for OwnerAddAccommodationPage.xaml
    /// </summary>
    public partial class OwnerAddAccommodationPage : Page
    {
        public MyICommand NavigateBack { get; set; }
        public OwnerAddAccommodationViewModel ViewModel { get; set; }

        public OwnerAddAccommodationPage()
        {
            NavigateBack = new MyICommand(Execute_NavigateBack);
            InitializeComponent();
            ViewModel = new OwnerAddAccommodationViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_NavigateBack()
        {
            NavigationService.Navigate(new Uri("WPF/Pages/OwnermanageAccommodationsPage.xaml", UriKind.Relative));
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBack();
        }
    }
}
