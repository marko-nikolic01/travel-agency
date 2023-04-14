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
        public TourRepository TourRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public KeyPointRepository KeyPointRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public VoucherRepository VoucherRepository { get; set; }
        public TourReservationRepository? TourReservationRepository { get; set; }
        public TourOccurrenceAttendanceRepository TourOccurrenceAttendanceRepository { get; set; }
        public GuideMain(Model.User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = user;

            TourRepository = new TourRepository();
            LocationRepository = new LocationRepository();
            PhotoRepository = new PhotoRepository();
            TourReservationRepository = new TourReservationRepository();
            UserRepository = new UserRepository();
            KeyPointRepository = new KeyPointRepository();
            TourOccurrenceRepository = new TourOccurrenceRepository(PhotoRepository, LocationRepository, TourRepository, TourReservationRepository, UserRepository, KeyPointRepository);
            TourOccurrenceAttendanceRepository = new TourOccurrenceAttendanceRepository();
            VoucherRepository = new VoucherRepository();
        }

        private void UpcomingTours_Click(object sender, RoutedEventArgs e)
        {
            UpcomingTours upcomingTours = new UpcomingTours(ActiveGuide, TourRepository, LocationRepository, PhotoRepository, TourOccurrenceRepository,
                KeyPointRepository, TourReservationRepository, UserRepository, TourOccurrenceAttendanceRepository, VoucherRepository);
            upcomingTours.Show();
            Close();
        }

        private void TodaysTours_Click(object sender, RoutedEventArgs e)
        {
            TodaysTours todaysTours = new TodaysTours(ActiveGuide, TourRepository, LocationRepository, PhotoRepository, TourOccurrenceRepository, 
                KeyPointRepository, TourReservationRepository, UserRepository, TourOccurrenceAttendanceRepository, VoucherRepository);
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
