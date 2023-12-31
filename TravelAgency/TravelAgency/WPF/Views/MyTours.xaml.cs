﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class MyTours : Page
    {
        MyToursViewModel myToursViewModel;
        public MyTours(int guestId, bool tourRated = false)
        {
            if(tourRated)
                MessageBox.Show("Tour successfully rated.", "My tours", MessageBoxButton.OK, MessageBoxImage.Information);
            InitializeComponent();
            myToursViewModel = new MyToursViewModel(guestId);
            DataContext = myToursViewModel;
            ToolTipViewModel toolTipViewModel = new ToolTipViewModel();
            ActiveTourBtn.DataContext = toolTipViewModel;
            popup1.DataContext = toolTipViewModel;
            DataGridBtn.DataContext = toolTipViewModel;
            popup2.DataContext = toolTipViewModel;
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
           if (myToursViewModel.SelectedTourOccurrence == null)
                MessageBox.Show("You must select tour occurrence to rate." , "Finished tours", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (myToursViewModel.CanTourBeRated())
           {
                TourRatingFormView ratingFormView = new TourRatingFormView(myToursViewModel.SelectedTourOccurrence, myToursViewModel.currentGuestId);
                this.NavigationService.Navigate(ratingFormView);
           }
           else
                MessageBox.Show("This tour occurrence is already rated.", "Finished tours", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            FinishedTourDetailedView view = new FinishedTourDetailedView(myToursViewModel.SelectedTourOccurrence, myToursViewModel.currentGuestId);
            this.NavigationService.Navigate(view);
        }
    }
}
