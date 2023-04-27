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
using TravelAgency.Services;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for GuideView.xaml
    /// </summary>
    public partial class GuideView : Window
    {
        public GuideView(int id)
        {
            InitializeComponent();
            Page tours = new TodaysToursView(id);
            this.frame.NavigationService.Navigate(tours);
            this.DataContext = new GuideMainViewModel(this.frame.NavigationService, id);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.frame.NavigationService.CanGoBack)
            {
                this.frame.NavigationService.GoBack();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
