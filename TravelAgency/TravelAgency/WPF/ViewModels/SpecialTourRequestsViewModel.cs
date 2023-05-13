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
            service = new SpecialTourRequestService();
            Location lokacija = new Location(0, "grad", "drzava", "fulnejm");
            DateOnly date1 = new DateOnly(2020,2,2);
            DateOnly date2 = new DateOnly(2020,2,5);
            TourRequest tourRequest1 = new TourRequest(lokacija, "opis", "jezik", 23, date1, date2, 0, RequestStatus.Pending, date1);        
            TourRequest tourRequest2 = new TourRequest(lokacija, "opis", "jezik", 23, date1, date2, 0, RequestStatus.Pending, date1);        
            TourRequest tourRequest3 = new TourRequest(lokacija, "opis", "jezik", 23, date1, date2, 0, RequestStatus.Pending, date1);        
            TourRequest tourRequest4 = new TourRequest(lokacija, "opis", "jezik", 23, date1, date2, 0, RequestStatus.Pending, date1);
            SpecialTourRequestList = new List<SpecialTourRequest>();
            SpecialTourRequest specialRequest1 = new SpecialTourRequest();
            specialRequest1.Id = 1;
            specialRequest1.TourRequests.Add(tourRequest1);
            specialRequest1.TourRequests.Add(tourRequest2);
            specialRequest1.Status = SpecialRequestStatus.Pending;
            SpecialTourRequest specialRequest2 = new SpecialTourRequest();
            specialRequest2.Id = 2;
            specialRequest2.TourRequests.Add(tourRequest3);
            specialRequest2.TourRequests.Add(tourRequest4);
            specialRequest2.Status = SpecialRequestStatus.Pending;
            specialRequest1.SerialNumber = 1;
            specialRequest2.SerialNumber = 2;
            specialRequest1.BuildRequestString();
            specialRequest2.BuildRequestString();
            SpecialTourRequestList.Add(specialRequest1);
            SpecialTourRequestList.Add(specialRequest2);
        }
        
    }
}
