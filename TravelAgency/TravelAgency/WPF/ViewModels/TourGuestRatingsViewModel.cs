using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class TourGuestRatingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TourDetailsViewModel> TourReviews { get; set; }
        public ButtonCommand<TourDetailsViewModel> ReportCommand { get; set; }
        public ButtonCommand<TourDetailsViewModel> RightCommand { get; set; }
        public ButtonCommand<TourDetailsViewModel> LeftCommand { get; set; }
        public TourRatingService TourRatingService { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public TourGuestRatingsViewModel(int id)
        {
            TourRatingService = new TourRatingService();
            TourReviews = new ObservableCollection<TourDetailsViewModel>(TourRatingService.getTourReviews(id));
            ReportCommand = new ButtonCommand<TourDetailsViewModel>(ReportNotValid);
            RightCommand = new ButtonCommand<TourDetailsViewModel>(ShowNextPhoto);
            LeftCommand = new ButtonCommand<TourDetailsViewModel>(ShowPreviousPhoto);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ReportNotValid(TourDetailsViewModel tourReviewViewModel)
        {
            if (tourReviewViewModel.TourRating.IsValid)
            {
                tourReviewViewModel.TourRating.IsValid = false;
                TourRatingService.UpdateTourRatingIsValid(tourReviewViewModel.TourRating);
            }
            else
            {
                tourReviewViewModel.TourRating.IsValid = true;
                TourRatingService.UpdateTourRatingIsValid(tourReviewViewModel.TourRating);
            }
        }
        private void ShowNextPhoto(TourDetailsViewModel tourReviewViewModel)
        {
            for (int i = 0; i < tourReviewViewModel.TourRating.PhotoUrls.Count; i++)
            {
                if (tourReviewViewModel.CurrentPhoto.Id == tourReviewViewModel.TourRating.PhotoUrls[i].Id)
                {
                    if (i < tourReviewViewModel.TourRating.PhotoUrls.Count - 1)
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
                        tourReviewViewModel.CurrentPhoto = tourReviewViewModel.TourRating.PhotoUrls[tourReviewViewModel.TourRating.PhotoUrls.Count - 1];
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
