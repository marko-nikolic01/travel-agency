using System.Collections.ObjectModel;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for AlternativeTours.xaml
    /// </summary>
    public partial class AlternativeTours : Window
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public string tourLocation { get; set; }
        private User activeGuest;
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }

        public AlternativeTours(ObservableCollection<TourOccurrence> AllTourOccurrences, int id, Location location, User user, TourOccurrenceRepository tourOccurrenceRepository)
        {
            InitializeComponent();
            DataContext = this;
            TourOccurrences = new ObservableCollection<TourOccurrence>();
            foreach (TourOccurrence tour in AllTourOccurrences)
            {
                if (tour.Id != id && tour.Guests.Count != tour.Tour.MaxGuestNumber)
                {
                    if (tour.Tour.Location.Id == location.Id)
                    {
                        TourOccurrences.Add(tour);
                    }
                }
            }
            activeGuest = user;
            tourLocation = "TOURS IN " + location.City.ToUpper() + ", " + location.Country.ToUpper();
            TourOccurrenceRepository = tourOccurrenceRepository;
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourOccurrence == null)
            {
                MessageBox.Show("You must choose a tour.");
            }
            else
            {
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
