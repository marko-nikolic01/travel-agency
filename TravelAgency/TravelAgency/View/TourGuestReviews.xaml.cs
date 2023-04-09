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
    /// Interaction logic for TourGuestReviews.xaml
    /// </summary>
    public partial class TourGuestReviews : Window
    {
        public ObservableCollection<TourRating> TourRatings { get; set; }
        public TourGuestReviews(int id)
        {
            InitializeComponent();
            DataContext = this;
            TourRatingPhotoRepository tourRatingPhotoRepository = new TourRatingPhotoRepository();
            TourRatingRepository tourRatingRepository = new TourRatingRepository(tourRatingPhotoRepository);
            TourRatings = new ObservableCollection<TourRating>(tourRatingRepository.GetRatingsByTourOccurrenceId(id));
        }
    }
}
