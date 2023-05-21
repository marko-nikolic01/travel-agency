using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateTourForm.xaml
    /// </summary>
    public partial class CreateTourForm : Page, INotifyPropertyChanged
    {
        public Tour NewTour { get; set; }
        public User ActiveGuide { get; set; }
        public NavigationService NavService { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public List<string> Countries { get; set; }
        private ObservableCollection<string> cities;
        public ObservableCollection<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged();
            }
        }
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged();
            }
        }
        private string selectedCountry;
        public string SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                if (value.Equals(Countries[0]))
                {
                    Cities.Clear();
                    Cities.Add("< select a city >");
                    IsCountrySelected = false;
                }
                else
                {
                    Cities.Clear();
                    Cities.Add("< select a city >");
                    foreach (var city in LocationService.GetCitiesByCountry(value))
                    {
                        Cities.Add(city);
                    }
                    IsCountrySelected = true;
                }
                SelectedCity = Cities[0];
                selectedCountry = value;
                OnPropertyChanged();
            }
        }
        private bool isCountrySelected;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsCountrySelected
        {
            get { return isCountrySelected; }
            set
            {
                isCountrySelected = value;
                OnPropertyChanged();
            }
        }
        private int selectedPhoto;
        public int SelectedPhoto
        {
            get { return selectedPhoto; }
            set
            {
                selectedPhoto = value;
                OnPropertyChanged();
            }
        }
        private bool photosEnabled;
        public bool PhotosEnabled
        {
            get { return photosEnabled; }
            set
            {
                photosEnabled = value;
                OnPropertyChanged();
            }
        }
        public LocationService LocationService { get; set; }
        private bool CreatingTourForLanguage;
        private bool CreatingTourForLocation;
        public CreateTourForm(int id, NavigationService navService, string language = null, Location location = null)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = new UserService().GetById(id);
            CreatingTourForLocation = false;
            CreatingTourForLanguage = false;
            Cities = new ObservableCollection<string>() { "< select a city >" };
            SelectedCity = Cities[0];
            Countries = new List<string>() { "< select a country >" };
            SelectedCountry = Countries[0];
            LocationService = new LocationService();
            Countries.AddRange(LocationService.GetCountries());
            NewTour = new Tour();
            if (language != null)
            {
                NewTour.Language = language;
                CreatingTourForLanguage = true;
            }
            if (location != null)
            {
                SelectedCountry = location.Country;
                selectedCity = location.City;
                CreatingTourForLocation = true;
            }
            TourOccurrenceService = new TourOccurrenceService();

            DateCalendar.DisplayDateStart = DateTime.Today;
            //cultureinfo from stackoverflow
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            NavService = navService;
            SelectedPhoto = -1;
            PhotosEnabled = false;
        }
        private void AddKeyPoint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyPointsText.Text))
            {
                return;
            }
            ListKeyPoints.Items.Add(KeyPointsText.Text);
            KeyPointsText.Clear();
            KeyPointsText.Focus();
        }

        private void AddDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DateCalendar.Text) || string.IsNullOrEmpty(Time.Text))
            {
                return;
            }
            ListDateTimes.Items.Add(DateCalendar.Text + " " + Time.Text);
            DateCalendar.Focus();
        }

        private void AddImages_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageText.Text))
            {
                return;
            }
            ListPhotos.Items.Add(ImageText.Text);
            ImageText.Clear();
            ImageText.Focus();
            PhotosEnabled = true;
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPhoto != -1 && ListPhotos.Items.Count > 0)
            {
                ListPhotos.Items.RemoveAt(SelectedPhoto);
            }
            ImageText.Focus();
            SelectedPhoto = -1;
            if(ListPhotos.Items.Count == 0)
            {
                PhotosEnabled = false;
            }
        }
        private void PreView_Click(object sender, RoutedEventArgs e)
        {
            List<string> links = new List<string>();
            foreach(var link in ListPhotos.Items)
            {
                links.Add(link.ToString());
            }
            Window window = new TourPhotosPreView(links);
            window.ShowDialog();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!AreListsComplete() || !AreInputsValid())
            {
                return;
            }
            ProcessInputs(NewTour);
            SaveTours();
            Page tours = new TodaysToursView(ActiveGuide.Id);
            NavService.Navigate(tours);
        }

        private void SaveTours()
        {
            CreatedTourFromStatisticService service = new CreatedTourFromStatisticService();
            Tour newTour = TourOccurrenceService.SaveNewTours(NewTour, ListPhotos.Items, ListDateTimes.Items, ListKeyPoints.Items, ActiveGuide);
            if(CreatingTourForLanguage)
            {
                service.MakeNotifications(newTour, "language");
            }
            else if(CreatingTourForLocation)
            {
                service.MakeNotifications(newTour, "location");
            }
        }

        private void ProcessInputs(Tour newTour)
        {
            ProcessIntInputs(newTour);
            ProcessLocationInput(newTour);
        }

        private void ProcessIntInputs(Tour newTour)
        {
            int result;
            if (int.TryParse(MaxGuests.Text, out result))
            {
                newTour.MaxGuestNumber = result;
            }
            if (int.TryParse(Duration.Text, out result))
            {
                newTour.Duration = result;
            }
        }

        private void ProcessLocationInput(Tour newTour)
        {
            newTour.Location = new LocationRepository().GetLocationForCountryAndCity(SelectedCountry, SelectedCity);
            newTour.LocationId = newTour.Location.Id;
        }

        private bool AreListsComplete()
        {
            if (ListKeyPoints.Items.Count < 2)
            {
                MessageBox.Show("You have to enter at least two key points!");
                return false;
            }
            else if (ListPhotos.Items.Count == 0)
            {
                MessageBox.Show("You have to enter at least one photo link!");
                return false;
            }
            else if (ListDateTimes.Items.Count == 0)
            {
                MessageBox.Show("You have to enter at least one date and time!");
                return false;
            }
            return true;
        }

        private bool AreInputsValid()
        {
            if (selectedCountry.Equals(Countries[0]))
            {
                MessageBox.Show("Select a country");
                return false;
            }
            else if (SelectedCity.Equals(Cities[0])){
                MessageBox.Show("Select a city");
                return false;
            }
            else if (NewTour.IsValid == false)
            {
                MessageBox.Show("Tour entry is wrong");
                return false;
            }
            return true;
        }
        private void DateCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DateCalendar.SelectedDate == DateTime.Now.Date)
            {
                Time.StartTime = DateTime.Now.TimeOfDay;
            }
            else
            {
                TimeSpan timeSpan = (TimeSpan)DateTime.ParseExact("00:00", "HH:mm", null).TimeOfDay;
                Time.StartTime = timeSpan;
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
