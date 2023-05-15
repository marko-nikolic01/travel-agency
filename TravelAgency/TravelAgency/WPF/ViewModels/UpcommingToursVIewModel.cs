using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;
using System.Windows.Controls;
using TravelAgency.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelAgency.Observer;

namespace TravelAgency.WPF.ViewModels
{
    public class UpcommingToursVIewModel : INotifyPropertyChanged, IObserver
    {
        public Domain.Models.User ActiveGuide { get; set; }
        private ObservableCollection<TourOccurrence> tourOccurrences;
        public ObservableCollection<TourOccurrence> TourOccurrences
        {
            get { return tourOccurrences; }
            set
            {
                tourOccurrences = value;
                OnPropertyChanged();
            }
        }
        private int canceledTour;
        public int CanceledTour
        {
            get { return canceledTour; }
            set {
                canceledTour = value;
                OnPropertyChanged();
            }
        }

        public TourOccurrence? SelectedTourOccurrence { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public NavigationService NavService { get; set; }
        public ButtonCommandNoParameter CancelCommand { get; set; }
        public ButtonCommandNoParameter CreateCommand { get; set; }
        public ButtonCommandNoParameter UndoCancelCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public UpcommingToursVIewModel(int id, NavigationService navService)
        {
            ActiveGuide = new UserService().GetById(id);
            TourOccurrenceService = new TourOccurrenceService();
            TourOccurrenceService.Subscribe(this);
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceService.GetUpcomingToursForGuide(ActiveGuide.Id));
            NavService = navService;
            CancelCommand = new ButtonCommandNoParameter(CancelTour);
            CreateCommand = new ButtonCommandNoParameter(CreateNewTour);
            UndoCancelCommand = new ButtonCommandNoParameter(UndoCancelTour);
            CanceledTour = -1;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UndoCancelTour()
        {
            if (CanceledTour != -1)
            {
                TourOccurrenceService.UndoCancelTour(CanceledTour);
            }
            CanceledTour = -1;
        }
        private void CancelTour()
        {
            if (SelectedTourOccurrence.DateTime < DateTime.Now.AddDays(2))
            {
                MessageBox.Show("This tour can not be canceled because it occurres in less than 48 hours");
                return;
            }
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                CanceledTour = TourOccurrenceService.CancelTour(SelectedTourOccurrence, ActiveGuide.Id);
            }
        }

        private void CreateNewTour()
        {
            Page create = new CreateTourForm(ActiveGuide.Id, NavService);
            NavService.Navigate(create);
        }

        public void Update()
        {
            TourOccurrences.Clear();
            foreach (TourOccurrence tourOccurrence in TourOccurrenceService.GetUpcomingToursForGuide(ActiveGuide.Id))
            {
                TourOccurrences.Add(tourOccurrence);
            }
        }
    }
}