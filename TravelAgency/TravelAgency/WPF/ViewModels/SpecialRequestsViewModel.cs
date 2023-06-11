using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;
using System.Windows.Controls;
using System.ComponentModel;

namespace TravelAgency.WPF.ViewModels
{
    public class SpecialRequestsViewModel : INotifyPropertyChanged
    {
        public User ActiveGuide { get; set; }
        public UserService UserService { get; set; }
        public GuideScheduleService GuideScheduleService { get; set; }
        public TourRequestService TourRequestService { get; set; }
        public SpecialTourRequestService SpecialTourRequestService { get; set; }
        public TourRequest SelectedTourRequest { get; set; }
        private int bookedRequest;
        public int BookedRequest
        {
            get { return bookedRequest; }
            set 
            { 
                bookedRequest = value;
                OnPropertyChanged();

            }
        }


        private ObservableCollection<SpecialTourRequest> specialTourRequests;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<SpecialTourRequest> SpecialTourRequests
        {
            get { return specialTourRequests; }
            set 
            { 
                specialTourRequests = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ButtonCommandNoParameter BookTourRequestCommand { get; set; }
        public ButtonCommandNoParameter UndoBookCommand { get; set; }
        public NavigationService NavService { get; set; }
        public SpecialRequestsViewModel(int activeGuideId, NavigationService navService, int requestId)
        {
            UserService = new UserService();
            GuideScheduleService = new GuideScheduleService();
            TourRequestService = new TourRequestService();
            NavService = navService;
            ActiveGuide = UserService.GetById(activeGuideId);
            SpecialTourRequestService = new SpecialTourRequestService();
            SpecialTourRequests = new ObservableCollection<SpecialTourRequest>(SpecialTourRequestService.GetOpenSpecialRequest());
            BookTourRequestCommand = new ButtonCommandNoParameter(Book);
            UndoBookCommand = new ButtonCommandNoParameter(UndoBook);
            CheckCanBook();
            if(requestId != -1)
            {
                BookedRequest = requestId;
            }
            else
            {
                BookedRequest = -1;
            }
        }

        private void CheckCanBook()
        {
            foreach(var specialTourRequest in SpecialTourRequests)
            {
                foreach(var tourRequest in specialTourRequest.TourRequests)
                {
                    if (tourRequest.CheckCanBook() && IsGuideFree(tourRequest))
                    {
                        tourRequest.CanBook = true;
                    }
                    else
                    {
                        tourRequest.CanBook = false;
                    }
                }
            }
        }

        private bool IsGuideFree(TourRequest tourRequest)
        {
            if(GuideScheduleService.IsGuideFree(ActiveGuide.Id, tourRequest.MinDate, tourRequest.MaxDate, tourRequest.SpecialTourRequestId))
            {
                return true;
            }
            return false;
        }

        public void Book()
        {
            Page booking = new SpecialRequestBookingView(ActiveGuide.Id, NavService, SelectedTourRequest);
            NavService.Navigate(booking);
        }
        public void UndoBook()
        {
            TourRequestService.UndoBookRequest(BookedRequest);
            BookedRequest = -1;
            SpecialTourRequests = new ObservableCollection<SpecialTourRequest>(SpecialTourRequestService.GetOpenSpecialRequest());
            CheckCanBook();
        }
    }
}
