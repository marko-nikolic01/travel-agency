using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class TourPhotosView : Window
    {
        private TourPhotosViewModel tourPhotosViewModel;
        public TourPhotosView(TourOccurrence tourOccurrence)
        {
            InitializeComponent();
            tourPhotosViewModel = new TourPhotosViewModel(tourOccurrence.Tour.Photos);
            DataContext = tourPhotosViewModel;
        }
    }
}
