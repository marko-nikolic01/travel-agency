using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace TravelAgency.Domain.Models
{
    public class RequestAcceptedNotification : Serializer.ISerializable
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int GuideId { get; set; }
        public int RequestId { get; set; }
        public bool IsSeen { get; set; }

        public RequestAcceptedNotification()
        {
        }

        public RequestAcceptedNotification(DateTime dateTime, int guideId, int requestId, bool isSeen)
        {
            DateTime = dateTime;
            GuideId = guideId;
            RequestId = requestId;
            IsSeen = isSeen;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),  DateTime.ToString("dd-MM-yyyy HH-mm"), GuideId.ToString(), RequestId.ToString(), IsSeen.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            DateTime = DateTime.ParseExact(values[1], "dd-MM-yyyy HH-mm", CultureInfo.InvariantCulture);
            GuideId = int.Parse(values[2]);
            RequestId = int.Parse(values[3]);
            IsSeen = bool.Parse(values[4]);
        }
    }
}
