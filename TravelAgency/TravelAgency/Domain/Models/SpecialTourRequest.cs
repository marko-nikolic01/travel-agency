using System;
using System.Collections.Generic;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public enum SpecialRequestStatus { Pending, Invalid, Accepted }
    public class SpecialTourRequest : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public List<TourRequest> TourRequests { get; set; }
        public SpecialRequestStatus Status { get; set; }
        public int SerialNumber;
        public string SpecialTourRequestString { get; set; }
        public SpecialTourRequest() 
        {
            TourRequests = new List<TourRequest>();
        }
        public void BuildRequestString()
        {
            SpecialTourRequestString = "Special tour request #"+SerialNumber+"; "+TourRequests.Count+" items, Status: "+Status;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), ((int)Status).ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            Status = (SpecialRequestStatus)Convert.ToInt32(values[2]);
        }
    }
}
