using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourRequestForm.xaml
    /// </summary>
    public partial class TourRequestFormView : Page
    {
        public TourRequestFormViewModel TourRequestFormViewModel { get; set; }
        public TourRequestFormView(int id)
        {
            InitializeComponent();
            TourRequestFormViewModel = new TourRequestFormViewModel(id);
            DataContext = TourRequestFormViewModel;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (TourRequestFormViewModel.SubmitRequest())
            {
                TourRequestView requestView = new TourRequestView(TourRequestFormViewModel.guestId);
                this.NavigationService.Navigate(requestView);
            }
            else
                MessageBox.Show("Invalid input");
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TourRequestView requestView = new TourRequestView(TourRequestFormViewModel.guestId);
            this.NavigationService.Navigate(requestView);
        }
    }
}
