using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerPDFReportViewModel : ViewModelBase
    {
        public MyICommand GenerateReportCommand { get; set; }

        private User loggedInUser { get; set; }

        private UserService userService;
        private PDFReportService pdfReportService;

        private DateTime selectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return selectedStartDate; }
            set
            {
                selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
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
            }
        }

        public OwnerPDFReportViewModel()
        {
            userService = new UserService();
            pdfReportService = new PDFReportService();

            loggedInUser = userService.GetLoggedInUser();

            SelectedStartDate = DateTime.Now;
            SelectedEndDate = DateTime.Now;

            GenerateReportCommand = new MyICommand(Execute_GenerateReportCommand);
        }

        private void Execute_GenerateReportCommand()
        {
            pdfReportService.GenerateAccommodationRenovationsReport(loggedInUser, SelectedStartDate, SelectedEndDate);
        }
    }
}
