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
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourPhotosView.xaml
    /// </summary>
    public partial class TourPhotosView : Window
    {
        private TourOccurrence tourOccurrence;
        private TourPhotosViewModel tourPhotosViewModel;
        public TourPhotosView(TourOccurrence tourOccurrence)
        {
            InitializeComponent();
            this.tourOccurrence = tourOccurrence;
            tourPhotosViewModel = new TourPhotosViewModel(tourOccurrence.Tour.Photos);
            DataContext = tourPhotosViewModel;
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {
            tourPhotosViewModel.ShowNextPhoto();
        }

        private void PreviousImage_Click(object sender, RoutedEventArgs e)
        {
            tourPhotosViewModel.ShowPreviousPhoto();
        }
    }
}
