using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Commands;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Services;
using TravelAgency.View;

namespace TravelAgency.ViewModel
{
    public class TourGuestReviewsViewModel
    {
        public ObservableCollection<TourDetailsViewModel> TourReviews { get; set; }
        public ButtonCommand<TourDetailsViewModel> ReportCommand { get; set; }
        public ButtonCommand<TourDetailsViewModel> RightCommand { get; set; }
        public ButtonCommand<TourDetailsViewModel> LeftCommand { get; set; }

        public TourGuestReviewsViewModel(int id)
        {
            TourReviewService tourReviewService = new TourReviewService();
            TourReviews = new ObservableCollection<TourDetailsViewModel>(tourReviewService.getTourReviews(id));
            ReportCommand = new ButtonCommand<TourDetailsViewModel>(ReportNotValid);
            RightCommand = new ButtonCommand<TourDetailsViewModel>(ShowNextPhoto);
            LeftCommand = new ButtonCommand<TourDetailsViewModel>(ShowPreviousPhoto);
        }

        private void ReportNotValid(TourDetailsViewModel tourReviewViewModel)
        {
            tourReviewViewModel.TourRating.IsValid = false;
            TourReviewService tourReviewService = new TourReviewService();
            tourReviewService.UpdateTourRatingIsValid(tourReviewViewModel.TourRating);
        }

        private void ShowNextPhoto(TourDetailsViewModel tourReviewViewModel)
        {
            for (int i = 0; i < tourReviewViewModel.TourRating.PhotoUrls.Count; i++)
            {
                if (tourReviewViewModel.CurrentPhoto.Id == tourReviewViewModel.TourRating.PhotoUrls[i].Id)
                {
                    if(i < tourReviewViewModel.TourRating.PhotoUrls.Count - 1)
                    {
                        tourReviewViewModel.CurrentPhoto = tourReviewViewModel.TourRating.PhotoUrls[++i];
                        return;
                    }
                    else
                    {
                        tourReviewViewModel.CurrentPhoto = tourReviewViewModel.TourRating.PhotoUrls[0];
                        return;
                    }
                }
            }
            return;
        }

        private void ShowPreviousPhoto(TourDetailsViewModel tourReviewViewModel)
        {
            for (int i = 0; i < tourReviewViewModel.TourRating.PhotoUrls.Count; i++)
            {
                if (tourReviewViewModel.CurrentPhoto.Id == tourReviewViewModel.TourRating.PhotoUrls[i].Id)
                {
                    if (i == 0)
                    {
                        tourReviewViewModel.CurrentPhoto = tourReviewViewModel.TourRating.PhotoUrls[tourReviewViewModel.TourRating.PhotoUrls.Count-1];
                        return;
                    }
                    else
                    {
                        tourReviewViewModel.CurrentPhoto = tourReviewViewModel.TourRating.PhotoUrls[--i];
                        return;
                    }
                }
            }
            return;
        }
    }
}
