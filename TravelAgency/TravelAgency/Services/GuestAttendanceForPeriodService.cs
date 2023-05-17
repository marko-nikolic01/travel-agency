using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class GuestAttendanceForPeriodService
    {
        public ITourOccurrenceAttendanceRepository ITourOccurrenceAttendanceRepository { get; set; }
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public ITourReservationRepository ITourReservationRepository { get; set; }
        public IKeyPointRepository IKeyPointRepository { get; set; }
        public GuestAttendanceForPeriodService()
        {
            ITourOccurrenceAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            ITourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            IKeyPointRepository = Injector.Injector.CreateInstance<IKeyPointRepository>();
        }
        public List<TourOccurrenceAttendanceDTO> GetAttendancesForPeriod(int guestId, DateTime startDate, DateTime endDate)
        {
            return GetAttendances(guestId, GetAppropriateTourOccurrences(guestId, startDate, endDate));
        }
        private List<TourOccurrence> GetAppropriateTourOccurrences(int guestId, DateTime startDate, DateTime endDate)
        {
            List<TourOccurrence> tourOccurrences = new List<TourOccurrence>();
            foreach (TourReservation reservation in ITourReservationRepository.GetAllForGuest(guestId))
            {
                TourOccurrence tourOccurrence = ITourOccurrenceRepository.GetById(reservation.TourOccurrenceId);
                if (tourOccurrence.IsInAppropriateDateSpan(startDate, endDate))
                    tourOccurrences.Add(tourOccurrence);
            }
            return tourOccurrences;
        }
        private List<TourOccurrenceAttendanceDTO> GetAttendances(int guestId, List<TourOccurrence> tourOccurrences)
        {
            List<TourOccurrenceAttendanceDTO> attendancesDTOs = new List<TourOccurrenceAttendanceDTO>();
            foreach (TourOccurrence tourOccurrence in tourOccurrences) 
            {
                TourOccurrenceAttendance attendance = ITourOccurrenceAttendanceRepository.GetByTourOccurrenceIdAndGuestId(tourOccurrence.Id, guestId);
                attendancesDTOs.Add(GetAttendance(tourOccurrence, attendance));
            }
            return attendancesDTOs;
        }
        private TourOccurrenceAttendanceDTO GetAttendance(TourOccurrence tourOccurrence, TourOccurrenceAttendance? attendance)
        {
            TourOccurrenceAttendanceDTO attendanceDTO;
            if (attendance == null)
            {
                attendanceDTO = new TourOccurrenceAttendanceDTO(tourOccurrence.Tour.Name, "Didn't show up", "/", tourOccurrence.DateTime);
            }
            else if (attendance.ResponseStatus == ResponseStatus.NotAnsweredYet)
            {
                attendanceDTO = new TourOccurrenceAttendanceDTO(tourOccurrence.Tour.Name, "Marked as present but not confirmed", "/", tourOccurrence.DateTime);
            }
            else if (attendance.ResponseStatus == ResponseStatus.Declined)
            {
                attendanceDTO = new TourOccurrenceAttendanceDTO(tourOccurrence.Tour.Name, "Marked as present but declined presence", "/", tourOccurrence.DateTime);
            }
            else
            {
                KeyPoint keyPoint = IKeyPointRepository.GetById(attendance.KeyPointId);
                attendanceDTO = new TourOccurrenceAttendanceDTO(tourOccurrence.Tour.Name, "Was present on the tour", keyPoint.Name, tourOccurrence.DateTime);
            }
            return attendanceDTO;
        }
    }
}
