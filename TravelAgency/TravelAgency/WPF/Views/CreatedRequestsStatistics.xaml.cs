using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class CreatedRequestsStatistics : Page
    {
        CreatedRequestsStatisticsViewModel viewModel;
        public CreatedRequestsStatistics(int id)
        {
            InitializeComponent();
            viewModel = new CreatedRequestsStatisticsViewModel(id);
            DataContext = viewModel;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.ChangeChart();
        }
    }
}
