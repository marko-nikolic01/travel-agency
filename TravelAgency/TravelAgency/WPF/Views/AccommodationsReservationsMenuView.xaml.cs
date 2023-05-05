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

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationsReservationsMenuView.xaml
    /// </summary>
    public partial class AccommodationsReservationsMenuView : Page
    {
        private Guest1HomeView _mainWindow;
        public AccommodationsReservationsMenuView(Guest1HomeView guest1HomeView)
        {
            InitializeComponent();
            _mainWindow = guest1HomeView;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.HighlightSelectedTab(_mainWindow.buttonHome);
            this.NavigationService.Navigate(new HomeMenuView(_mainWindow));
        }

        private void ButtonNavigation_Click(object sender, RoutedEventArgs e)
        {
            Button selectedTab = (Button)sender;
            Navigate(selectedTab);
        }

        private void Navigate(Button selectedTab)
        {
            switch (selectedTab.Name)
            {
                case "buttonSearchAccommodations":
                    this.NavigationService.Navigate(new AccommodationSearchView(_mainWindow));
                    break;
            }
        }
    }
}
