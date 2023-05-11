using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
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
        public TourPhotosView(List<string> links)
        {
            InitializeComponent();
            tourPhotosViewModel = new TourPhotosViewModel(links);
            DataContext = tourPhotosViewModel;
        }
    }
}
