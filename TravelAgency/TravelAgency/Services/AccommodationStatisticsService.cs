using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;
using TravelAgency.Domain.Models;
using Syncfusion.Windows.Shared;
using System.Windows.Media;
using Syncfusion.Windows.PdfViewer;
using TravelAgency.Converters;
using System.Globalization;

namespace TravelAgency.Services
{
    public class AccommodationStatisticsService
    {
        public IUserRepository UserRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }
        public IAccommodationRenovationRepository RenovationRepository { get; set; }
        public IRenovationRecommendationRepository RecommendationRepository { get; set; }
        public IAccommodationOwnerRatingRepository RatingRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IAccommodationReservationMoveRequestRepository MoveRequestRepository { get; set; }

        public AccommodationStatisticsService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();
            RenovationRepository = Injector.Injector.CreateInstance<IAccommodationRenovationRepository>();
            RecommendationRepository = Injector.Injector.CreateInstance<IRenovationRecommendationRepository>();
            RatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            MoveRequestRepository = Injector.Injector.CreateInstance<IAccommodationReservationMoveRequestRepository>();

            AccommodationRepository.LinkOwners(UserRepository.GetAll());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            RatingRepository.LinkRenovationRecommendations(RecommendationRepository.GetAll());
            RenovationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            ReservationRepository.LinkGuests(UserRepository.GetAll());
            MoveRequestRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public AccommodationStatisticsDTO GetStatisticsForAccommodation(Accommodation accommodation)
        {
            var dto = new AccommodationStatisticsDTO(accommodation);
            var activeYears = GetActiveYearsForAccommodation(accommodation);

            foreach (var activeYear in activeYears)
            {
                var statisticsForYear = GetStatisticsForAccommodationByYear(accommodation, activeYear);
                dto.StatisticsByYear.Add(statisticsForYear);
            }

            dto.BusiestYear = GetBusiestYear(dto);

            return dto;
        }

        private string GetBusiestYear(AccommodationStatisticsDTO accommodationStatisticsDTO)
        {
            var busiestYearStats = accommodationStatisticsDTO.StatisticsByYear.OrderByDescending(a => a.NumberOfBusyDays).FirstOrDefault();
            
            if (busiestYearStats != null)
            {
                return busiestYearStats.Year.ToString();
            }

            return string.Empty;
        }

        private string GetBusiestMonth(AccommodationStatisticsByYearDTO accommodationStatisticsByYear)
        {
            var busiestMonthStats = accommodationStatisticsByYear.StatisticsByMonths.OrderByDescending(a => a.NumberOfBusyDays).FirstOrDefault();

            if (busiestMonthStats != null)
            {
                var converter = new IntegerToMonthString();
                return (string)converter.Convert(busiestMonthStats.Month, typeof(string), null, CultureInfo.CurrentCulture);
            }

            return string.Empty;
        }

        private List<int> GetActiveYearsForAccommodation(Accommodation accommodation)
        {
            List<int> activeYears = new List<int>();

            foreach (var reservation in ReservationRepository.GetByAccommodation(accommodation))
            {
                int year = reservation.DateSpan.StartDate.Year;
                if (!activeYears.Contains(year))
                {
                    activeYears.Add(year);
                }
            }

            foreach (var moving in MoveRequestRepository.GetByAccommodation(accommodation))
            {
                int year = moving.RequestDate.Year;
                if (!activeYears.Contains(year))
                {
                    activeYears.Add(year);
                }
            }

            foreach (var rating in RatingRepository.GetByAccommodation(accommodation))
            {
                int year = rating.AccommodationReservation.DateSpan.StartDate.Year;
                if (!activeYears.Contains(year))
                {
                    activeYears.Add(year);
                }
            }

            activeYears.Sort();

            return activeYears;
        }

        private AccommodationStatisticsByYearDTO GetStatisticsForAccommodationByYear(Accommodation accommodation, int year)
        {
            AccommodationStatisticsByYearDTO dto = new AccommodationStatisticsByYearDTO();

            for (int i = 1; i <= 12; i++) {
                var monthStat = GetStatisticsForAccommodationByMonthAndYear(accommodation, year, i);

                dto.Accommodation = accommodation;
                dto.Year = year;
                dto.NumberOfReservations += monthStat.NumberOfReservations;
                dto.NumberOfCancellations += monthStat.NumberOfCancellations;
                dto.NumberOfMovings += monthStat.NumberOfMovings;
                dto.NumberOfRenovationSuggestions += monthStat.NumberOfRenovationSuggestions;
                dto.NumberOfBusyDays += monthStat.NumberOfBusyDays;

                dto.StatisticsByMonths.Add(monthStat);
            }

            dto.BusiestMonth = GetBusiestMonth(dto);

            return dto;
        }

        private AccommodationStatisticsByYearAndMonthDTO GetStatisticsForAccommodationByMonthAndYear(Accommodation accommodation, int year, int month)
        {
            int numberOfReservations = GetNumberOfReservationsForAccommodationByYearAndMonth(accommodation, year, month);
            int numberOfReservationCancellations = GetNumberOfReservationCancellationsForAccommodationByYearAndMonth(accommodation, year, month);
            int numberOfRenovationReccommendations = GetNumberOfRenovationReccommedationsForAccommodationByYearAndMonth(accommodation, year, month);
            int numberOfReservationMovings = GetNumberOfReservationMovingsForAccommodationByYearAndMonth(accommodation, year, month);
            int numberOfBusyDays = GetNumberOfBusyDaysForAccommodationByYearAndMonth(accommodation, year, month);

            return new AccommodationStatisticsByYearAndMonthDTO(accommodation, year, month, numberOfReservations, numberOfReservationCancellations, numberOfReservationMovings, numberOfRenovationReccommendations, numberOfBusyDays);
        }

        private int GetNumberOfReservationsForAccommodationByYearAndMonth(Accommodation accommodation, int year, int month)
        {
            int count = 0;

            foreach (var reservation in ReservationRepository.GetByAccommodation(accommodation))
            {
                var date = reservation.DateSpan.StartDate;
                if (date.Year == year && date.Month == month)
                {
                    if (!reservation.Canceled)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int GetNumberOfReservationCancellationsForAccommodationByYearAndMonth(Accommodation accommodation, int year, int month)
        {
            int count = 0;

            foreach (var reservation in ReservationRepository.GetByAccommodation(accommodation))
            {
                var date = reservation.DateSpan.StartDate;
                if (date.Year == year && date.Month == month)
                {
                    if (reservation.Canceled)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int GetNumberOfReservationMovingsForAccommodationByYearAndMonth(Accommodation accommodation, int year, int month)
        {
            int count = 0;

            foreach (var moveRequest in MoveRequestRepository.GetByAccommodation(accommodation))
            {
                var date = moveRequest.DateSpan.StartDate;
                if (date.Year == year && date.Month == month)
                {
                    if (moveRequest.Status == AccommodationReservationMoveRequestStatus.ACCEPTED)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int GetNumberOfRenovationReccommedationsForAccommodationByYearAndMonth(Accommodation accommodation, int year, int month)
        {
            int count = 0;

            foreach (var rating in RatingRepository.GetByAccommodation(accommodation))
            {
                var date = rating.AccommodationReservation.DateSpan.StartDate;
                if (date.Year == year && date.Month == month)
                {
                    if (rating.RenovationReccommendationId != -1)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int GetNumberOfBusyDaysForAccommodationByYearAndMonth(Accommodation accommodation, int year, int month)
        {
            int count = 0;

            foreach (var reservation in ReservationRepository.GetByAccommodation(accommodation))
            {
                var date = reservation.DateSpan.StartDate;
                if (date.Year == year && date.Month == month)
                {
                    if (!reservation.Canceled)
                    {
                        count += reservation.DateSpan.EndDate.DayNumber - reservation.DateSpan.StartDate.DayNumber;
                    }
                }
            }

            return count;
        }
    }
}
