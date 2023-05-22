using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Injector;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class RenovationService
    {
        public IUserRepository UserRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        public IAccommodationRenovationRepository RenovationRepository { get; set; }
        public IRenovationRecommendationRepository RecommendationRepository { get; set; }
        public IAccommodationOwnerRatingRepository RatingRepository { get; set; }

        private AccommodationDateFinderService accommodationDateFinderService;


        public RenovationService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            RenovationRepository = Injector.Injector.CreateInstance<IAccommodationRenovationRepository>();
            RecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
            RatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();

            AccommodationRepository.LinkOwners(UserRepository.GetAll());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            RatingRepository.LinkRenovationRecommendations(RecommendationRepository.GetAll());
            RenovationRepository.LinkAccommodations(AccommodationRepository.GetAll());

            accommodationDateFinderService = new AccommodationDateFinderService();
        }

        public bool RecommendRenovation(AccommodationOwnerRating rating, RenovationRecommendation recommendation)
        {
            if (recommendation.IsValid)
            {
                recommendation.RatingId = rating.Id;
                RecommendationRepository.Save(recommendation);
                rating.RenovationReccommendationId = recommendation.Id;
                rating.RenovationRecommendation = recommendation;
                RatingRepository.SaveAll();
                return true;
            }
            return false;
        }

        public List<AccommodationRenovation> GetPastRenovationsByOwner(User owner)
        {
            var renovations = RenovationRepository.GetAll();
            var filtered = new List<AccommodationRenovation>();

            foreach (var renovation in renovations)
            {
                if (!IsRenovationActive(renovation))
                {
                    filtered.Add(renovation);
                }
            }

            return filtered;
        }

        public List<AccommodationRenovation> GetScheduledRenovationsByOwner(User owner)
        {
            var renovations = RenovationRepository.GetByOwner(owner);
            var filtered = new List<AccommodationRenovation>();

            foreach (var renovation in renovations)
            {
                if (IsRenovationActive(renovation))
                {
                    filtered.Add(renovation);
                }
            }

            return filtered;
        }

        public bool ScheduleRenovation(AccommodationRenovation renovation)
        {
            if (CanRenovationBeScheduled(renovation))
            {
                RenovationRepository.Save(renovation);
                return true;
            }

            return false;
        }

        private bool IsRenovationActive(AccommodationRenovation renovation)
        {
            return DateOnly.FromDateTime(DateTime.Now).CompareTo(renovation.DateSpan.EndDate) <= 0;
        }

        public bool CancelRenovation(AccommodationRenovation renovation)
        {
            if (CanRenovationBeCancelled(renovation))
            {
                RenovationRepository.Delete(renovation);
                return true;
            }

            return false;
        }

        public bool CanRenovationBeCancelled(AccommodationRenovation renovation)
        {
            return renovation.DateSpan.StartDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber > 5;
        }

        public bool IsAccommodationRenovatedInTheLastYear(Accommodation accommodation)
        {
            foreach (var renovation in RenovationRepository.GetByAccommodation(accommodation))
            {
                if (IsRenovationInTheLastYear(renovation))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsRenovationInTheLastYear(AccommodationRenovation renovation)
        {
            return IsDateInTheLastYear(renovation.DateSpan.EndDate);
        }

        private bool IsDateInTheLastYear(DateOnly date)
        {
            int days = DateOnly.FromDateTime(DateTime.Now).DayNumber - date.DayNumber;
            return 0 < days && days < 365;
        }

        public List<AccommodationWithRenovationDTO> GetAccommodationswWithRenovations()
        {
            List<AccommodationWithRenovationDTO> dtos = new List<AccommodationWithRenovationDTO>();

            foreach (var accommodation in AccommodationRepository.GetAll())
            {
                dtos.Add(new AccommodationWithRenovationDTO(accommodation, IsAccommodationRenovatedInTheLastYear(accommodation)));
            }

            return dtos;
        }

        public void SetRenovationStatus(List<Accommodation> accommodations)
        {
            foreach (var accommodation in accommodations)
            {
                accommodation.IsRenovated = IsAccommodationRenovatedInTheLastYear(accommodation);
            }
        }

        public bool CanRenovationBeScheduled(AccommodationRenovation renovation)
        {
            return accommodationDateFinderService.IsDateSpanAvailable(renovation.Accommodation, renovation.DateSpan.StartDate, renovation.DateSpan.EndDate);
        }
    }
}
