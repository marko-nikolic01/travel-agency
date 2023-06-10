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
    /// Interaction logic for OwnerReviewMoveRequestView.xaml
    /// </summary>
    public partial class OwnerReviewMoveRequestView : Page
    {

        public OwnerReviewMoveRequestViewModel ViewModel { get; set; }

        public OwnerReviewMoveRequestView(OwnerReviewMoveRequestViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            explanationTextBox.Loaded += (s, e) => explanationTextBox.Focus();
        }

        private void NavigateBack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigateBackCommand.Execute();
        }

        private void RejectRequest_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RejectRequestCommand.Execute();
        }
    }
}
