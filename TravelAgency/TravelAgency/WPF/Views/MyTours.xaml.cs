using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class MyTours : Page
    {
        MyToursViewModel myToursViewModel;
        public MyTours(int guestId=0)
        {
            InitializeComponent();
            /*myToursViewModel = new MyToursViewModel(guestId);
            DataContext = myToursViewModel;*/
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            //todo: refactor this function with viewmodel and service
            TourRatingRepository tourRatingRepository = new TourRatingRepository();
            if(myToursViewModel.SelectedTourOccurrence != null)
            {
                if (tourRatingRepository.IsTourNotRated(myToursViewModel.currentGuestId, myToursViewModel.SelectedTourOccurrence.Id))
                {
                    TourRatingWindow tourRatingWindow = new TourRatingWindow(myToursViewModel.SelectedTourOccurrence, myToursViewModel.currentGuestId);
                    tourRatingWindow.Show();
                }
                else 
                {
                    MessageBox.Show("This tour occurrence is already rated.");
                }
            }
        }
        private void ShowDetails_Click(object sender, RoutedEventArgs e)
        {
            FinishedTourDetailedView details = new FinishedTourDetailedView(myToursViewModel.SelectedTourOccurrence);
            Point point = Mouse.GetPosition(this);
            Point pointToScreen = PointToScreen(point);
            details.Left = pointToScreen.X - 800;
            details.Top = pointToScreen.Y - 520;
            details.Show();
        }
    }
}
