using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerProfilePage.xaml
    /// </summary>
    public partial class OwnerProfilePage : Page
    {
        public User LoggedInUser { get; set; }
        private UserService userService;
        private AccommodationService accommodationService;
        private AccommodationOwnerRatingService accommodationOwnerRatingService;
        private SuperOwnerService superOwnerService;

        public int AccommodationsCount { get; set; }
        public int RatingsCount { get; set; }
        public double AverageRating { get; set; }

        public OwnerProfilePage()
        {
            InitializeComponent();
            DataContext = this;

            userService = new UserService();
            accommodationService = new AccommodationService();
            accommodationOwnerRatingService = new AccommodationOwnerRatingService();
            superOwnerService = new SuperOwnerService();

            LoggedInUser = userService.GetLoggedInUser();
            AccommodationsCount = accommodationService.GetAccommodationsCountForOwner(LoggedInUser);
            RatingsCount = accommodationOwnerRatingService.GetRatingsCountForOwner(LoggedInUser);
            AverageRating = superOwnerService.GetAverageRatingForOwner(LoggedInUser);
        }
    }
}
