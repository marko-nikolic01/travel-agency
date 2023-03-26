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
    /// Interaction logic for Guest2Main.xaml
    /// </summary>
    public partial class Guest2Main : Window, IObserver
    {
        public static ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private List<TourOccurrence> toursList;
        public TourRepository TourRepository { get; set; }
        public static TourOccurrenceRepository TourOccurrenceRepository { get; set; }
        public static TourReservationRepository TourReservationRepository { get; set; }
        public LocationRepository LocationRepository { get; set; }
        public PhotoRepository PhotoRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public TourOccurrenceAttendanceRepository TourOccurrenceAttendanceRepository { get; set; }
        public User ActiveGuest { get; set; }

        public Guest2Main(User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuest = user;
            TourRepository = new TourRepository();
            LocationRepository = new LocationRepository();
            PhotoRepository = new PhotoRepository();
            TourReservationRepository = new TourReservationRepository();
            UserRepository = new UserRepository();
            TourOccurrenceAttendanceRepository = new TourOccurrenceAttendanceRepository();
            TourOccurrenceRepository = new TourOccurrenceRepository(PhotoRepository, LocationRepository, TourRepository, TourReservationRepository, UserRepository);
            TourOccurrences = new ObservableCollection<TourOccurrence>();
            FilterTourOccurrences();
            TourOccurrenceRepository.Subscribe(this);
            toursList = TourOccurrences.ToList();
            AllertIfSelectеd(ActiveGuest);
        }

        private void FilterTourOccurrences()
        {
            foreach (TourOccurrence tourOccurrence in TourOccurrenceRepository.GetAll())
            {
                if (tourOccurrence.DateTime.Date >= DateTime.Now.Date && tourOccurrence.CurrentState != CurrentState.Ended)
                {
                    TourOccurrences.Add(tourOccurrence);
                }
            }
        }

        private void AllertIfSelectеd(User activeGuest)
        {
            foreach(TourOccurrenceAttendance tourOccurrenceAttendance in TourOccurrenceAttendanceRepository.GetAll())
            {
                if(tourOccurrenceAttendance.GuestId == activeGuest.Id && tourOccurrenceAttendance.ResponseStatus == ResponseStatus.NotAnsweredYet 
                    && tourOccurrenceAttendance.KeyPointId != -1)
                {
                    if(MessageBox.Show("You have just been selected as present on the tour! Do you confirm?", "Notification", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                    {
                        tourOccurrenceAttendance.ResponseStatus = ResponseStatus.Accepted;
                    }
                    else
                    {
                        tourOccurrenceAttendance.ResponseStatus = ResponseStatus.Declined;
                    }
                    TourOccurrenceAttendanceRepository.UpdateTourOccurrenceAttendaces(tourOccurrenceAttendance);
                }
            }
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTourOccurrence == null)
            {
                MessageBox.Show("You must choose a tour.");
            }
            else if (SelectedTourOccurrence.Guests.Count == SelectedTourOccurrence.Tour.MaxGuestNumber)
            {
                AlternativeTours alternativeTours = new AlternativeTours(TourOccurrences, SelectedTourOccurrence.Id, SelectedTourOccurrence.Tour.Location, ActiveGuest);
                alternativeTours.Show();
            }
            else
            {
                TourReservationWindow tourReservation = new TourReservationWindow(SelectedTourOccurrence, TourOccurrences, ActiveGuest);
                tourReservation.Show();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if(tbCity.Text != "" || tbCountry.Text != "" || tbLanguage.Text != "" || tbDuration.Text != "" || tbNumOfGuests.Text != "")
            {
             if(!IsValid())
             {
                MessageBox.Show("Number of guests must be a non negative number");
                tbNumOfGuests.Text = "";
                return;
             }
                ToursDataGrid.ItemsSource = FilterList(IsTextBoxEmpty(tbCity), IsTextBoxEmpty(tbCountry), IsTextBoxEmpty(tbDuration), IsTextBoxEmpty(tbLanguage), IsTextBoxEmpty(tbNumOfGuests));
            }
            else
            {
                ToursDataGrid.ItemsSource = TourOccurrences;
            }
        }
        private bool IsTextBoxEmpty(TextBox textBox)
        {
            return textBox.Text == "";
        }

        //proverava da li tekst iz textbox zadovaljava kriterijum ili ako je prazan textbox onda svakako zadovoljava kriterijum
        private IEnumerable<TourOccurrence> FilterList(bool tbCityEmpty, bool tbCountryEmpty, bool tbDurEmpty, bool tbLanguageEmpty, bool tbNumOfGuestsEmpty)
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

        private bool IsValid()
        {
            if(tbNumOfGuests.Text == "")
            {
                return true;
            }
            int res;
            if(int.TryParse(tbNumOfGuests.Text, out res))
            {
                if(res>=0)
                    return true;
                else 
                    return false;
            }
            else
            {
                return false;
            }
        }

        public void Update()
        {
            TourOccurrences.Clear();
            FilterTourOccurrences();
            ToursDataGrid.ItemsSource = TourOccurrences;
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void MyToursButton_Click(object sender, RoutedEventArgs e)
        {
            MyTours myTours = new MyTours(TourOccurrenceRepository, ActiveGuest.Id);
            myTours.Show();
        }
    }
}
