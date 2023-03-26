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
        public MyTours(TourOccurrenceRepository tourOccurrenceRepository, int guestId)
        {
            InitializeComponent();
            DataContext = this;
            this.tourOccurrenceRepository = tourOccurrenceRepository;
            TourOccurrences = new List<TourOccurrence>();
            currentGuestId = guestId;
            FilterFinishedTourOccurrences();
        }
        //ovo prebaciti u repository posle ili gde vec treba
        private void FilterFinishedTourOccurrences()
        {
            //TourOccurrenceAttendanceRepository toar = new TourOccurrenceAttendanceRepository();
            foreach (TourOccurrence occurrence in tourOccurrenceRepository.GetAll())
            {
                if(occurrence.CurrentState == CurrentState.Ended)
                {
                    if(WasGuestOnTour(occurrence))
                        TourOccurrences.Add(occurrence);
                }
            }
        }
        private bool WasGuestOnTour(TourOccurrence occurrence)
        {
            TourOccurrenceAttendanceRepository toar = new TourOccurrenceAttendanceRepository();
            TourOccurrenceAttendance toa;
            toa = toar.GetAll().Find(x => x.TourOccurrenceId == occurrence.Id && x.GuestId == currentGuestId);
            if (toa != null)
            {
                if (toa.ResponseStatus == ResponseStatus.Accepted)
                    return true;
            }
            return false;
        }

        private void RateTour_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTourOccurrence != null)
            {
                TourGuideRating tourGuideRating = new TourGuideRating();
                tourGuideRating.Show();
            }
        }
    }
}
