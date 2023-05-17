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
    /// Interaction logic for OwnerRenovationsView.xaml
    /// </summary>
    public partial class OwnerRenovationsView : Page
    {
        public OwnerRenovationsViewModel ViewModel { get; set; }

        public MyICommand NavigateBackCommand { get; set; }

        public OwnerRenovationsView()
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            InitializeComponent();
            ViewModel = new OwnerRenovationsViewModel(this.NavigationService);
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_NavigateBackCommand()
        {
            NavigationService.Navigate(new Uri("WPF/Views/OwnerAccommodationsView.xaml", UriKind.Relative));
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBackCommand();
        }
    }
}
