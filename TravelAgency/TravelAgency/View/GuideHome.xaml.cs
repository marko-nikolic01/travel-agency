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
    public partial class GuideHome : Window
    {
        public User ActiveGuide { get; set; }
        public TourRepository TourRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public KeyPointRepository KeyPointRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public TourReservationRepository? TourReservationRepository { get; set; }
        public TourOccurrenceAttendanceRepository TourOccurrenceAttendanceRepository { get; set; }
        public GuideHome(Model.User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = user;

            TourRepository = new TourRepository();
            LocationRepository = new LocationRepository();
            PhotoRepository = new PhotoRepository();
            TourOccurrenceRepository = new TourOccurrenceRepository();
            KeyPointRepository = new KeyPointRepository();
            TourReservationRepository = new TourReservationRepository();
            UserRepository = new UserRepository();
            TourOccurrenceAttendanceRepository = new TourOccurrenceAttendanceRepository();

            LinkData();
        }

        private void LinkData()
        {
            LinkTourLocation();
            LinkTourPhotos();
            LinkTourOccurrences();
            LinkKeyPoints();
            LinkTourGuests();
        }

        private void LinkTourGuests()
        {
            foreach (TourReservation tourReservation in TourReservationRepository.GetTourReservations())
            {
                TourOccurrence tourOccurrence = TourOccurrenceRepository.GetAll().Find(x => x.Id == tourReservation.TourOccurrenceId);
                User guest = UserRepository.GetUsers().Find(x => x.Id == tourReservation.UserId);
                tourOccurrence.Guests.Add(guest);
            }
        }

        private void LinkKeyPoints()
        {
            foreach (KeyPoint keyPoint in KeyPointRepository.GetKeyPoints())
            {
                TourOccurrence tourOccurrence = TourOccurrenceRepository.GetAll().Find(tO => tO.Id == keyPoint.TourOccurrenceId);
                if (tourOccurrence != null)
                {
                    tourOccurrence.KeyPoints.Add(keyPoint);
                }
            }
        }

        private void LinkTourOccurrences()
        {
            foreach (TourOccurrence tourOccurrence in TourOccurrenceRepository.GetAll())
            {
                Tour tour = TourRepository.GetAll().Find(t => t.Id == tourOccurrence.TourId);
                if (tour != null)
                {
                    tourOccurrence.Tour = tour;
                }
            }
        }

        private void LinkTourPhotos()
        {
            foreach (Photo photo in PhotoRepository.GetAll())
            {
                Tour tour = TourRepository.GetAll().Find(t => t.Id == photo.TourId);
                if (tour != null)
                {
                    tour.Photos.Add(photo);
                }
            }
        }

        private void LinkTourLocation()
        {
            foreach (var tour in TourRepository.GetAll())
            {
                Location location = LocationRepository.GetAll().Find(l => l.Id == tour.LocationId);
                if (location != null)
                {
                    tour.Location = location;
                }
            }
        }
        private void UpcomingTours_Click(object sender, RoutedEventArgs e)
        {
            UpcomingTours upcomingTours = new UpcomingTours();
            upcomingTours.Show();
            Close();
        }

        private void TodaysTours_Click(object sender, RoutedEventArgs e)
        {
            TodaysTours todaysTours = new TodaysTours(ActiveGuide, TourRepository, LocationRepository, PhotoRepository, TourOccurrenceRepository, 
                KeyPointRepository, TourReservationRepository, UserRepository, TourOccurrenceAttendanceRepository);
            todaysTours.Show();
            Close();
        }
    }
}
