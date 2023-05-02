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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for FinishedTourDetailedView.xaml
    /// </summary>
    public partial class FinishedTourDetailedView : Page
    {
        FinishedTourViewModel viewModel;
        TourPhotosViewModel photosViewModel;
        public FinishedTourDetailedView(TourOccurrence tourOccurrence, int guestId)
        {
            viewModel = new FinishedTourViewModel(tourOccurrence, guestId);
            photosViewModel = new TourPhotosViewModel(tourOccurrence.Tour.Photos);
            InitializeComponent();
            DataContext = viewModel;
            img.DataContext = photosViewModel;
            btn1.DataContext = photosViewModel;
            btn2.DataContext = photosViewModel;
        }
        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            TourRatingFormView ratingFormView = new TourRatingFormView(viewModel.tourOccurrence, viewModel.guestId);
            this.NavigationService.Navigate(ratingFormView);
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MyTours myTours = new MyTours(viewModel.guestId);
            this.NavigationService.Navigate(myTours);
        }
    }
}
