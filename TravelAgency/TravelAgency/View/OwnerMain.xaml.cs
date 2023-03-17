﻿using System;
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

        public Accommodation? SelectedAccommodation { get; set; }

        public User LoggedInUser { get; set; }

        private readonly UserRepository userRepository;
        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly AccommodationPhotoRepository imageRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        private readonly AccommodationGuestRatingRepository accommodationGuestRatingRepository;

        public OwnerMain(User user)
        {
            InitializeComponent();
            DataContext = this;

            LoggedInUser = user;

            userRepository = new UserRepository();
            locationRepository = new LocationRepository();
            imageRepository = new AccommodationPhotoRepository();
            accommodationRepository = new AccommodationRepository(userRepository, locationRepository, imageRepository);
            accommodationReservationRepository = new AccommodationReservationRepository(accommodationRepository, userRepository);
            accommodationGuestRatingRepository = new AccommodationGuestRatingRepository(accommodationReservationRepository.GetAll());

            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetByUser(user));
        }

        private void ShowCreateAccommodation_Click(object sender, RoutedEventArgs e)
        {
            CreateAccommodation createAccommodation = new CreateAccommodation(LoggedInUser, accommodationRepository, locationRepository, imageRepository);
            createAccommodation.ShowDialog();
        }

        private void ShowAccommodationGuestRatingWindow_Click(object sender, RoutedEventArgs e)
        {
            AccommodationGuestRatingWindow accommodationGuestRatingWindow = new AccommodationGuestRatingWindow(LoggedInUser, accommodationReservationRepository);
            accommodationGuestRatingWindow.ShowDialog();
        }

        private void NorifyOwnerForUnratedGuests()
        {
            var unratedGuests = accommodationReservationRepository.GetUnrated(accommodationGuestRatingRepository.GetAll());
        }
    }
}
