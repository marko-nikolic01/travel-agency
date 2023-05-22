using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class TourRequestView : Page
    {
        TourRequestViewModel viewModel;
        public TourRequestView(int id, bool requestMade = false)
        {
            viewModel = new TourRequestViewModel(id);
            InitializeComponent();
            DataContext = viewModel;
            if (requestMade)
                MessageBox.Show("The request was made successfully.");
        }

        private void CreateRequest_Click(object sender, RoutedEventArgs e)
        {
            TourRequestFormView requestFormView = new TourRequestFormView(viewModel.guestId);
            this.NavigationService.Navigate(requestFormView);
        }
        private void ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            CreatedRequestsStatistics requestsStatistics = new CreatedRequestsStatistics(viewModel.guestId);
            this.NavigationService.Navigate(requestsStatistics);
        }
    }
}
