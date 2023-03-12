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
using TravelAgency.Repository;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for Guest2Main.xaml
    /// </summary>
    public partial class Guest2Main : Window
    {
        public static ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private List<TourOccurrence> toursList;
        public TourRepository TourRepository { get; set; }
        public TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }

        public Guest2Main()
        {
            InitializeComponent();
            DataContext = this;
            TourRepository = new TourRepository();
            LocationRepository = new LocationRepository();
            PhotoRepository = new PhotoRepository();
            TourOccurrenceRepository = new TourOccurrenceRepository();
            LinkingTourLocation();
            LinkingTourOccurrences();
            LinkingTourImages();
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceRepository.GetTourOccurrences());
            toursList = TourOccurrences.ToList();
        }

        private void ReserveClick(object sender, RoutedEventArgs e)
        {
            if (SelectedTourOccurrence.Guests.Count == SelectedTourOccurrence.Tour.MaxGuestNumber)
            {
                AlternativeTours alternativeTours = new AlternativeTours(TourOccurrences, SelectedTourOccurrence.Id, SelectedTourOccurrence.Tour.Location);
                alternativeTours.Show();
            }
            else
            {
                TourReservation tourReservation = new TourReservation(SelectedTourOccurrence, TourOccurrences);
                tourReservation.Show();
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            bool tbCityEmpty = true, tbDurEmpty = true, tbCountryEmpty = true, tbLanguageEmpty = true, tbNumOfGuestsEmpty = true;
            if(tbCity.Text != "" || tbCountry.Text != "" || tbLanguage.Text != "" || tbDuration.Text != "" || tbNumOfGuests.Text != "")
            {
                if(tbCity.Text != "")
                    tbCityEmpty= false;
                if (tbCountry.Text != "")
                    tbCountryEmpty = false;
                if (tbDuration.Text != "")
                    tbDurEmpty = false;
                if (tbLanguage.Text != "")
                    tbLanguageEmpty = false;
                if (tbNumOfGuests.Text != "")
                    tbNumOfGuestsEmpty = false;
                
                var filteredList = FilterList(tbCityEmpty, tbDurEmpty, tbCountryEmpty, tbLanguageEmpty, tbNumOfGuestsEmpty);
                ToursDataGrid.ItemsSource = filteredList;
            }
            else
            {
                ToursDataGrid.ItemsSource = TourOccurrences;
            }
        }
        //proverava da li tekst iz textbox zadovaljava kriterijum ili ako je prazan textbox onda svakako zadovoljava kriterijum
        private IEnumerable<TourOccurrence> FilterList(bool tbCityEmpty, bool tbDurEmpty, bool tbCountryEmpty, bool tbLanguageEmpty, bool tbNumOfGuestsEmpty)
        {
            int numOfGuests;
            if (!tbNumOfGuestsEmpty)
                numOfGuests = int.Parse(tbNumOfGuests.Text);
            else
                numOfGuests = 0;
            return toursList.Where(x => (x.Tour.Location.City.ToLower().Contains(tbCity.Text) || tbCityEmpty) &&
                                                        (x.Tour.Location.Country.ToLower().Contains(tbCountry.Text) || tbCountryEmpty) &&
                                                        (x.Tour.Language.ToLower().Contains(tbLanguage.Text) || tbLanguageEmpty) &&
                                                        (x.Tour.Duration.ToString().Contains(tbDuration.Text) || tbDurEmpty) &&
                                                        ((x.Tour.MaxGuestNumber - x.Guests.Count) >= numOfGuests));
        }

        private void LinkingTourLocation()
        {
            foreach (var tour in TourRepository.GetTours())
            {
                Location location = LocationRepository.GetLocations().Find(l => l.Id == tour.Id);
                if (location != null)
                {
                    tour.Location = location;
                }
            }
        }
        private void LinkingTourOccurrences()
        {
            foreach (TourOccurrence tourOccurrence in TourOccurrenceRepository.GetTourOccurrences())
            {
                Tour tour = TourRepository.GetTours().Find(t => t.Id == tourOccurrence.TourId);
                if (tour != null)
                {
                    tourOccurrence.Tour = tour;
                }
            }
        }

        private void LinkingTourImages()
        {
            foreach (Photo photo in PhotoRepository.GetPhotos())
            {
                Tour tour = TourRepository.GetTours().Find(t => t.Id == photo.TourId);
                if (tour != null)
                {
                    tour.Photos.Add(photo);
                }
            }
        }
    }
}
