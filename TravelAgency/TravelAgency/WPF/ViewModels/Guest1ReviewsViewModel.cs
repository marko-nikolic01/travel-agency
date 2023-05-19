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
    public class Guest1ReviewsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private AccommodationGuestRatingService _ratingService;

        public MyICommand<string> NavigationCommand { get; private set; }

        public User Guest { get; set; }
        private ObservableCollection<AccommodationGuestRating> _ratings;
        private AccommodationGuestRating _selectedRating;

        public ObservableCollection<AccommodationGuestRating> Ratings
        {
            get => _ratings;
            set
            {
                if (value != _ratings)
                {
                    _ratings = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationGuestRating SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (value != _selectedRating)
                {
                    _selectedRating = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guest1ReviewsViewModel(MyICommand<string> navigationCommand, User guest)
        {
            NavigationCommand = navigationCommand;

            _ratingService = new AccommodationGuestRatingService();

            Guest = guest;
            InitializeData();
        }

        private void InitializeData()
        {
            InitializeMoveRequests();
        }

        private void InitializeMoveRequests()
        {
            List<AccommodationGuestRating> ratings = _ratingService.GetRatingsVisibleToGuest(Guest);
            SortByStartDate(ratings);
            Ratings = new ObservableCollection<AccommodationGuestRating>(ratings);
        }

        private void SortByStartDate(List<AccommodationGuestRating> ratings)
        {
            for (int i = 0; i < ratings.Count() - 1; i++)
            {
                for (int j = 0; j < ratings.Count() - i - 1; j++)
                {
                    if (ratings[j].AccommodationReservation.DateSpan.EndDate.CompareTo(ratings[j + 1].AccommodationReservation.DateSpan.EndDate) < 0)
                    {
                        AccommodationGuestRating swaper = ratings[j];
                        ratings[j] = ratings[j + 1];
                        ratings[j + 1] = swaper;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
