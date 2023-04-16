using Microsoft.VisualBasic.ApplicationServices;
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
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Observer;
using TravelAgency.Repository;
using TravelAgency.Services;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for UpcomingTours.xaml
    /// </summary>
    public partial class UpcomingTours : Window, IObserver
    {
        public Model.User ActiveGuide { get; set; }
        public ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence? SelectedTourOccurrence { get; set; }
        public TourOccurrenceService TourOccurrenceService { get; set; }
        public UpcomingTours(Model.User activeGuide)
        {
            InitializeComponent();
            DataContext = this;
            ActiveGuide = activeGuide;
            TourOccurrenceService = new TourOccurrenceService();
            TourOccurrenceService.Subscribe(this);
            TourOccurrences = new ObservableCollection<TourOccurrence>(TourOccurrenceService.GetUpcomingToursForGuide(ActiveGuide.Id));
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
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
            CreateTour createTour = new CreateTour(ActiveGuide);
            createTour.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
