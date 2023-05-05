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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerManageAccommodationsPage.xaml
    /// </summary>
    public partial class OwnerManageAccommodationsPage : Page
    {
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationService accommodationService;

        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public OwnerManageAccommodationsPage()
        {
            InitializeComponent();
            DataContext = this;

            userService = new UserService();
            accommodationService = new AccommodationService();

            LoggedInUser = userService.GetLoggedInUser();

            Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetByOwner(LoggedInUser));
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OwnerAccommodationsPage());
        }
    }
}
