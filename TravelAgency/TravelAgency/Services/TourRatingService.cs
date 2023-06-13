using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;
using TravelAgency.WPF.Views;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.Services
{
    public class TourRatingService
    {
        public IUserRepository IUserRepository { get; set; }
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public ITourRatingRepository ITourRatingRepository { get; set; }
        public IKeyPointRepository IKeyPointRepository { get; set; }
        public ITourOccurrenceAttendanceRepository ITourOccurrenceAttendanceRepository { get; set; }
        public ITourRatingPhotoRepository ITourRatingPhotoRepository { get; set; }
        public TourRatingService()
        {
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            ITourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
            IKeyPointRepository = Injector.Injector.CreateInstance<IKeyPointRepository>();
            ITourOccurrenceAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
            ITourRatingPhotoRepository = Injector.Injector.CreateInstance<ITourRatingPhotoRepository>();
            LinkRatingPhoto();
        }
        private void LinkRatingPhoto()
        {
            foreach (TourRating tourRating in ITourRatingRepository.GetAll())
            {
                if (tourRating.PhotoUrls != null)
                {
                    tourRating.PhotoUrls.Clear();
                }
            }
            foreach (TourRatingPhoto tourRatingPhoto in ITourRatingPhotoRepository.GetAll())
            {
                TourRating tourRating = ITourRatingRepository.GetAll().Find(t => t.Id == tourRatingPhoto.TourRatingId);
                if (tourRating != null)
                {
                    tourRating.PhotoUrls.Add(tourRatingPhoto);
                }
            }
        }
        public List<TourDetailsViewModel> getTourReviews(int id)
        {
            List<TourDetailsViewModel> tourReviews = new List<TourDetailsViewModel>();
            foreach (TourRating rating in ITourRatingRepository.GetRatingsByTourOccurrenceId(id))
            {
                TourDetailsViewModel tourReviewViewModel = new TourDetailsViewModel(rating);
                tourReviewViewModel.Guest = IUserRepository.GetById(rating.GuestId);
                tourReviewViewModel.TourOccurrence = ITourOccurrenceRepository.GetById(rating.TourOccurrenceId);
                TourOccurrenceAttendance tourOccurrenceAttendance = ITourOccurrenceAttendanceRepository.GetByTourOccurrenceIdAndGuestId(tourReviewViewModel.TourOccurrence.Id, tourReviewViewModel.Guest.Id);
                tourReviewViewModel.ArrivalKeyPoint = IKeyPointRepository.GetById(tourOccurrenceAttendance.KeyPointId);
                tourReviews.Add(tourReviewViewModel);
            }
            return tourReviews;
        }
        public TourRating SaveTourRating(TourRating tourRating)
        {
            return ITourRatingRepository.Save(tourRating);
        }
        public void SaveTourRatingPhoto(TourRatingPhoto tourRatingPhoto)
        {
            ITourRatingPhotoRepository.Save(tourRatingPhoto);
        }
        public void UpdateTourRatingIsValid(TourRating tourRating)
        {
            ITourRatingRepository.UpdateIsValid(tourRating);
        }
        public bool IsTourNotRated(int guestId, int occurrenceId)
        {
            return ITourRatingRepository.IsTourNotRated(guestId, occurrenceId);
        }
        public int GetNumberForGuest(int guestId)
        {
            return ITourRatingRepository.GetRatingsNumberByGuestId(guestId);
        }
        public double GetAverageGrade(int id)
        {
            double sum = 0.0;
            int ratingsCount = 0;
            foreach(var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                foreach(var tourRating in ITourRatingRepository.GetRatingsByTourOccurrenceId(tourOccurrence.Id))
                {
                    sum += tourRating.GuideLanguage;
                    sum += tourRating.GuideKnowledge;
                    sum += tourRating.Interesting;
                    ratingsCount++;
                }
            }
            sum /= 3.0;
            sum /= (double)ratingsCount;
            return sum;
        }
        public double GetAverageGradesForLanguage(int id, string l)
        {
            double sum = 0.0;
            int ratingsCount = 0;
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                if (tourOccurrence.Tour.Language.Equals(l))
                {
                    foreach (var tourRating in ITourRatingRepository.GetRatingsByTourOccurrenceId(tourOccurrence.Id))
                    {
                        sum += tourRating.GuideLanguage;
                        sum += tourRating.GuideKnowledge;
                        sum += tourRating.Interesting;
                        ratingsCount++;
                    }
                }
            }
            sum /= 3.0;
            sum /= (double)ratingsCount;
            return sum;
        }
        public string[] GetUniqeLanguages(int id)
        {
            HashSet<string> uniqueLanguages = new HashSet<string>();
            foreach (var r in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                uniqueLanguages.Add(r.Tour.Language);
            }
            return uniqueLanguages.ToArray<string>();
        }
        
        
        public Dictionary<string, double> GetAverageGradesForLanguages(int id)
        {
            Dictionary<string, double> grades = new Dictionary<string, double>();
            foreach(string l in GetUniqeLanguages(id))
            {
                grades[l] = GetAverageGradesForLanguage(id, l);
            }
            return grades;
        }
    }
}
