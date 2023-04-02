using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for MyTours.xaml
    /// </summary>
    public partial class MyTours : Window
    {
        public List<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private TourOccurrenceRepository tourOccurrenceRepository;
        private int currentGuestId;
        public MyTours(TourOccurrenceRepository occurrenceRepository, int guestId)
        {
            InitializeComponent();
            DataContext = this;
            tourOccurrenceRepository = occurrenceRepository;
            TourOccurrences = tourOccurrenceRepository.GetFinishedOccurrencesForGuest(guestId);
            currentGuestId = guestId;
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTourOccurrence != null)
            {
                TourRatingWindow tourRatingWindow = new TourRatingWindow(SelectedTourOccurrence, currentGuestId);
                tourRatingWindow.Show();
            }
        }
    }
}
