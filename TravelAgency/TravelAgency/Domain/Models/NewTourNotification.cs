using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class NewTourNotification : ISerializable
    {
        public int TourId { get; set; }
        public int GuestId { get; set; }
        public bool Seen { get; set; }
        public Tour Tour { get; set; }
        public bool IsForLanguage { get; set; }
        public bool IsForLocation { get; set; }
        public string NotificationText { get; set; }
        public NewTourNotification()
        {

        }
        public NewTourNotification(int tourId, int guestId, bool seen, Tour tour, bool isForLanguage, bool isForLocation)
        {
            TourId = tourId;
            GuestId = guestId;
            Seen = seen;
            Tour = tour;
            IsForLanguage = isForLanguage;
            IsForLocation = isForLocation;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { TourId.ToString(), GuestId.ToString(), Seen.ToString(), IsForLanguage.ToString(), IsForLocation.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            TourId = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            Seen = bool.Parse(values[2]);
            IsForLanguage = bool.Parse(values[3]);
            IsForLocation = bool.Parse(values[4]);
        }

    }
}
