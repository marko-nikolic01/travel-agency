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

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerAccommodationsStatisticsView.xaml
    /// </summary>
    public partial class OwnerAccommodationsStatisticsView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }

        public OwnerAccommodationsStatisticsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);

            InitializeComponent();

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_NavigateBackCommand()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBackCommand();
        }
    }
}
