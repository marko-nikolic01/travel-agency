using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.Views;

namespace TravelAgency.WPF.ViewModels
{
    public class AccommodationReservationMoveRequestViewModel
    {
        public AccommodationReservationMoveRequest SelectedMoveRequest { get; set; }
        public AccommodationReservationMoveService MoveReqestService { get; set; }

        public AccommodationReservationMoveRequestViewModel(AccommodationReservationMoveRequest moveRequest)
        {
            SelectedMoveRequest = moveRequest;

            MoveReqestService = new AccommodationReservationMoveService();
        }

        public bool CanSelectedReservationBeMoved()
        {
            return MoveReqestService.CanReservationBeMoved(SelectedMoveRequest);
        }

        public void AcceptSelectedMoveRequest()
        {
            MoveReqestService.AcceptMoveRequest(SelectedMoveRequest);
            OwnerMain.AccommodationReservationMoveRequests.Remove(SelectedMoveRequest);
        }

        public void RejectSelectedMoveRequest()
        {
            MoveReqestService.RejectMoveRequest(SelectedMoveRequest);
            OwnerMain.AccommodationReservationMoveRequests.Remove(SelectedMoveRequest);
        }
    }
}
