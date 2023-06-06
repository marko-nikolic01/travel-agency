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
    /// Interaction logic for OwnerForumOverviewView.xaml
    /// </summary>
    public partial class OwnerForumOverviewView : Page
    {
        public MyICommand NavigateBackCommand { get; set; }

        public OwnerForumOverviewViewModel ViewModel { get; set; }

        public OwnerForumOverviewView(OwnerForumOverviewViewModel viewModel)
        {
            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
            
            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void Execute_NavigateBackCommand()
        {
            NavigationService.Navigate(ViewModel.BackPage);
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateBackCommand();
        }
    }
}
