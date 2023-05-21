using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SpecialTourRequestForm.xaml
    /// </summary>
    public partial class SpecialTourRequestForm : Page
    {
        ViewModelIterator viewModelIterator;
        public SpecialTourRequestForm(int guestId)
        {
            InitializeComponent();
            viewModelIterator = new ViewModelIterator(guestId);
            DataContext = viewModelIterator.GetViewModelInstance();
            BackButton.DataContext = viewModelIterator;
            NextButton.DataContext = viewModelIterator;
            discardBtn.DataContext = viewModelIterator;
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            NumGuestBtn.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            DateBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
            DescriptionBtn.DataContext = toolTipViewModel;
            popup3.DataContext = toolTipViewModel;
        }
        private void SaveRequest_Click(object sender, RoutedEventArgs e)
        {
            if (viewModelIterator.GetViewModelInstance().Valid())
                DataContext = viewModelIterator.AddNextViewModel();
            else
                MessageBox.Show("Invalid input");
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModelIterator.GetViewModelInstance().Valid())
                DataContext = viewModelIterator.GetPreviousViewModel();
            else
                MessageBox.Show("Invalid input");
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModelIterator.GetViewModelInstance().Valid())
                DataContext = viewModelIterator.GetNextViewModel();
            else
                MessageBox.Show("Invalid input");
        }
        private void DiscardViewModel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Discard request", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModelIterator.DeleteCurrentViewModel();
                DataContext = viewModelIterator.GetViewModelInstance();
            }
        }
        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            if (viewModelIterator.viewModels.Count < 2)
                MessageBox.Show("Must be at least 2 tour requests");
            else if (viewModelIterator.GetViewModelInstance().Valid())
            {
                if (MessageBox.Show("Are you sure you want to make \nthis request?", "Special tour request", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    viewModelIterator.SaveSpecialTourRequest();
                    SpecialTourRequestsView view = new SpecialTourRequestsView(viewModelIterator.currentGuestId, true);
                    this.NavigationService.Navigate(view);
                }
            }
            else
                MessageBox.Show("Invalid input");
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SpecialTourRequestsView view = new SpecialTourRequestsView(viewModelIterator.currentGuestId);
            this.NavigationService.Navigate(view);
        }
    }
}
