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
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for GuideHome.xaml
    /// </summary>
    public partial class GuideMain : Window
    {
        public User ActiveGuide { get; set; }
        public GuideMain(User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = user;
        }

        private void UpcomingTours_Click(object sender, RoutedEventArgs e)
        {
            UpcomingTours upcomingTours = new UpcomingTours(ActiveGuide);
            upcomingTours.Show();
            Close();
        }

        private void TodaysTours_Click(object sender, RoutedEventArgs e)
        {
            TodaysTours todaysTours = new TodaysTours(ActiveGuide.Id);
            todaysTours.Show();
            Close();
        }

        private void GuestRatings_Click(object sender, RoutedEventArgs e)
        {
            TourRatingsView tourRatings = new TourRatingsView(ActiveGuide);
            tourRatings.Show();
            Close();
        }

        private void TourStatistics_Click(object sender, RoutedEventArgs e)
        {
            TourStatistics tourStatistics = new TourStatistics(ActiveGuide.Id);
            tourStatistics.Show();
            Close();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
