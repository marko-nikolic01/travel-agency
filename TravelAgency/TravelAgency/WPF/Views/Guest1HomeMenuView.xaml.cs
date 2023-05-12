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
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest1HomeMenuView.xaml
    /// </summary>
    public partial class Guest1HomeMenuView : UserControl
    {
        public wHomeMenuViewModel ViewModel { get; set; }

        private Guest1HomeView _mainWindow;
        public Guest1HomeMenuView()
        {
            InitializeComponent();
            //ViewModel = new wHomeMenuViewModel(guest);
            //this.DataContext = ViewModel;

            //_mainWindow = guest1HomeView;
        }

        private void ButtonNavigation_Click(object sender, RoutedEventArgs e)
        {
            Button selectedTab = (Button)sender;
            Navigate(selectedTab);
        }

        private void Navigate(Button selectedTab)
        {/*
            switch (selectedTab.Name)
            {
                case "buttonAccommodationsReservations":
                    _mainWindow.HighlightSelectedTab(_mainWindow.buttonAccommodationsReservations);
                    this.NavigationService.Navigate(new AccommodationsReservationsMenuView(_mainWindow, ViewModel.Guest));
                    break;
                case "buttonReviews":
                    _mainWindow.HighlightSelectedTab(_mainWindow.buttonReviews);
                    this.NavigationService.Navigate(new ReviewsMenuView(_mainWindow, ViewModel.Guest));
                    break;
                case "buttonForums":
                    _mainWindow.HighlightSelectedTab(_mainWindow.buttonForums);
                    this.NavigationService.Navigate(new ForumsMenuView(_mainWindow, ViewModel.Guest));
                    break;
            }*/
        }
    }
}
