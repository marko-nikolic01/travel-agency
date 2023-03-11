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
using System.Windows.Threading;
using TravelAgency.Model;
using TravelAgency.Repository;
using Xceed.Wpf.Toolkit;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for Guest1Main.xaml
    /// </summary>
    public partial class Guest1Main : Window
    {
        public AccommodationRepository accommodationRepository;
        public LocationRepository locationRepository;
        public ImageRepository imageRepository;

        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedCity { get; set; }

        public Guest1Main()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.9);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.9);

            
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            accommodationRepository = new AccommodationRepository(locationRepository, imageRepository);

            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAll());

            Countries = locationRepository.GetAllCountries();
            Countries.Insert(0, "Not Specified");
            SelectedCountry = Countries[0];
        }

        private void LoadDateTime(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.statusBarDateTime.Content = DateTime.Now.ToString("dd/mm/yyyy     hh:mm:ss");
            }, this.Dispatcher);
            timer.Start();
        }

        private void NumberUpDownValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var upDown = (sender as IntegerUpDown);
            if (upDown.Value == 0)
            {
                upDown.Text = "";
            }

        }

        private void CountrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedCountry != "")
            {
                Cities = locationRepository.GetCitiesByCountry(SelectedCountry);
                Cities.Insert(0, "Not Specified");
                cityComboBox.ItemsSource = Cities;
                cityComboBox.SelectedItem = 0;
                SelectedCity = Cities[0];
                cityComboBox.Text = Cities[0];
            }

        }

        private void ComboBoxLostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.Text != "")
            {
                comboBox.Text = comboBox.SelectedItem.ToString();
            }
            else
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void SignOut(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Search(object sender, RoutedEventArgs e)
        {

        }

        private void CancelSearch(object sender, RoutedEventArgs e)
        {

        }
    }
}
