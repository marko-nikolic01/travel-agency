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
using AccommodationPhoto = TravelAgency.Model.AccommodationPhoto;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodation : Window
    {
        public static ObservableCollection<string> Images { get; set; }

        public User LoggedInUser { get; set; }

        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly AccommodationPhotoRepository accommodationPhotoRepository;
        
        public Accommodation NewAccommodation { get; set; }
        public Location NewLocation { get; set; }

        public CreateAccommodation(User loggedInUser, AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationPhotoRepository accommodationPhotoRepository)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = loggedInUser;
            this.locationRepository = locationRepository;
            this.accommodationRepository = accommodationRepository;
            this.accommodationPhotoRepository = accommodationPhotoRepository;
            NewAccommodation = new() { Id = this.accommodationRepository.NextId(), OwnerId = LoggedInUser.Id, Owner = LoggedInUser };
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

                AccommodationPhoto NewImage = new() { ObjectId = NewAccommodation.Id, Path = imagePath};

                NewAccommodation.Photos.Add(NewImage);

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

            Location savedLocation = locationRepository.SaveLocation(NewLocation);

            NewAccommodation.LocationId = savedLocation.Id;
            NewAccommodation.Location = savedLocation;

            Accommodation savedAccommodation = accommodationRepository.Save(NewAccommodation);

            accommodationPhotoRepository.SaveAll(savedAccommodation.Photos);

            OwnerMain.Accommodations.Add(savedAccommodation);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
