using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class GuideProfileViewModel : ViewModelBase
    {
        public UserService UserService { get; set; }
        public TourOccurrenceService TourOccurrenceService{ get; set; }
        public TourRatingService TourRatingService { get; set; }
        public VoucherService VoucherService { get; set; }
        public User Guide { get; set; }
        public int FinishedTours { get; set; }
        public double AverageGrade { get; set; }
        public ButtonCommandNoParameter ResignCommand { get; set; }
        private string password;
        public string Password
        {
            get { return password; }
            set 
            { 
                password = value;
                if (password.Equals(Guide.Password))
                {
                    IsPasswordCorrect = true;
                }
                else
                {
                    IsPasswordCorrect = false;
                }
                OnPropertyChanged(nameof(Password));
            }
        }
        private bool isPasswordCorrect;
        public bool IsPasswordCorrect
        {
            get { return isPasswordCorrect; }
            set
            {
                isPasswordCorrect = value;
                OnPropertyChanged(nameof(isPasswordCorrect));
            }
        }
        public SeriesCollection SeriesCollectionGrades { get; set; }
        public SeriesCollection SeriesCollectionFinished { get; set; }
        public string[] Labels { get; set; }
        public string[] LabelsFinished { get; set; }
        public Func<double, string> Formatter { get; set; }

        public GuideProfileViewModel()
        {
            UserService = new UserService();
            Guide = UserService.GetLoggedInUser();
            TourOccurrenceService = new TourOccurrenceService();
            VoucherService = new VoucherService();
            FinishedTours = TourOccurrenceService.GetFinishedToursById(Guide.Id);
            TourRatingService = new TourRatingService();
            AverageGrade = TourRatingService.GetAverageGrade(Guide.Id);
            IsPasswordCorrect = false;
            Password = "";
            ResignCommand = new ButtonCommandNoParameter(Resign);

            SeriesCollectionGrades = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Grades",
                    Values = new ChartValues<double>(),
                    Fill = Brushes.PapayaWhip
                }
            };
            foreach(var g in TourRatingService.GetAverageGradesForLanguages(Guide.Id).Values)
            {
                SeriesCollectionGrades[0].Values.Add(g);
            }
            SeriesCollectionFinished = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Tours",
                    Values = new ChartValues<double>(),
                    Fill = Brushes.SpringGreen
                }
            };
            foreach (var g in TourOccurrenceService.GetFinishedForLanguages(Guide.Id).Values)
            {
                SeriesCollectionFinished[0].Values.Add(g);
            }
            Formatter = value => value.ToString("N");
            Labels = TourRatingService.GetAverageGradesForLanguages(Guide.Id).Keys.ToArray();
            LabelsFinished = TourOccurrenceService.GetFinishedForLanguages(Guide.Id).Keys.ToArray();
        }
        public void Resign()
        {
            UserService.DeleteUser(Guide.Id);
            TourOccurrenceService.CancelAllTours(Guide.Id);
            VoucherService.UpdateVouchers(Guide.Id);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window w = Application.Current.Windows[0];
            w.Close();
        }
    }
}
