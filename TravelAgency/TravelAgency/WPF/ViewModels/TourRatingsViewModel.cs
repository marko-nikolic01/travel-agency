using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRatingsViewModel
    {
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        public ButtonCommandNoParameter ViewCommand { get; set; }
        public User ActiveGuide { get; set; }
        public NavigationService NavService { get; set; }

        public TourRatingsViewModel(int activeGuideId, System.Windows.Navigation.NavigationService navService)
        {
            UserService userService = new UserService();
            ActiveGuide = userService.GetById(activeGuideId);
            TourOccurrenceService tourOccurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceService.GetFinishedOccurrencesForGuide(activeGuideId));
            ViewCommand = new ButtonCommandNoParameter(ViewDetails);
            NavService = navService;
        }
        public void ViewDetails()
        {
            TourGuestRatingsViewModel viewModel = new TourGuestRatingsViewModel(SelectedTourOccurrence.Id);
            //TourGuestRatingsView view = new TourGuestRatingsView();
            //view.DataContext = viewModel;
            //view.ShowDialog();
            Page details = new TourRatingDetailsView(viewModel);
            NavService.Navigate(details);
        }
    }
}
