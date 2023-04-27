using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Repositories;
using TravelAgency.Services;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    public partial class OfferedToursView : Page, IObserver, INotifyPropertyChanged
    {
        public static ObservableCollection<TourOccurrence> TourOccurrences { get; set; }
        public TourOccurrence SelectedTourOccurrence { get; set; }
        private List<TourOccurrence> toursList;
        public TourOccurrenceService tourOccurrenceService;
        public User ActiveGuest { get; set; }
        private bool isHelpClicked;
        public bool IsHelpClicked {
            get => isHelpClicked;
            set
            {
                if (value != isHelpClicked)
                {
                    isHelpClicked = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private OfferedToursViewModel toursViewModel;
        public OfferedToursView(/*User user*/)
        {
            toursViewModel = new OfferedToursViewModel(0);
            InitializeComponent();
            DataContext = toursViewModel;   
        }

        private void AllertIfSelectеd(User activeGuest)
        {
            TourOccurrenceAttendanceService tourOccurrenceAttendanceService = new TourOccurrenceAttendanceService();
            TourOccurrenceAttendance attendance;
            if( (attendance = tourOccurrenceAttendanceService.GetAttendance(activeGuest.Id)) != null)
            {
                if (MessageBox.Show("You have just been selected as present on the tour! Do you confirm?", "Notification", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    tourOccurrenceAttendanceService.SaveAnswer(true, attendance);
                }
                else
                {
                    tourOccurrenceAttendanceService.SaveAnswer(false, attendance);
                }
            }
        }

        
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            toursViewModel.Search();
        }

        private void ShowPhotos_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTourOccurrence != null)
            {
                TourPhotosView tourPhotosView = new TourPhotosView(SelectedTourOccurrence);
                tourPhotosView.Show();
            }
        }
        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            VouchersView vouchersView = new VouchersView(ActiveGuest);
            vouchersView.Show();
            //Close();
        }
        public void Update()
        {
            TourOccurrences.Clear();
            TourOccurrences = new ObservableCollection<TourOccurrence>(tourOccurrenceService.GetOfferedTours());
            ToursDataGrid.ItemsSource = TourOccurrences;
        }

        private void MyToursButton_Click(object sender, RoutedEventArgs e)
        {
            MyTours myTours = new MyTours(ActiveGuest.Id);
           // myTours.Show();
        }
        private void RequestButton_Click(object sender, RoutedEventArgs e)
        {
            TourRequestView requests = new TourRequestView(ActiveGuest.Id);
            requests.Show();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if(!IsHelpClicked)
                IsHelpClicked = true;
            else
                IsHelpClicked = false;
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
          {
              MainWindow mainWindow = new MainWindow();
              mainWindow.Show();
            //  Close();
          }

    }
}
