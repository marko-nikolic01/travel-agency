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

        public List<TourDetailsViewModel> getTourReviews(int id)
        {
            List <TourDetailsViewModel> tourReviews = new List <TourDetailsViewModel> ();
            foreach (TourRating rating in new TourRatingRepository(new TourRatingPhotoRepository()).GetRatingsByTourOccurrenceId(id))
            {
                TourDetailsViewModel tourReviewViewModel = new TourDetailsViewModel(rating);
                tourReviewViewModel.Guest = new UserRepository().GetById(rating.GuestId);
                tourReviewViewModel.TourOccurrence = new TourOccurrenceRepository(new TourRepository()).GetById(rating.TourOccurrenceId);
                TourOccurrenceAttendance tourOccurrenceAttendance = new TourOccurrenceAttendanceRepository().GetByTourOccurrenceIdAndGuestId(tourReviewViewModel.TourOccurrence.Id, tourReviewViewModel.Guest.Id);
                tourReviewViewModel.ArrivalKeyPoint = new KeyPointRepository().GetById(tourOccurrenceAttendance.KeyPointId);
                tourReviews.Add(tourReviewViewModel);
            }
            return tourReviews;
        }

        public void UpdateTourRatingIsValid(TourRating tourRating)
        {
            new TourRatingRepository().UpdateIsValid(tourRating);
        }
    }
}
