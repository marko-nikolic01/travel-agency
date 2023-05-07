﻿using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels

{
    public class FinishedTourViewModel
    {
        public string GuideName { get; set; }
        public string TourName { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string KeyPointOfArrival { get; set; }
        public string ButtonVisibility { get; set; }
        public TourOccurrence tourOccurrence;
        private TourOccurrenceAttendanceService attendanceService;
        
        public int guestId;
        public FinishedTourViewModel(TourOccurrence tourOccurrence, int guestId)
        {
            this.tourOccurrence= tourOccurrence;
            this.guestId= guestId;
            attendanceService = new TourOccurrenceAttendanceService();
            SetStringValues();
            SetButtonVisibility();
        }
        private void SetStringValues()
        {
            GuideName = tourOccurrence.Guide.Username;
            TourName = tourOccurrence.Tour.Name;
            Location = tourOccurrence.Tour.Location.Country+", "+ tourOccurrence.Tour.Location.City;
            Language = tourOccurrence.Tour.Language;
            Date = tourOccurrence.DateTime.ToString();
            Description = tourOccurrence.Tour.Description;
            Duration = tourOccurrence.Tour.Duration+" hours";
            TourName = tourOccurrence.Tour.Name;
            KeyPointOfArrival = attendanceService.GetArrivalKeyPoint(tourOccurrence.Id, guestId);
        }
        private void SetButtonVisibility()
        {
            TourRatingService ratingService = new TourRatingService();
            if (ratingService.IsTourNotRated(guestId, tourOccurrence.Id))
                ButtonVisibility = "Visible";
            else
                ButtonVisibility = "Hidden";
        }
    }
}