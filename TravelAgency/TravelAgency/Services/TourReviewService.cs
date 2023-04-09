using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.View;
using TravelAgency.ViewModel;

namespace TravelAgency.Services
{
    public class TourReviewService
    {
        public TourReviewService()
        {
        }

        public List<TourReviewViewModel> getTourReviews(int id)
        {
            List <TourReviewViewModel> tourReviews = new List <TourReviewViewModel> ();
            foreach (TourRating rating in new TourRatingRepository(new TourRatingPhotoRepository()).GetRatingsByTourOccurrenceId(id))
            {
                TourReviewViewModel tourReviewViewModel = new TourReviewViewModel(rating);
                tourReviewViewModel.Guest = new UserRepository().GetById(rating.GuestId);
                tourReviewViewModel.TourOccurrence = new TourOccurrenceRepository(new TourRepository()).GetById(rating.TourOccurrenceId);
                TourOccurrenceAttendance tourOccurrenceAttendance = new TourOccurrenceAttendanceRepository().GetByTourOccurrenceIdAndGuestId(tourReviewViewModel.TourOccurrence.Id, tourReviewViewModel.Guest.Id);
                tourReviewViewModel.ArrivalKeyPoint = new KeyPointRepository().GetById(tourOccurrenceAttendance.KeyPointId);
                tourReviews.Add(tourReviewViewModel);
            }
            return tourReviews;
        }
    }
}
