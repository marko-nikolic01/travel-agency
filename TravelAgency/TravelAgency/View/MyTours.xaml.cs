using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    public partial class MyTours : Window
    {
        public List<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private TourOccurrenceRepository tourOccurrenceRepository;
        private TourOccurrenceAttendanceRepository tourAttendanceRepository;
        private int currentGuestId;
        public MyTours(TourOccurrenceRepository occurrenceRepository, TourOccurrenceAttendanceRepository tourAttendanceRepository, int guestId)
        {
            InitializeComponent();
            DataContext = this;
            tourOccurrenceRepository = occurrenceRepository;
            this.tourAttendanceRepository = tourAttendanceRepository;
            TourOccurrences = tourOccurrenceRepository.GetFinishedOccurrencesForGuest(guestId);
            currentGuestId = guestId;
            activeTourText.Text = tourOccurrenceRepository.GetActiveTour(currentGuestId);
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            TourRatingRepository tourRatingRepository = new TourRatingRepository();
            if(SelectedTourOccurrence != null)
            {
                if (tourRatingRepository.TourIsNotRated(currentGuestId, SelectedTourOccurrence.Id))
                {
                    TourRatingWindow tourRatingWindow = new TourRatingWindow(SelectedTourOccurrence, currentGuestId);
                    tourRatingWindow.Show();
                }
                else 
                {
                    MessageBox.Show("This tour occurrence is already rated.");
                }
            }
        }
    }
}
