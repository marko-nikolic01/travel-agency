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
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerAccommodationsStatisticsView.xaml
    /// </summary>
    public partial class OwnerAccommodationsStatisticsView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand NavigateToMonthStatsCommand { get; set; }
        public MyICommand FocusDataGrid { get; set; }

        public OwnerAccommodationsStatisticsViewModel ViewModel { get; set; }

        public OwnerAccommodationsStatisticsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            NavigateToMonthStatsCommand = new MyICommand(Execute_NavigateToMonthStatsCommand);
            FocusDataGrid = new MyICommand(Execute_FocusDataGrid);

            InitializeComponent();
            ViewModel = new OwnerAccommodationsStatisticsViewModel(this.NavigationService);
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            yearStatsDataGrid.Loaded += FocusDataGridEvent;
        }

        private void Execute_FocusDataGrid()
        {
            if (yearStatsDataGrid.Items.Count > 0)
            {
                yearStatsDataGrid.SelectedItem = yearStatsDataGrid.Items[0];
                yearStatsDataGrid.ScrollIntoView(yearStatsDataGrid.Items[0]);
                yearStatsDataGrid.Focus();
            }
        }

        private void FocusDataGridEvent(object sender, RoutedEventArgs e)
        {
            Execute_FocusDataGrid();
        }

        private void Execute_NavigateToMonthStatsCommand()
        {
            if (ViewModel.SelectedYearStats != null)
            {
                OwnerAccommodationStatisticsByYearViewModel vm = new OwnerAccommodationStatisticsByYearViewModel(ViewModel.SelectedYearStats);
                OwnerAccommodationStatisticsByYearView page = new OwnerAccommodationStatisticsByYearView(vm);
                this.NavigationService.Navigate(page);
            }
            else
            {
                MessageBox.Show("Select a year.");
            }
        }

        private void Execute_NavigateBackCommand()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBackCommand();
        }

        private void MonthlyStatistics_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateToMonthStatsCommand();
        }
    }
}
