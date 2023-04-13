using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TourReviews.xaml
    /// </summary>
    public partial class TourReviews : Window
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public TourReviews(User activeGuide)
        {
            InitializeComponent();
            DataContext = this;
            PhotoRepository photoRepository = new PhotoRepository();
            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            UserRepository userRepository = new UserRepository();
            KeyPointRepository keyPointRepository = new KeyPointRepository();
            TourOccurrenceRepository tourOccurrenceRepository = new TourOccurrenceRepository( photoRepository,  locationRepository,  tourRepository,  tourReservationRepository,  userRepository, keyPointRepository);
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceRepository.GetFinishedOccurrencesForGuide(activeGuide.Id));
        }
        private void View_Click(object sender, RoutedEventArgs e)
        {
            TourGuestReviews tourGuestReviews = new TourGuestReviews(SelectedTourOccurrence.Id);
            tourGuestReviews.Show();
            Close();
        }
    }
}
