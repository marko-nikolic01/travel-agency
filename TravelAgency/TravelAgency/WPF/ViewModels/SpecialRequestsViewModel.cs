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

namespace TravelAgency.WPF.ViewModels
{
    public class SpecialRequestsViewModel
    {
        public User ActiveGuide { get; set; }
        public UserService UserService { get; set; }
        public SpecialTourRequestService SpecialTourRequestService { get; set; }

        private TourRequest selectedTourRequest;
        public TourRequest SelectedTourRequest
    {
            get { return selectedTourRequest; }
            set 
            { 
                selectedTourRequest = value;
            }
        }

        public ObservableCollection<SpecialTourRequest> SpecialTourRequests { get; set; }
        public ButtonCommandNoParameter BookTourRequestCommand { get; set; }
        
        
        public SpecialRequestsViewModel(int activeGuideId, NavigationService navService)
        {
            UserService = new UserService();
            ActiveGuide = UserService.GetById(activeGuideId);
            SpecialTourRequestService = new SpecialTourRequestService();
            SpecialTourRequests = new ObservableCollection<SpecialTourRequest>(SpecialTourRequestService.GetOpenSpecialRequest());
            BookTourRequestCommand = new ButtonCommandNoParameter(Book);
        }
        public void Book()
        {
            MessageBox.Show(SelectedTourRequest.Location.City);
        }
    }
}
