using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Commands;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Services;
using TravelAgency.View;

namespace TravelAgency.ViewModel
{
    public class TourReviewsViewModel
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public StatisticsButtonCommand ViewCommand { get; set; }

        public TourReviewsViewModel(int activeGuideId)
        {
            TourOccurrenceService tourOccurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceService.GetFinishedOccurrencesForGuide(activeGuideId));
            ViewCommand = new StatisticsButtonCommand(ViewDetails);
        }

        public void ViewDetails()
        {
            TourGuestReviewsViewModel viewModel = new TourGuestReviewsViewModel(SelectedTourOccurrence.Id);
            TourGuestReviews view = new TourGuestReviews();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
    }
}
