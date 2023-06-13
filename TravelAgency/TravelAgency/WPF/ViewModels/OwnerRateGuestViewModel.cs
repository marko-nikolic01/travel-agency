using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerRateGuestViewModel : ViewModelBase
    {
        public MyICommand RateGuestCommand { get; set; }
        public MyICommand NavigateBackCommand { get; set; }

        private User loggedInUser;

        private UserService userService;
        private AccommodationGuestRatingService ratingService;

        private NavigationService navigationService;

        public AccommodationReservation SelectedReservation { get; set; }
        private int cleanliness;
        public int Cleanliness
        {
            get { return cleanliness; }
            set
            {
                cleanliness = value;
                OnPropertyChanged(nameof(Cleanliness));
            }
        }

        private int responsivenes;
        public int Responsivenes
        {
            get { return responsivenes; }
            set
            {
                responsivenes = value;
                OnPropertyChanged(nameof(Responsivenes));
            }
        }

        private int noisiness;
        public int Noisiness
        {
            get { return noisiness; }
            set
            {
                noisiness = value;
                OnPropertyChanged(nameof(Noisiness));
            }
        }

        private int friendliness;
        public int Friendliness
        {
            get { return friendliness; }
            set
            {
                friendliness = value;
                OnPropertyChanged(nameof(Friendliness));
            }
        }

        private int compliance;
        public int Compliance
        {
            get { return compliance; }
            set
            {
                compliance = value;
                OnPropertyChanged(nameof(Compliance));
            }
        }

        private string comment;

        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }


        public OwnerRateGuestViewModel(AccommodationReservation selectedReservation, NavigationService navigationService)
        {
            userService = new UserService();
            ratingService = new AccommodationGuestRatingService();

            loggedInUser = userService.GetLoggedInUser();

            this.navigationService = navigationService;

            SelectedReservation = selectedReservation;

            Cleanliness = 1;
            Responsivenes = 1;
            Friendliness = 1;
            Compliance = 1;
            Noisiness = 1;

            NavigateBackCommand = new MyICommand(Execute_NavigateBack);
            RateGuestCommand = new MyICommand(Execute_RateGuestCommand);
        }

        public void Execute_RateGuestCommand()
        {
            AccommodationGuestRating newRating = new AccommodationGuestRating();
            newRating.AccommodationReservation.Id = SelectedReservation.Id;
            newRating.AccommodationReservation = SelectedReservation;
            newRating.Compliance = Compliance;
            newRating.Comment = Comment;
            newRating.Noisiness = Noisiness;
            newRating.Friendliness = Friendliness;
            newRating.Cleanliness = Cleanliness;

            ratingService.CreateNew(newRating);
            MessageBox.Show("Guest rated successfully.", "Successful rating", MessageBoxButton.OK, MessageBoxImage.Information);
            Execute_NavigateBack();
        }

        private void Execute_NavigateBack()
        {
            this.navigationService.Navigate(new Uri("WPF/Views/OwnerRatingsView.xaml", UriKind.Relative));
        }
    }
}
