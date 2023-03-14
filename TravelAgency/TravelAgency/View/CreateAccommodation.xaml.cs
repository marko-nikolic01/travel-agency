using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;
using System.Collections.ObjectModel;
using Image = TravelAgency.Model.Image;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodation : Window
    {
        public static ObservableCollection<string> Images { get; set; }

        public User LoggedInUser { get; set; }

        private readonly UserRepository _UserRepository;
        private readonly AccommodationRepository _AccommodationRepository;
        private readonly LocationRepository _LocationRepository;
        private readonly ImageRepository _ImageRepository;
        
        public Accommodation NewAccommodation { get; set; }
        public Location NewLocation { get; set; }

        public CreateAccommodation(User loggedInUser, AccommodationRepository accommodationRepository, LocationRepository locationRepository, ImageRepository imageRepository)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            _LocationRepository = locationRepository;
            _AccommodationRepository = accommodationRepository;
            _ImageRepository = imageRepository;
            NewAccommodation = new() { Id = _AccommodationRepository.NextId(), OwnerId = LoggedInUser.Id, Owner = LoggedInUser};
            NewLocation = new();

            Images = new ObservableCollection<string>();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG images (*.png)|*.png|JPEG images (*.jpg, *.jpeg)|*.jpg;*.jpeg";
            ofd.Multiselect = false;
            ofd.InitialDirectory = $"c:\\Users\\{Environment.UserName}\\Pictures";
            var result = ofd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                var imagePath = ofd.FileName;

                Image NewImage = new() { ObjectId = NewAccommodation.Id, Path = imagePath};

                NewAccommodation.Images.Add(NewImage);

                Images.Add(imagePath);
            }
        }

        private void RegisterAccommodation_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentRadioButton.IsChecked == true)
            {
                NewAccommodation.Type = AccommodationType.APARTMENT;
            }
            else if (HouseRadioButton.IsChecked == true)
            {
                NewAccommodation.Type = AccommodationType.HOUSE;
            }
            else
            {
                NewAccommodation.Type = AccommodationType.HUT;
            }

            Location savedLocation = _LocationRepository.SaveLocation(NewLocation);

            NewAccommodation.LocationId = savedLocation.Id;
            NewAccommodation.Location = savedLocation;

            Accommodation savedAccommodation = _AccommodationRepository.Save(NewAccommodation);

            foreach (var image in savedAccommodation.Images)
            {
                _ImageRepository.Save(image);
            }

            OwnerMain.Accommodations.Add(savedAccommodation);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
