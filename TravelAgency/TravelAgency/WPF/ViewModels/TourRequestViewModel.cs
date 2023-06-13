using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    class TourRequestViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TourRequest> MadeRequests { get; set; }
        public int guestId;
        private bool tourRequestHelpClicked;
        public bool TourRequestHelpClicked
        {
            get { return tourRequestHelpClicked; }
            set { tourRequestHelpClicked = value; OnPropertyChanged(); }
        }
        public ButtonCommandNoParameter TourRequestHelpCommand { get; set; }
        public ButtonCommandNoParameter NewRequestCommand { get; set; }
        public ButtonCommandNoParameter StatisticsCommand { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public NavigationService NavigationService { get; set; }
        public TourRequestViewModel(int id, NavigationService navService)
        {
            NavigationService = navService;
            guestId = id;
            TourRequestService requestService = new TourRequestService();
            MadeRequests = new ObservableCollection<TourRequest>(requestService.GetRequestsByGuestId(guestId));
            TourRequestHelpCommand = new ButtonCommandNoParameter(TourRequestClick);
            NewRequestCommand = new ButtonCommandNoParameter(NewRequestClick);
            StatisticsCommand = new ButtonCommandNoParameter(StatisticsClick);
        }
        private void TourRequestClick()
        {
            TourRequestHelpClicked = !TourRequestHelpClicked;
        }
        public void NewRequestClick()
        {
            TourRequestFormView requestFormView = new TourRequestFormView(guestId, NavigationService);
            NavigationService.Navigate(requestFormView);
        }
        public void StatisticsClick()
        {
            CreatedRequestsStatistics requestsStatistics = new CreatedRequestsStatistics(guestId);
            NavigationService.Navigate(requestsStatistics);
        }
    }
}
