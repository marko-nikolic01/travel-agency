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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest2MainWindow.xaml
    /// </summary>
    public partial class Guest2MainWindow : Window
    {
        public Guest2MainViewModel ViewModel { get; set; }
        public Guest2MainWindow(int guestId)
        {
            InitializeComponent();
            Guest2ProfileView guest2 = new Guest2ProfileView(guestId);
            this.frame.NavigationService.Navigate(guest2);
            ViewModel = new Guest2MainViewModel(this.frame.NavigationService, guestId);
            this.DataContext = ViewModel;
            HelpGrid.Height = 0;
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpGrid.Height == 0)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(550, new Duration(TimeSpan.FromSeconds(0.5)));
                HelpGrid.BeginAnimation(HeightProperty, heightAnimation);
            }
            else
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.2)));
                HelpGrid.BeginAnimation(HeightProperty, heightAnimation);
            }
        }
    }
}
