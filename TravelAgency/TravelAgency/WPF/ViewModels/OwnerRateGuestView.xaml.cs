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

namespace TravelAgency.WPF.ViewModels
{
    /// <summary>
    /// Interaction logic for OwnerRateGuestView.xaml
    /// </summary>
    public partial class OwnerRateGuestView : Page
    {
        public OwnerRateGuestViewModel ViewModel { get; set; }

        public OwnerRateGuestView(OwnerRateGuestViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigateBackCommand.Execute();
        }

        private void RateGuest_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RateGuestCommand.Execute();
        }
    }
}
