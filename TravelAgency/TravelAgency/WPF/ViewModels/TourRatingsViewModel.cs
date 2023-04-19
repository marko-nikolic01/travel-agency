using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        public ButtonCommand<Window> HomeCommand { get; set; }
        public User ActiveGuide { get; set; }

        public TourRatingsViewModel(int activeGuideId)
        {
            UserService userService = new UserService();
            ActiveGuide = userService.GetById(activeGuideId);
            TourOccurrenceService tourOccurrenceService = new TourOccurrenceService();
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceService.GetFinishedOccurrencesForGuide(activeGuideId));
            ViewCommand = new ButtonCommandNoParameter(ViewDetails);
            HomeCommand = new ButtonCommand<Window>(ShowHome);
        }
        private void ShowHome(Window window)
        {
            GuideMain guideMain = new GuideMain(ActiveGuide);
            guideMain.Show();
            window.Close();
        }
        public void ViewDetails()
        {
            TourGuestRatingsViewModel viewModel = new TourGuestRatingsViewModel(SelectedTourOccurrence.Id);
            TourGuestRatingsView view = new TourGuestRatingsView();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
    }
}
