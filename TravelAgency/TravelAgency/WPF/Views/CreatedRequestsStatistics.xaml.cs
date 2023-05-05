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
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreatedRequestsStatistics.xaml
    /// </summary>
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
