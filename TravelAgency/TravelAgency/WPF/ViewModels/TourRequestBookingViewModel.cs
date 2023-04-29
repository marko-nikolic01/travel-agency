using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRequestBookingViewModel
    {
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public User ActiveGuide { get; set; }
        public TourRequest SelectedRequest { get; set; }
        public ButtonCommandNoParameter BookCommand { get; set; }
        public TourRequestBookingViewModel(int activeGuideId)
        {
            ActiveGuide = new UserService().GetById(activeGuideId);
            TourRequests = new ObservableCollection<TourRequest>(new TourRequestService().GetPendings());
            BookCommand = new ButtonCommandNoParameter(ShowRequest);
        }

        private void ShowRequest()
        {
            AcceptTourRequestDialogue acceptTourRequest = new AcceptTourRequestDialogue(SelectedRequest);
            acceptTourRequest.ShowDialog();
        }
    }
}
