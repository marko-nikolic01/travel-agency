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
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for GuideHome.xaml
    /// </summary>
    public partial class GuideMain : Window
    {
        public User ActiveGuide { get; set; }
        public GuideMain(Model.User user)
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
            TodaysTours todaysTours = new TodaysTours(ActiveGuide);
            todaysTours.Show();
            Close();
        }

        private void GuestReviews_Click(object sender, RoutedEventArgs e)
        {
            TourReviews tourReviews = new TourReviews(ActiveGuide);
            tourReviews.Show();
            Close();
        }

        private void TourStatistics_Click(object sender, RoutedEventArgs e)
        {
            TourStatistics tourStatistics = new TourStatistics(ActiveGuide.Id);
            tourStatistics.Show();
            Close();
        }
    }
}
