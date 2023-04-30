using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRequestBookingViewModel : IObserver
    {
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public User ActiveGuide { get; set; }
        public TourRequest SelectedRequest { get; set; }
        public ButtonCommandNoParameter BookCommand { get; set; }
        public TourRequestService TourRequestService { get;set; }
        public TourRequestBookingViewModel(int activeGuideId)
        {
            ActiveGuide = new UserService().GetById(activeGuideId);
            TourRequestService = new TourRequestService();
            TourRequests = new ObservableCollection<TourRequest>(TourRequestService.GetPendings());
            BookCommand = new ButtonCommandNoParameter(ShowRequest);
            TourRequestService.Subscribe(this);
        }

        private void ShowRequest()
        {
            AcceptTourRequestDialogue acceptTourRequest = new AcceptTourRequestDialogue(SelectedRequest, ActiveGuide.Id);
            acceptTourRequest.ShowDialog();
        }

        public void Update()
        {
            TourRequests.Clear();
            foreach (TourRequest request in TourRequestService.GetPendings())
            {
                TourRequests.Add(request);
            }
        }
    }
}
