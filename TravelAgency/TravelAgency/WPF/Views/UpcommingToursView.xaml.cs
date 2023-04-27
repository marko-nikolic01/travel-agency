using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Services;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for UpcommingToursView.xaml
    /// </summary>
    public partial class UpcommingToursView : Page, IObserver
    {
        public Domain.Models.User ActiveGuide { get; set; }
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence? SelectedTourOccurrence { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public NavigationService NavService { get; set; }

        public UpcommingToursView(int id, NavigationService navService)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = new UserService().GetById(id);
            TourOccurrenceService = new TourOccurrenceService();
            TourOccurrenceService.Subscribe(this);
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceService.GetUpcomingToursForGuide(ActiveGuide.Id));
            NavService = navService;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourOccurrence.DateTime < DateTime.Now.AddDays(2))
            {
                MessageBox.Show("This tour can not be canceled because it occurres in less than 48 hours");
                return;
            }
            if (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                TourOccurrenceService.CancelTour(SelectedTourOccurrence, ActiveGuide.Id);
            }
        }

        public void Update()
        {
            TourOccurrences.Clear();
            foreach (TourOccurrence tourOccurrence in TourOccurrenceService.GetUpcomingToursForGuide(ActiveGuide.Id))
            {
                TourOccurrences.Add(tourOccurrence);
            }
        }

        private void NewTour_Click(object sender, RoutedEventArgs e)
        {
            Page create = new CreateTourForm(ActiveGuide.Id, NavService);
            NavService.Navigate(create);
        }
    }
}
