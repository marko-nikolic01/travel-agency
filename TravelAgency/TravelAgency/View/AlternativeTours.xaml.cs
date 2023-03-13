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
        private TourReservationWindow reservationWindow;
        public AlternativeTours(ObservableCollection<TourOccurrence> AllTourOccurrences, int id, Location location, TourReservationWindow reservationWindow=null)
        {
            InitializeComponent();
            DataContext = this;
            TourOccurrences = new ObservableCollection<TourOccurrence>();
            foreach(TourOccurrence tour in AllTourOccurrences) 
            {
                if(tour.Id != id)
                {
                    if(tour.Tour.Location.Id == location.Id)
                    {
                        TourOccurrences.Add(tour);
                    }
                }
            }
            this.reservationWindow = reservationWindow;
            tourLocation = "TOURS IN " + location.City.ToUpper() + ", " + location.Country.ToUpper();
        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTourOccurrence == null)
            {
                MessageBox.Show("You must choose a tour.");
            }
            else
            {
                TourReservationWindow tourReservation = new TourReservationWindow(SelectedTourOccurrence, Guest2Main.TourOccurrences);
                tourReservation.Show();
                Close();
                if (reservationWindow != null)
                {
                    reservationWindow.Close();
                }
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
