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
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            NumGuestBtn.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            DateBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
            DescriptionBtn.DataContext = toolTipViewModel;
            popup3.DataContext = toolTipViewModel;
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (TourRequestFormViewModel.Valid())
            {
                if (MessageBox.Show("Are you sure you want to make \nthis request?", "Tour request", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    TourRequestFormViewModel.SubmitRequest();
                    TourRequestView requestView = new TourRequestView(TourRequestFormViewModel.guestId, true);
                    this.NavigationService.Navigate(requestView);
                }
            }
            else
                MessageBox.Show("Invalid input", "Tour request", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TourRequestView requestView = new TourRequestView(TourRequestFormViewModel.guestId);
            this.NavigationService.Navigate(requestView);
        }
    }
}
