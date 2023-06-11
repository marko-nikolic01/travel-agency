using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class CreatedRequestsStatistics : Page
    {
        public CreatedRequestsStatistics(int id)
        {
            InitializeComponent();
            DataContext = new CreatedRequestsStatisticsViewModel(id);
        }
    }
}
