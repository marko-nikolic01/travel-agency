using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class OwnerScheduleRenovationViewModel : ViewModelBase
    {
        public MyICommand ScheduleRenovationCmd { get; set; }

        private NavigationService navigationService;

        private User loggedInUser;

        public MyICommand NavigateBackCommand { get; set; }

        private UserService userService;
        private AccommodationService accommodationService;
        private RenovationService renovationService;
        private AccommodationDateFinderService accommodationDateFinderService;

        private Accommodation selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get { return selectedAccommodation; }
            set
            {
                selectedAccommodation = value;
                OnPropertyChanged(nameof(SelectedAccommodation));
                UpdateAvailableDateSpans();
            }
        }

        private DateTime selectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return selectedStartDate; }
            set
            {
                selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
                UpdateMinimumEndDate();
                UpdateAvailableDateSpans();
            }
        }
        private DateTime selectedEndDate;
        public DateTime SelectedEndDate
        {
            get { return selectedEndDate; }
            set
            {
                selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
                UpdateMinimumEndDate();
                UpdateAvailableDateSpans();
            }
        }

        public DateTime Tommorrow { get; set; }

        private DateTime minimumEndDate { get; set; }
        public DateTime MinimumEndDate
        {
            get { return minimumEndDate; }
            set
            {
                minimumEndDate = value;
                OnPropertyChanged(nameof(MinimumEndDate));
            }
        }

        private int numberOfDays;
        public int NumberOfDays
        {
            get { return numberOfDays; }
            set
            {
                numberOfDays = value;
                OnPropertyChanged(nameof(NumberOfDays));
                UpdateMinimumEndDate();
                UpdateAvailableDateSpans();
            }
        }
        private DateSpan selectedDateSpan;
        public DateSpan SelectedDateSpan
        {
            get { return selectedDateSpan; }
            set
            {
                selectedDateSpan = value;
                OnPropertyChanged(nameof(SelectedDateSpan));
            }
        }

        public ObservableCollection<DateSpan> AvailableDateSpans { get; set; }

        public List<Accommodation> Accommodations { get; set; }

        public AccommodationRenovation NewAccommodationRenovation { get; set; }

        public OwnerScheduleRenovationViewModel(NavigationService navigationService)
        {
            ScheduleRenovationCmd = new MyICommand(Execute_ScheduleRenovationCommand);

            this.navigationService = navigationService;

            NavigateBackCommand = new MyICommand(Execute_NavigateBackCommand);

            userService = new UserService();
            accommodationService = new AccommodationService();
            renovationService = new RenovationService();
            accommodationDateFinderService = new AccommodationDateFinderService();

            loggedInUser = userService.GetLoggedInUser();

            Accommodations = accommodationService.GetByOwner(loggedInUser);
            AvailableDateSpans = new ObservableCollection<DateSpan>();

            numberOfDays = 1;

            if (Accommodations.Count == 0)
            {
                MessageBox.Show("You have no accommodations.");
                Execute_NavigateBackCommand();
            }
            else
            {
                Tommorrow = DateTime.Now.AddDays(1);
                selectedStartDate = DateTime.Now.AddDays(1);
                selectedEndDate = DateTime.Now.AddDays(1);
                minimumEndDate = DateTime.Now.AddDays(1);

                SelectedAccommodation = Accommodations[0];

                NewAccommodationRenovation = new AccommodationRenovation() { Accommodation = SelectedAccommodation, AccommodationId = SelectedAccommodation.Id, Description = "" };
            }
        }

        private void Execute_NavigateBackCommand()
        {
            navigationService.Navigate(new Uri("WPF/Views/OwnerRenovationsView.xaml", UriKind.Relative));
        }

        private void UpdateAvailableDateSpans()
        {
            if (SelectedStartDate != null && SelectedEndDate != null)
            {
                var availableDates = accommodationDateFinderService.FindAvailableDatesInsideDateRange(SelectedAccommodation, SelectedStartDate, SelectedEndDate, numberOfDays);
                AvailableDateSpans.Clear();
                foreach (var date in availableDates)
                {
                    AvailableDateSpans.Add(date);
                }
            }
        }

        private void UpdateMinimumEndDate()
        {
            MinimumEndDate = SelectedStartDate.AddDays(NumberOfDays - 1);
            if (MinimumEndDate.CompareTo(SelectedEndDate) > 0)
            {
                SelectedEndDate = MinimumEndDate;
            }
        }

        private void ScheduleRenovation()
        {
            if (NewAccommodationRenovation.DateSpan == null)
            {
                MessageBox.Show("Select a datespan.");
                return;
            }
            if (NewAccommodationRenovation.Description == string.Empty)
            {
                MessageBox.Show("Enter a description.");
                return;
            }

            NewAccommodationRenovation.AccommodationId = SelectedAccommodation.Id;
            NewAccommodationRenovation.Accommodation = SelectedAccommodation;
            NewAccommodationRenovation.DateSpan = SelectedDateSpan;

            if (renovationService.CanRenovationBeScheduled(NewAccommodationRenovation))
            {
                renovationService.ScheduleRenovation(NewAccommodationRenovation);
                MessageBox.Show("Successfully scheduled renovation!");
                Execute_NavigateBackCommand();
            }
            else
            {
                MessageBox.Show("Invalid request!");
            }
        }

        private void Execute_ScheduleRenovationCommand()
        {
            ScheduleRenovation();
        }
    }
}
