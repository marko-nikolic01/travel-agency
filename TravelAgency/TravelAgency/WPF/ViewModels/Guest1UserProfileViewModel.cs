using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest1UserProfileViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationGuestRatingService _ratingService;

        public MyICommand<string> NavigationCommand { get; private set; }
        public User Guest { get; set; }
        public double _averageCleanliness;
        public double _averageCompliance;
        public double _averageFriendliness;
        public double _averageNoisiness;
        public double _averageResponsiveness;
        public double _average;
        private bool _hasRatings;

        public double AverageCleanliness
        {
            get => _averageCleanliness;
            set
            {
                if (value != _averageCleanliness)
                {
                    _averageCleanliness = value;
                    OnPropertyChanged();
                }
            }
        }

        public double AverageCompliance
        {
            get => _averageCompliance;
            set
            {
                if (value != _averageCompliance)
                {
                    _averageCompliance = value;
                    OnPropertyChanged();
                }
            }
        }

        public double AverageFriendliness
        {
            get => _averageFriendliness;
            set
            {
                if (value != _averageFriendliness)
                {
                    _averageFriendliness = value;
                    OnPropertyChanged();
                }
            }
        }

        public double AverageNoisiness
        {
            get => _averageNoisiness;
            set
            {
                if (value != _averageNoisiness)
                {
                    _averageNoisiness = value;
                    OnPropertyChanged();
                }
            }
        }

        public double AverageResponsiveness
        {
            get => _averageResponsiveness;
            set
            {
                if (value != _averageResponsiveness)
                {
                    _averageResponsiveness = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Average
        {
            get => _average;
            set
            {
                if (value != _average)
                {
                    _average = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasRatings
        {
            get => _hasRatings;
            set
            {
                if (value != _hasRatings)
                {
                    _hasRatings = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1UserProfileViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;

            _ratingService = new AccommodationGuestRatingService();

            Guest = guest;
            InitializeData();
        }

        private void InitializeData()
        {
            InitializeRatings();
        }

        private void InitializeRatings()
        {
            List<AccommodationGuestRating> allRatings = _ratingService.GetAllRatings();
            List<AccommodationGuestRating> ratings = new List<AccommodationGuestRating>();
            foreach (AccommodationGuestRating rating in allRatings)
            {
                if (rating.AccommodationReservation.Guest.Id == Guest.Id)
                {
                    ratings.Add(rating);
                }
            }

            if (ratings.Count() > 0)
            {
                HasRatings = true;
                CalculateAverages(ratings);
            }
        }

        private void CalculateAverages(List<AccommodationGuestRating> ratings)
        {
            double[] sums = { 0, 0, 0, 0, 0, 0};
            foreach (AccommodationGuestRating rating in ratings)
            {
                sums[0] += rating.Cleanliness;
                sums[1] += rating.Compliance;
                sums[2] += rating.Friendliness;
                sums[3] += rating.Noisiness;
                sums[4] += rating.Responsivenes;
            }
            sums[5] = ((double)(sums[0] + sums[1] + sums[2] + sums[3] + sums[4])) / 5;
            AverageCleanliness = Math.Round(((double)sums[0]) / ratings.Count(), 2);
            AverageCompliance = Math.Round(((double)sums[1]) / ratings.Count(), 2);
            AverageFriendliness = Math.Round(((double)sums[2]) / ratings.Count(), 2);
            AverageNoisiness = Math.Round(((double)sums[3]) / ratings.Count(), 2);
            AverageResponsiveness = Math.Round(((double)sums[4]) / ratings.Count(), 2);
            Average = Math.Round(((double)sums[5]) / ratings.Count(), 2);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
