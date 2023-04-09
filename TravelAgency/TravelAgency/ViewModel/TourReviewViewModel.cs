using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.ViewModel
{
    public class TourReviewViewModel
    {
        public TourRating TourRating { get; set; }
        public User Guest { get; set; }
        public TourOccurrence TourOccurrence { get; set; }
        public KeyPoint ArrivalKeyPoint { get; set; }

        public TourReviewViewModel(TourRating tourRating)
        {
            TourRating = tourRating;
        }
    }
}
