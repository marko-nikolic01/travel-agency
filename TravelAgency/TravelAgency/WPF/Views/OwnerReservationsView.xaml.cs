using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerReservationsPage.xaml
    /// </summary>
    public partial class OwnerReservationsView : Page
    {
        public MyICommand FocusOtherDataGrid { get; set; }
        public MyICommand NavigateToReviewRequestCommand { get; set; }

        public OwnerReservationsViewModel ViewModel { get; set; }

        public OwnerReservationsView()
        {
            FocusOtherDataGrid = new MyICommand(Execute_FocusOtherDataGrid);
            NavigateToReviewRequestCommand = new MyICommand(Execute_NavigateToReviewRequestCommand);
            InitializeComponent();
            ViewModel = new OwnerReservationsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            activeReservationsDataGrid.Loaded += FocusFirstDataGrid;
        }

        private void FocusFirstDataGrid(object sender, RoutedEventArgs e)
        {
            if (activeReservationsDataGrid.Items.Count > 0)
            {
                activeReservationsDataGrid.SelectedItem = activeReservationsDataGrid.Items[0];
                activeReservationsDataGrid.ScrollIntoView(activeReservationsDataGrid.Items[0]);
                requestsDataGrid.SelectedItems.Clear();
                activeReservationsDataGrid.Focus();
            }
        }

        private void FocusSecondDataGrid(object sender, RoutedEventArgs e)
        {
            if (requestsDataGrid.Items.Count > 0)
            {
                requestsDataGrid.SelectedItem = requestsDataGrid.Items[0];
                requestsDataGrid.ScrollIntoView(requestsDataGrid.Items[0]);
                requestsDataGrid.Focus();
            }
        }

        private void AcceptMoveRequest_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AcceptRequestCommand.Execute();
        }

        private void Execute_NavigateToReviewRequestCommand()
        {
            if (ViewModel.SelectedMoveRequest == null)
            {
                MessageBox.Show("Select a move request.");
                return;
            }

            OwnerReviewMoveRequestViewModel vm = new OwnerReviewMoveRequestViewModel(ViewModel.SelectedMoveRequest.MoveRequest, NavigationService);
            OwnerReviewMoveRequestView page = new OwnerReviewMoveRequestView(vm);
            this.NavigationService.Navigate(page);
        }

        private void Execute_FocusOtherDataGrid()
        {
            if (activeReservationsDataGrid.IsFocused)
            {
                FocusSecondDataGrid(null, null);              
            }
            else
            {
                FocusFirstDataGrid(null, null);
            }
        }

        private void NavigateToReviewRequestCommand_Click(object sender, RoutedEventArgs e)
        {
            Execute_NavigateToReviewRequestCommand();
        }
    }
}
