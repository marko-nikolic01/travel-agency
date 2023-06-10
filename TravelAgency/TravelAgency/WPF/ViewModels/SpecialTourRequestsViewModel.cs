using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class SpecialTourRequestsViewModel : INotifyPropertyChanged
    {
        private bool tourRequestHelpClicked;
        public bool TourRequestHelpClicked
        {
            get { return tourRequestHelpClicked; }
            set { tourRequestHelpClicked = value; OnPropertyChanged(); }
        }
        public ButtonCommandNoParameter TourRequestHelpCommand { get; set; }
        public ButtonCommandNoParameter CreateRequestCommand { get; set; }
        public List<SpecialTourRequest> SpecialTourRequestList { get; set; }
        SpecialTourRequestService service;
        int currentGuestId;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public NavigationService NavigationService { get; set; }
        public SpecialTourRequestsViewModel(int id, NavigationService navService) 
        {
            NavigationService = navService;
            currentGuestId = id;
            new TourRequestService();
            service = new SpecialTourRequestService();
            SpecialTourRequestList = service.GetSpecialRequestForGuest(currentGuestId);
            TourRequestHelpCommand = new ButtonCommandNoParameter(TourRequestClick);
            CreateRequestCommand = new ButtonCommandNoParameter(CreateRequestClick);
            DisplayTitle();
        }
        private void TourRequestClick()
        {
            TourRequestHelpClicked = !TourRequestHelpClicked;
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
        public void CreateRequestClick()
        {
            SpecialTourRequestForm tourRequestForm = new SpecialTourRequestForm(currentGuestId, NavigationService);
            NavigationService.Navigate(tourRequestForm);
        }
    }
}
