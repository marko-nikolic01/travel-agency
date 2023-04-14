using System.Collections.ObjectModel;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    public partial class TourRequestWindow : Window
    {
        private int guestId;
        public ObservableCollection<TourRequest> MadeRequests { get; set; }
        public TourRequestWindow(int id)
        {
            InitializeComponent();
            DataContext = this;
            TourRequestRepository repository = new TourRequestRepository();
            MadeRequests = new ObservableCollection<TourRequest>(repository.GetAll());
            guestId = id;
        }

        private void CreateRequest_Click(object sender, RoutedEventArgs e)
        {
            TourRequestFormWindow createRequest = new TourRequestFormWindow(guestId);
            createRequest.Show();
        }
    }
}
