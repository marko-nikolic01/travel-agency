using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class MyTours : Page
    {
        MyToursViewModel myToursViewModel;
        public MyTours(int guestId)
        {
            InitializeComponent();
            myToursViewModel = new MyToursViewModel(guestId);
            DataContext = myToursViewModel;
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
            FinishedTourDetailedView details = new FinishedTourDetailedView(myToursViewModel.SelectedTourOccurrence);
            Point point = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(point);
            details.Left = pointToScreen.X - 800;
            details.Top = pointToScreen.Y - 520;
            details.Show();
        }
    }
}
