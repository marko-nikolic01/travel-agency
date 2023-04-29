using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;

namespace TravelAgency.WPF.Views
{
    public partial class TourRequestView : Page
    {
        private int guestId;
        public ObservableCollection<TourRequest> MadeRequests { get; set; }
        public TourRequestView(int id)
        {
            InitializeComponent();
            DataContext = this;
            TourRequestRepository repository = new TourRequestRepository();
            MadeRequests = new ObservableCollection<TourRequest>(repository.GetAll());
            guestId = id;
        }

        private void CreateRequest_Click(object sender, RoutedEventArgs e)
        {
            TourRequestFormView createRequest = new TourRequestFormView(guestId);
            createRequest.Show();
        }
    }
}
