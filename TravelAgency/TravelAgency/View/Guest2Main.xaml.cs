using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Repository;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.Services;

namespace TravelAgency.View
{
    public partial class Guest2Main : Window, IObserver, INotifyPropertyChanged
    {
        public static ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private List<TourOccurrence> toursList;
        public TourOccurrenceService tourOccurrenceService;
        public User ActiveGuest { get; set; }
        private bool isHelpClicked;
        public bool IsHelpClicked {
            get => isHelpClicked;
            set
            {
                if (value != isHelpClicked)
                {
                    isHelpClicked = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Guest2Main(User user)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuest = user;
            tourOccurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceService.GetOfferedTours());
            tourOccurrenceService.Subscribe(this);
            toursList = TourOccurrences.ToList();
            AllertIfSelectеd(ActiveGuest);
            IsHelpClicked = false;
        }

        private void AllertIfSelectеd(User activeGuest)
        {
            TourOccurrenceAttendanceService tourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            TourOccurrenceAttendance attendance;
            if( (attendance = tourOccurrenceAttendanceService.GetAttendance(activeGuest.Id)) != null)
            {
                if (MessageBox.Show("You have just been selected as present on the tour! Do you confirm?", "Notification", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    tourOccurrenceAttendanceService.SaveAnswer(true, attendance);
                }
                else
                {
                    tourOccurrenceAttendanceService.SaveAnswer(false, attendance);
                }
            }
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            if(SelectedTourOccurrence == null)
            {
                MessageBox.Show("You must choose a tour.");
            }
            else if (SelectedTourOccurrence.Guests.Count == SelectedTourOccurrence.Tour.MaxGuestNumber)
            {
                /*AlternativeTours alternativeTours = new AlternativeTours(TourOccurrences, SelectedTourOccurrence.Id, SelectedTourOccurrence.Tour.Location, ActiveGuest, TourOccurrenceRepository);
                alternativeTours.Show();*/
            }
            else if (tourReservationRepository.IsTourReserved(ActiveGuest.Id, SelectedTourOccurrence.Id))
            {
                MessageBox.Show("You already have reservation for this tour.");
            }
            else
            {
                TourGuests tourGuests = new TourGuests(SelectedTourOccurrence, ActiveGuest);
                tourGuests.Show();
            }
        }

        private void tbCountry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void tbCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void tbDuration_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void tbLanguage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void tbGuests_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }
        private void Search()
        {
            if (tbCity.Text != "" || tbCountry.Text != "" || tbLanguage.Text != "" || tbDuration.Text != "" || tbNumOfGuests.Text != "")
            {
                if (!IsValid())
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

        private void ShowPhotos_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourOccurrence != null)
            {
                TourPhotosView tourPhotosView = new TourPhotosView(SelectedTourOccurrence);
                tourPhotosView.Show();
            }
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersView vouchersView = new VouchersView(ActiveGuest);
            vouchersView.Show();
            Close();
        }
        public void Update()
        {
            TourOccurrences.Clear();
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceService.GetOfferedTours());
            ToursDataGrid.ItemsSource = TourOccurrences;
        }

        private void MyToursButton_Click(object sender, RoutedEventArgs e)
        {
            MyTours myTours = new MyTours(ActiveGuest.Id);
            myTours.Show();
        }
        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            TourRequestView requests = new TourRequestView(ActiveGuest.Id);
            requests.Show();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if(!IsHelpClicked)
                IsHelpClicked = true;
            else
                IsHelpClicked = false;
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
          {
              MainWindow mainWindow = new MainWindow();
              mainWindow.Show();
              Close();
          }

    }
}
