using System;
using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class SpecialTourRequestsViewModel
    {
        public List<SpecialTourRequest> SpecialTourRequestList { get; set; }
        SpecialTourRequestService service;
        int currentGuestId;
        public SpecialTourRequestsViewModel(int id) 
        {
            currentGuestId = id;
            new TourRequestService();
            service = new SpecialTourRequestService();
            SpecialTourRequestList = service.GetSpecialRequestForGuest(currentGuestId);
            DisplayTitle();
        }
        private void DisplayTitle()
        {
            int i = 1;
            foreach (SpecialTourRequest request in SpecialTourRequestList)
            {
                request.SerialNumber = i++;
                request.BuildRequestString();
            }
        }
    }
}
