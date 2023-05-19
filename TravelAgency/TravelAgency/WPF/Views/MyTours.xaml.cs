using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class MyTours : Page
    {
        MyToursViewModel myToursViewModel;
        public MyTours(int guestId, bool tourRated = false)
        {
            if(tourRated)
                MessageBox.Show("Tour successfully rated.");
            InitializeComponent();
            myToursViewModel = new MyToursViewModel(guestId);
            DataContext = myToursViewModel;
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            ActiveTourBtn.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            DataGridBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
           if (myToursViewModel.CanTourBeRated())
           {
                TourRatingFormView ratingFormView = new TourRatingFormView(myToursViewModel.SelectedTourOccurrence, myToursViewModel.currentGuestId);
                this.NavigationService.Navigate(ratingFormView);
            }
           else
           {
                MessageBox.Show("This tour occurrence is already rated.");
           }
        }
        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            FinishedTourDetailedView view = new FinishedTourDetailedView(myToursViewModel.SelectedTourOccurrence, myToursViewModel.currentGuestId);
            this.NavigationService.Navigate(view);
        }
    }
}
