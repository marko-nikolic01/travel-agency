﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAddAccommodationViewModel : ViewModelBase
    {
        public NavigationService NavigationService { get; set; }

        private UserService userService;
        private LocationService locationService;
        private AccommodationService accommodationService;

        private User loggedInUser;

        public string NavigateBackLocation { get; set; }
        public MyICommand NavigateBack { get; set; }
        public MyICommand AddPhotoCommand { get; set; }
        public MyICommand RemovePhotoCommand { get; set; }
        public MyICommand AddAccommodationCommand { get; set; }

        public Accommodation NewAccommodation { get; set; }
        private string selectedCountry;
        public string SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                UpdateLocations();
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }
        private string selectedCity;

        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        private List<string> countries;
        public List<string> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged(nameof(Countries));
            }
        }
        private List<string> cities;
        public List<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        private ObservableCollection<AccommodationPhoto> photos;

        public ObservableCollection<AccommodationPhoto> Photos
        {
            get { return photos; }
            set
            {
                photos = value;
                OnPropertyChanged(nameof(Photos));
            }
        }

        private string photoLink;

        public string PhotoLink
        {
            get { return photoLink; }
            set
            {
                photoLink = value;
                OnPropertyChanged(nameof(PhotoLink));
            }
        }

        private AccommodationPhoto selectedPhoto;

        public AccommodationPhoto SelectedPhoto
        {
            get { return selectedPhoto; }
            set
            {
                selectedPhoto = value;
                OnPropertyChanged(nameof(SelectedPhoto));
            }
        }



        public OwnerAddAccommodationViewModel(NavigationService navigationService)
        {
            NavigationService = navigationService;

            userService = new UserService();
            locationService = new LocationService();
            accommodationService = new AccommodationService();

            NavigateBack = new MyICommand(Execute_NavigateBack);
            AddPhotoCommand = new MyICommand(Execute_AddPhotoCommand);
            RemovePhotoCommand = new MyICommand(Execute_RemovePhotoCommand);
            AddAccommodationCommand = new MyICommand(Execute_AddAccommodationCommand);

            loggedInUser = userService.GetLoggedInUser();

            NewAccommodation = new Accommodation() { Owner = loggedInUser, IsOpen = true };
            NewAccommodation.Owner.Id = loggedInUser.Id;

            Photos = new ObservableCollection<AccommodationPhoto>();

            IsEnabled = false;

            PhotoLink = string.Empty;

            Countries = locationService.GetCountries();
            Countries.Insert(0, "<Select country>");
            selectedCountry = Countries[0];

            Cities = new List<string>
            {
                "<Select city>"
            };
            SelectedCity = Cities[0];

            NavigateBackLocation = "WPF/Views/OwnerManageAccommodationsView.xaml";
        }

        public OwnerAddAccommodationViewModel(NavigationService navigationService, string preselectedCountry, string preselectedCity, string navigateBackLocation) : this(navigationService)
        {
            SelectedCountry = preselectedCountry;
            SelectedCity = preselectedCity;
            NavigateBackLocation = navigateBackLocation;
        }

        private void Execute_AddAccommodationCommand()
        {
            NewAccommodation.Photos = Photos.ToList();

            if (!locationService.CountryExists(SelectedCountry))
            {
                MessageBox.Show("Select a valid country.", "Selected country not valid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!locationService.CityExists(SelectedCity))
            {
                MessageBox.Show("Select a valid city.", "Selected city not valid", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewAccommodation.Location = locationService.GetLocationForCountryAndCity(SelectedCountry, SelectedCity);
            NewAccommodation.Location.Id = NewAccommodation.Location.Id;

            if (Photos.Count < 1)
            {
                MessageBox.Show("Add at least 1 photo.", "No photos added", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NewAccommodation.IsValid)
            {
                accommodationService.CreateNew(NewAccommodation);
                Execute_NavigateBack();
                return;
            }

            MessageBox.Show("Fields not valid!", "Invalid fields", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Execute_NavigateBack()
        {
            NavigationService.Navigate(new Uri(NavigateBackLocation, UriKind.Relative));
        }

        private void Execute_RemovePhotoCommand()
        {
            if (SelectedPhoto != null)
            {
                Photos.Remove(SelectedPhoto);
                OnPropertyChanged(nameof(Photos));
            }
            else
            {
                MessageBox.Show("Select a photo.", "No photo selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Execute_AddPhotoCommand()
        {
            if (PhotoLink != string.Empty)
            {
                Photos.Add(new AccommodationPhoto() { Path = PhotoLink });
                OnPropertyChanged(nameof(Photos));
                PhotoLink = string.Empty;
            }
            else
            {
                MessageBox.Show("Enter a photo link.", "No photo link given", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateLocations()
        {
            if (SelectedCountry != "<Select country>")
            {
                var tempList = new List<string>(locationService.GetCitiesByCountry(SelectedCountry));
                tempList.Insert(0, "<Select city>");
                Cities = tempList;
                SelectedCity = Cities[0];
                IsEnabled = true;
            }
            else
            {
                IsEnabled = false;
                Cities = new List<string>
                {
                    "<Select city>"
                };
                SelectedCity = Cities[0];
            }
        }
    }
}
