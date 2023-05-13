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
    /// Interaction logic for Guest1AccommodationReservationMoveRequestsView.xaml
    /// </summary>
    public partial class Guest1AccommodationReservationMoveRequestsView : UserControl
    {
        public Guest1AccommodationReservationMoveRequestsView()
        {
            InitializeComponent();
        }

        private void ButtonCancelReservation_Click(object sender, RoutedEventArgs e)
        {
            //listViewReservations.SelectedItem = ((FrameworkElement)sender).DataContext;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            /* NavigateBack();*/
        }

        private void TextBlockAccommodationsReservations_MouseUp(object sender, MouseButtonEventArgs e)
        {
            /*NavigateBack();*/
        }

        private void NavigateBack()
        {
            /*this.NavigationService.Navigate(new AccommodationsReservationsMenuView(_mainWindow, ViewModel.Guest));*/
        }

        private void ButtonMoveReservation_Click(object sender, RoutedEventArgs e)
        {
            //listViewReservations.SelectedItem = ((FrameworkElement)sender).DataContext;
        }
    }
}
