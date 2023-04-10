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
        public ObservableCollection<TourReviewViewModel> TourReviews { get; set; }
        public ButtonCommand<TourReviewViewModel> ReportCommand { get; set; }
        public ButtonCommand<TourReviewViewModel> RightCommand { get; set; }
        public ButtonCommand<TourReviewViewModel> LeftCommand { get; set; }

        public TourGuestReviewsViewModel(int id)
        {
            TourReviewService tourReviewService = new TourReviewService();
            TourReviews = new ObservableCollection<TourReviewViewModel>(tourReviewService.getTourReviews(id));
            ReportCommand = new ButtonCommand<TourReviewViewModel>(ReportNotValid);
            RightCommand = new ButtonCommand<TourReviewViewModel>(ShowNextPhoto);
            LeftCommand = new ButtonCommand<TourReviewViewModel>(ShowPreviousPhoto);
        }

        private void ReportNotValid(TourReviewViewModel tourReviewViewModel)
        {
            tourReviewViewModel.TourRating.IsValid = false;
            TourReviewService tourReviewService = new TourReviewService();
            tourReviewService.UpdateTourRatingIsValid(tourReviewViewModel.TourRating);
        }

        private void ShowNextPhoto(TourReviewViewModel tourReviewViewModel)
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

        private void ShowPreviousPhoto(TourReviewViewModel tourReviewViewModel)
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
