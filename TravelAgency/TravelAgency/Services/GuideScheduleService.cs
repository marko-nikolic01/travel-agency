using LiveCharts.Maps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class GuideScheduleService
    {
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public ILocationRepository ILocationRepository { get; set; }
        public IUserRepository IUserRepository { get; set; }
        public ITourRequestRepository ITourRequestRepository { get; set; }
        public GuideScheduleService()
        {
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            LinkTourGuide();
        }
        private void LinkTourGuide()
        {
            foreach (TourOccurrence tourOccurrence in ITourOccurrenceRepository.GetAll())
            {
                User user = IUserRepository.GetById(tourOccurrence.GuideId);
                if (user != null)
                {
                    tourOccurrence.Guide = user;
                }
            }
        }
        public bool IsGuideFree(int id, DateOnly minDate, DateOnly maxDate, int specialId)
        {
            List<TourOccurrence> tourOccurrences = ITourOccurrenceRepository.GetUpcomings(id);
            HashSet<DateOnly> uniqueDates = new HashSet<DateOnly>();
            foreach (var tourOccurrence in tourOccurrences)
            {
                uniqueDates.Add(DateOnly.FromDateTime(tourOccurrence.DateTime));
            }
            foreach (var tourRequest in ITourRequestRepository.GetAccepted(specialId))
            {
                if(tourRequest.GuideId == id)
                {
                    return false;
                }
                DateOnly date = DateOnly.ParseExact(tourRequest.GivenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                uniqueDates.Add(date);
            }
            for (DateOnly dO = minDate; dO <= maxDate; dO = dO.AddDays(1))
            {
                if (!uniqueDates.Contains(dO))
                {
                    return true;
                }
            }
            return false;
        }
        public List<DateOnly> GetFreeDates(int id, DateOnly minDate, DateOnly maxDate, int specialId)
        {
            List<TourOccurrence> tourOccurrences = ITourOccurrenceRepository.GetUpcomings(id);
            HashSet<DateOnly> uniqueDates = new HashSet<DateOnly>();
            for (DateOnly dO = minDate; dO <= maxDate; dO = dO.AddDays(1))
            {
                uniqueDates.Add(dO);
            }
            foreach (var tourOccurrence in tourOccurrences)
            {
                if (uniqueDates.Contains(DateOnly.FromDateTime(tourOccurrence.DateTime)))
                {
                    uniqueDates.Remove(DateOnly.FromDateTime(tourOccurrence.DateTime));
                }
            }
            foreach(var tourRequest in ITourRequestRepository.GetAccepted(specialId))
            {
                DateOnly date = DateOnly.ParseExact(tourRequest.GivenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if(uniqueDates.Contains(date))
                {
                    uniqueDates.Remove(date);
                }
            }
            foreach (var tourRequest in ITourRequestRepository.GetAcceptedForGuide(id))
            {
                DateOnly date = DateOnly.ParseExact(tourRequest.GivenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (uniqueDates.Contains(date))
                {
                    uniqueDates.Remove(date);
                }
            }
            return uniqueDates.ToList();
        }
    }
}
