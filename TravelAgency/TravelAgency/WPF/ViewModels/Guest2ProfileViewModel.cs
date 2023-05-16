using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class Guest2ProfileViewModel
    {
        public int GuestId { get; set; }    
        public string Username { get; set; }    
        public DateTime StartDate { get; set; }    
        public DateTime EndDate { get; set; }
        public GuestAttendanceService service;
        public List<TourOccurrenceAttendanceDTO> tourOccurrenceAttendanceDTOs;
        public Guest2ProfileViewModel(int guestId) 
        {
            new TourOccurrenceService();
            service = new GuestAttendanceService();
            StartDate = new DateTime(2022, 1, 1);
            EndDate = new DateTime(2023, 1, 1);
            GuestId = guestId;
            LoadUserData();
        }
        private void LoadUserData()
        {
            UserService userService = new UserService();
            Username = userService.GetById(GuestId).Username;
        }
        public void PrepareData()
        {
            tourOccurrenceAttendanceDTOs = service.GetAttendancesForPeriod(GuestId, StartDate, EndDate);
        }
    }
}
