using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for UpcomingTours.xaml
    /// </summary>
    public partial class UpcomingTours : Window, IObserver
    {
        public Model.User ActiveGuide { get; set; }
        public TourRepository TourRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public KeyPointRepository KeyPointRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public TourReservationRepository? TourReservationRepository { get; set; }
        public TourOccurrenceAttendanceRepository TourOccurrenceAttendanceRepository { get; set; }
        public VoucherRepository VoucherRepository { get; set; }
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence? SelectedTourOccurrence { get; set; }
        public UpcomingTours(Model.User activeGuide, TourRepository tourRepository, LocationRepository locationRepository, PhotoRepository photoRepository, TourOccurrenceRepository tourOccurrenceRepository, KeyPointRepository keyPointRepository,
            TourReservationRepository? tourReservationRepository, UserRepository userRepository, TourOccurrenceAttendanceRepository tourOccurrenceAttendanceRepository, VoucherRepository voucherRepository)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = activeGuide;
            TourRepository = tourRepository;
            LocationRepository = locationRepository;
            PhotoRepository = photoRepository;
            TourOccurrenceRepository = tourOccurrenceRepository;
            KeyPointRepository = keyPointRepository;
            TourReservationRepository = tourReservationRepository;
            UserRepository = userRepository;
            VoucherRepository = voucherRepository;
            TourOccurrenceAttendanceRepository = tourOccurrenceAttendanceRepository;
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceRepository.GetUpcomings(ActiveGuide));
            tourOccurrenceRepository.Subscribe(this);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var guest in SelectedTourOccurrence.Guests)
                {
                    VoucherRepository.Save(new Voucher() { GuestId = guest.Id, GuideId = ActiveGuide.Id });
                }
                TourOccurrenceRepository.Delete(SelectedTourOccurrence);
            }
        }

        public void Update()
        {
            TourOccurrences.Clear();
            foreach (TourOccurrence tourOccurrence in TourOccurrenceRepository.GetUpcomings(ActiveGuide))
            {
                TourOccurrences.Add(tourOccurrence);
            }
        }
    }
}
