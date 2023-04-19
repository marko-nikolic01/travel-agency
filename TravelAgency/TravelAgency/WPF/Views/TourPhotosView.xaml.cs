using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
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
