using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class SpecialRequestBookingViewModel
    {
        public TourRequest TourRequest { get; set; }
        public List<DateOnly> FreeDates { get; set; }
        public GuideScheduleService GuideScheduleService { get; set; }
        public UserService UserService { get; set; }
        public TourRequestService TourRequestService { get; set; }
        public User ActiveGuide { get; set; }

        private DateOnly selectedDate;
        public DateOnly SelectedDate
        {
            get { return selectedDate; }
            set 
            {
                isSelected = true;
                selectedDate = value;
            }
        }
        private bool isSelected { get; set; }
        public ButtonCommandNoParameter ConfirmCommand { get; set; }
        public ButtonCommandNoParameter CancelCommand { get; set; }
        public NavigationService NavigationService { get; set; }
        public SpecialRequestBookingViewModel(TourRequest selectedTourRequest, NavigationService navService)
        {
            NavigationService = navService;
            GuideScheduleService = new GuideScheduleService();
            UserService = new UserService();
            TourRequestService = new TourRequestService();
            ActiveGuide = UserService.GetLoggedInUser();
            TourRequest = selectedTourRequest;
            FreeDates = GuideScheduleService.GetFreeDates(ActiveGuide.Id, TourRequest.MinDate, TourRequest.MaxDate, TourRequest.SpecialTourRequestId);
            ConfirmCommand = new ButtonCommandNoParameter(Confirm);
            CancelCommand = new ButtonCommandNoParameter(Cancel);
            isSelected = false;
        }
        public void Confirm()
        {
            if(!isSelected)
            {
                MessageBox.Show("You have to choose a date!", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TourRequestService.BookRequest(TourRequest.Id, ActiveGuide.Id, SelectedDate);
            MessageBox.Show("Tour has been successfuly booked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            Page specialRequests = new SpecialRequestsView(ActiveGuide.Id, NavigationService, TourRequest.Id);
            NavigationService.Navigate(specialRequests);
        }

        public void Cancel()
        {
            Page specialRequests = new SpecialRequestsView(ActiveGuide.Id, NavigationService);
            NavigationService.Navigate(specialRequests);
        }
    }
}
