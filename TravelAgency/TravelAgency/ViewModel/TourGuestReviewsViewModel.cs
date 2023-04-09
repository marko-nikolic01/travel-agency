using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.Services;

namespace TravelAgency.ViewModel
{
    public class TourGuestReviewsViewModel
    {
        public ObservableCollection<TourReviewViewModel> TourReviews { get; set; }
        public TourGuestReviewsViewModel(int id)
        {
            TourReviews = new ObservableCollection<TourReviewViewModel>(new TourReviewService().getTourReviews(id));
        }
    }
}
