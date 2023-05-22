using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

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
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public TourRequestViewModel(int id)
        {
            guestId = id;
            TourRequestService requestService = new TourRequestService();
            MadeRequests = new ObservableCollection<TourRequest>(requestService.GetRequestsByGuestId(guestId));
            TourRequestHelpCommand = new ButtonCommandNoParameter(TourRequestClick);
        }
        private void TourRequestClick()
        {
            TourRequestHelpClicked = !TourRequestHelpClicked;
        }
    }
}
