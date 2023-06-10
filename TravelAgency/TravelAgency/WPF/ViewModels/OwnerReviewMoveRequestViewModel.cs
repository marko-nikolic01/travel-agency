using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerReviewMoveRequestViewModel : ViewModelBase
    {
        public MyICommand NavigateBackCommand { get; set; }
        public MyICommand RejectRequestCommand { get; set; }

        private User loggedInUser;

        private NavigationService navigationService;

        private UserService userService;
        private AccommodationReservationMoveService accommodationReservationMoveService;

        private string explanationText;
        public string ExplanationText
        {
            get { return explanationText; }
            set
            {
                explanationText = value;
                OnPropertyChanged(nameof(ExplanationText));
            }
        }

        public AccommodationReservationMoveRequest SelectedMoveRequest { get; set; }

        public OwnerReviewMoveRequestViewModel(AccommodationReservationMoveRequest selectedMoveRequest, NavigationService navigationService)
        {
            userService = new UserService();
            accommodationReservationMoveService = new AccommodationReservationMoveService();

            loggedInUser = userService.GetLoggedInUser();

            this.navigationService = navigationService;

            SelectedMoveRequest = selectedMoveRequest;

            NavigateBackCommand = new MyICommand(Execute_RejectRequestCommand);
            RejectRequestCommand = new MyICommand(Execute_RejectRequestCommand);

            ExplanationText = string.Empty;
        }

        private void Execute_RejectRequestCommand()
        {
            if (ExplanationText == string.Empty)
            {
                MessageBox.Show("Enter an explanation.");
                return;
            }

            SelectedMoveRequest.RejectionExplanation = ExplanationText;
            accommodationReservationMoveService.RejectMoveRequest(SelectedMoveRequest);
            MessageBox.Show("Request rejected successfully.");
            Execute_NavigateBackCommand();
        }

        private void Execute_NavigateBackCommand()
        {
            this.navigationService.Navigate(new Uri("WPF/Views/OwnerReservationsView.xaml", UriKind.Relative));
        }
    }
}
