using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{

    public partial class TourRatingFormView : Page
    {
        private int currentGuestId;
        TourRatingFormViewModel viewModel;
        public TourRatingFormView(TourOccurrence selectedTourOccurrence, int currentGuestId)
        {
            InitializeComponent();
            viewModel = new TourRatingFormViewModel(selectedTourOccurrence, currentGuestId);
            DataContext = viewModel;
            this.currentGuestId = currentGuestId;
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            TourRateBtn.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            TourImgBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
            CommentBtn.DataContext = toolTipViewModel;
            popup3.DataContext = toolTipViewModel;
        }
        private void SubmitRating_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to rate \nthis tour?", "Tour rating", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                viewModel.SubmitRating();
                MyTours myTours = new MyTours(currentGuestId, true);
                this.NavigationService.Navigate(myTours);
            }
        }
        private void PreviewImages_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Urls.Count != 0)
            {
                TourPhotosView tourPhotosView = new TourPhotosView(viewModel.Urls.ToList());
                tourPhotosView.Show();
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            MyTours myTours = new MyTours(currentGuestId);
            this.NavigationService.Navigate(myTours);
        }
    }
}
