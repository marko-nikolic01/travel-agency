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
using System.Windows.Threading;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest1HomeView.xaml
    /// </summary>
    public partial class Guest1HomeView : Window
    {
        public Guest1HomeViewModel ViewModel { get; set; }

        public Guest1HomeView(User guest)
        {
            InitializeComponent();
            ViewModel = new Guest1HomeViewModel(guest);
            this.DataContext = ViewModel;

            frame.Navigate(new HomeMenuView(this, guest));
        }

        private void LoadDateTime(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.statusBarDateTime.Content = DateTime.Now.ToString("dd/MM/yyyy     hh:mm:ss tt");
            }, this.Dispatcher);
            timer.Start();
        }

        private void ButtonNavigation_Click(object sender, RoutedEventArgs e)
        {
            Button selectedTab = (Button)sender;
            HighlightSelectedTab(selectedTab);
            Navigate(selectedTab);
        }

        public void HighlightSelectedTab(Button selectedTab)
        {
            ResetTabColors();
            selectedTab.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
            selectedTab.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#999999");
        }

        private void ResetTabColors()
        {
            buttonHome.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonHome.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonAccommodationsReservations.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonAccommodationsReservations.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonReviews.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonReviews.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonForums.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonForums.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonNotifications.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonNotifications.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonUserAccount.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
            buttonUserAccount.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#cccccc");
        }

        private void Navigate(Button selectedTab)
        {
            switch (selectedTab.Name)
            {
                case "buttonHome":
                    frame.Navigate(new HomeMenuView(this, ViewModel.Guest));
                    break;
                case "buttonAccommodationsReservations":
                    frame.Navigate(new AccommodationsReservationsMenuView(this, ViewModel.Guest));
                    break;
                case "buttonReviews":
                    frame.Navigate(new ReviewsMenuView(this, ViewModel.Guest));
                    break;
                case "buttonForums":
                    frame.Navigate(new ForumsMenuView(this, ViewModel.Guest));
                    break;
            }
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
