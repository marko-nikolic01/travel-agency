using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public enum RequestStatus { Pending, Invalid, Accepted }
    public class TourRequest: ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int GuestNumber { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public int GuestId { get; set;}
        public User Guest { get; set; }

        public RequestStatus Status { get; set; }
        public DateTime? GivenDate { get; set; }
        public TourRequest()
        {
        }
        public TourRequest(Location location, string description, string language, int guestNumber, DateTime minDate, DateTime maxDate, int guestId, RequestStatus status, DateTime givenDate)
        {
            Location = location;
            Description = description;
            Language = language;
            GuestNumber = guestNumber;
            MinDate = minDate;
            MaxDate = maxDate;
            GuestId = guestId;
            Status = status;
            GivenDate = givenDate;
        }
        // kako da cuvam given date?
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), LocationId.ToString(), Description, Language, GuestNumber.ToString(),
                                 MinDate.ToString("dd-MM-yyyy HH-mm"), MaxDate.ToString("dd-MM-yyyy HH-mm"), GuestId.ToString(),
                                 ((int)Status).ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            LocationId = int.Parse(values[1]);
            Description = values[2];
            Language = values[3];
            GuestNumber = int.Parse(values[4]);
            MinDate = DateTime.ParseExact(values[5], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
            MaxDate = DateTime.ParseExact(values[6], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
            GuestId = int.Parse(values[7]);
            Status = (RequestStatus)Convert.ToInt32(values[8]);
        }

        public bool Valid(string language, string numberOfGuests)
        {
            int result = 0;
            if(language != null && int.TryParse(numberOfGuests, out result))
            {
                Language = language;
                GuestNumber = result;
                return true;
            }
            return false;
        }
    }
}
