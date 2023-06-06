using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class GuideProfileViewModel
    {
        public UserService UserService { get; set; }
        public TourOccurrenceService TourOccurrenceService{ get; set; }
        public TourRatingService TourRatingService { get; set; }
        public User Guide { get; set; }
        public int FinishedTours { get; set; }
        public double AverageGrade { get; set; }
        public GuideProfileViewModel()
        {
            UserService = new UserService();
            Guide = UserService.GetLoggedInUser();
            TourOccurrenceService = new TourOccurrenceService();
            FinishedTours = TourOccurrenceService.GetFinishedToursById(Guide.Id);
            TourRatingService = new TourRatingService();
            AverageGrade = TourRatingService.GetAverageGrade(Guide.Id);
        }
    }
}
