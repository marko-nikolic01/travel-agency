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
    /// Interaction logic for AccommodationReservationsView.xaml
    /// </summary>
    public partial class AccommodationReservationsView : Page
    {
        private Guest1HomeView _mainWindow;
        public wAccommodationReservationsViewModel ViewModel { get; set; }
        public AccommodationReservationsView(Guest1HomeView guest1HomeView, User guest)
        {
            InitializeComponent();
            _mainWindow = guest1HomeView;

            ViewModel = new wAccommodationReservationsViewModel(guest);
            this.DataContext = ViewModel;
        }

        private void ButtonCancelReservation_Click(object sender, RoutedEventArgs e)
        {
            listViewAccommodations.SelectedItem = ((FrameworkElement)sender).DataContext;
            ViewModel.CancelReservation();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private void TextBlockAccommodationsReservations_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            this.NavigationService.Navigate(new AccommodationsReservationsMenuView(_mainWindow, ViewModel.Guest));
        }
    }
}
