using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
