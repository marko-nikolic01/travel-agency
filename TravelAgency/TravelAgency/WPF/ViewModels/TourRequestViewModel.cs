using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    class TourRequestViewModel
    {
        public ObservableCollection<TourRequest> MadeRequests { get; set; }
        public int guestId;
        public TourRequestViewModel(int id)
        {
            guestId = id;
            TourRequestService requestService = new TourRequestService();
            MadeRequests = new ObservableCollection<TourRequest>(requestService.GetRequestsByGuestId(guestId));
        }
    }
}
