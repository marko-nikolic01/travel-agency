using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for GuideMain.xaml
    /// </summary>
    public partial class GuideMain : Window, IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public TourRepository TourRepository { get; set; }
        public GuideMain()
        {
            InitializeComponent();
            DataContext = this;
            TourRepository = new TourRepository();
            Tours = new ObservableCollection<Tour>(TourRepository.GetTours());
            TourRepository.Subscribe(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SignOutClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void NewTourClick(object sender, RoutedEventArgs e)
        {
            CreateTour createTour = new CreateTour(TourRepository);
            createTour.Show();
        }

        public void Update()
        {
            Tours.Clear();
            foreach(Tour tour in TourRepository.GetTours())
            {
                Tours.Add(tour);
            }
        }
    }
}
