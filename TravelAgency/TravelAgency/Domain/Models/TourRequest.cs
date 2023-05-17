using System;
using System.Globalization;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public enum RequestStatus { Pending, Invalid, Accepted }
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int GuestNumber { get; set; }
        public DateOnly MinDate { get; set; }
        public DateOnly MaxDate { get; set; }
        public int GuestId { get; set; }
        public User Guest { get; set; }

        public RequestStatus Status { get; set; }
        public string GivenDate { get; set; }
        public int SpecialTourRequestId { get; set; }
        public TourRequest()
        {
            GivenDate = "/";
        }
        public TourRequest(Location location, string description, string language, int guestNumber, DateOnly minDate, DateOnly maxDate, int guestId, RequestStatus status)
        {
            Location = location;
            Description = description;
            Language = language;
            GuestNumber = guestNumber;
            MinDate = minDate;
            MaxDate = maxDate;
            GuestId = guestId;
            Status = status;
        }
        // kako da cuvam given date? kao string
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), LocationId.ToString(), Description, Language, GuestNumber.ToString(),
                                 MinDate.ToString("dd-MM-yyyy"), MaxDate.ToString("dd-MM-yyyy"), GuestId.ToString(),
                                 ((int)Status).ToString(), SpecialTourRequestId.ToString(), GivenDate};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            LocationId = int.Parse(values[1]);
            Description = values[2];
            Language = values[3];
            GuestNumber = int.Parse(values[4]);
            MinDate = DateOnly.ParseExact(values[5], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            MaxDate = DateOnly.ParseExact(values[6], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            GuestId = int.Parse(values[7]);
            Status = (RequestStatus)Convert.ToInt32(values[8]);
            SpecialTourRequestId = int.Parse(values[9]);
            GivenDate = values[10];
        }

        public bool Valid(string language, string numberOfGuests, DateOnly minDate, DateOnly maxDate)
        {
            int deltaDays= minDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;
            int result = 0;
            if (int.TryParse(numberOfGuests, out result))
            {
                if (result > 0)
                    GuestNumber = result;
                else
                    return false;
            }
            else
                return false;
            if (language != "" && deltaDays > 2 && maxDate > minDate)
            {
                Language = language;
                MinDate = minDate;
                MaxDate = maxDate;
                return true;
            }
            return false;
        }
        public void CheckIfExpired()
        {
            int currentDays = DateOnly.FromDateTime(DateTime.Now).DayNumber;
            if (MinDate.DayNumber - currentDays < 3)
            {
                Status = RequestStatus.Invalid;
            }
        }
        public bool MakeRequest(Location location, string language, string numberOfGuests, DateOnly minDate, DateOnly maxDate, string description, int guestId, int specialTourRequestId)
        {
            if (Valid(language, numberOfGuests, minDate, maxDate))
            {
                Location = location;
                LocationId = location.Id;
                Description = description;
                GuestId = guestId;
                Status = RequestStatus.Pending;
                SpecialTourRequestId = specialTourRequestId;
                GivenDate = "/";
                return true;
            }
            return false;
        }
    }
}
