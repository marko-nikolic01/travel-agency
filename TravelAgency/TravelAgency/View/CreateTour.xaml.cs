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
    /// Interaction logic for CreateTour.xaml
    /// </summary>
    public partial class CreateTour : Window
    {
        public TourRepository TourRepository { get; set; }
        public Tour NewTour { get; set; }
        public CreateTour(TourRepository tourRepository)
        {
            InitializeComponent();
            DataContext = this;
            NewTour = new Tour();
            TourRepository = tourRepository;  
        }

        private void AddKeyPointClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyPointsText.Text))
            {
                return;
            }
            ListKeyPoints.Items.Add(KeyPointsText.Text);
            KeyPointsText.Clear();
            KeyPointsText.Focus();
        }

        private void AddDateTimeClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DateTimeText.Text))
            {
                return;
            }
            ListDateTimes.Items.Add(DateTimeText.Text);
            DateTimeText.Clear();
            DateTimeText.Focus();
        }

        private void AddImagesClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageText.Text))
            {
                return;
            }
            ListImages.Items.Add(ImageText.Text);
            ImageText.Clear();
            ImageText.Focus();
        }

        private void CreateClick(object sender, RoutedEventArgs e)
        {
            int result = 0;
            int.TryParse(MaxGuestsText.Text, out result);
            NewTour.MaxGuestNumber = result;
            int.TryParse(DurationText.Text, out result);
            NewTour.Duration = result;
            TourRepository.SaveTours(NewTour);
            Close();
        }
    }
}
