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
    /// Interaction logic for OwnerMain.xaml
    /// </summary>
    public partial class OwnerMain : Window
    {
        public static ObservableCollection<Accommodation> Accommodations { get; set; }

        public Accommodation SelectedAccomodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly ImageRepository imageRepository;

        public OwnerMain(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            accommodationRepository = new AccommodationRepository(locationRepository, imageRepository);
            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetByUser(user));
        }

        private void ShowCreateAccommodation_Click(object sender, RoutedEventArgs e)
        {
            CreateAccommodation createAccommodation = new CreateAccommodation(LoggedInUser, accommodationRepository, locationRepository, imageRepository);
            createAccommodation.ShowDialog();
        }
    }
}
