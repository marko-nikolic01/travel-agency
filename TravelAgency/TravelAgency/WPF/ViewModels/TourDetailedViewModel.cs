using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class TourDetailedViewModel
    {
        public string TourName { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public DateTime DateTime { get; set; }
        public string FreeSpots { get; set; }
        public string Language { get; set; }
        public string Location { get; set; }
        public string ButtonVisibility { get; set; }
        public string LabelVisibility { get; set; }
        public List<string> KeyPoints { get; set; }
        public TourOccurrence tourOccurrence;
        public int currentGuestId;
        public TourDetailedViewModel(TourOccurrence tourOccurrence, int guestId) 
        {
            currentGuestId = guestId;
            this.tourOccurrence = tourOccurrence;
            FillData();
            CheckIfTourIsReserved();
        }
        private void FillData()
        {
            TourName = tourOccurrence.Tour.Name;
            Language = tourOccurrence.Tour.Language;
            Duration = tourOccurrence.Tour.Duration.ToString()+" hours";
            DateTime = tourOccurrence.DateTime;
            Description = tourOccurrence.Tour.Description;
            Location = tourOccurrence.Tour.Location.City + ", " + tourOccurrence.Tour.Location.Country;
            int numberOfFreeSpots = tourOccurrence.Tour.MaxGuestNumber - tourOccurrence.Guests.Count;
            FreeSpots = numberOfFreeSpots.ToString();
            FillKeyPoints();
        }
        private void FillKeyPoints()
        {
            KeyPoints = new List<string>();
            foreach(KeyPoint keyPoint in tourOccurrence.KeyPoints) 
            {
                KeyPoints.Add(keyPoint.Name);
            }
        }
        private void CheckIfTourIsReserved()
        {
            TourReservationService reservationService = new TourReservationService();
            if (reservationService.IsTourReserved(currentGuestId, tourOccurrence.Id))
            {
                LabelVisibility = "Visible";
                ButtonVisibility = "Hidden";
            }
            else
            {
                LabelVisibility = "Hidden";
                ButtonVisibility = "Visible";
            }
        }
    }
}
