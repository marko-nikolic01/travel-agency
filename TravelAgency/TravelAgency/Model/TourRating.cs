﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourRating : Serializer.ISerializable
    {
        public int GuestId { get; set; }
        public int TourOccurrenceId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int Interesting { get; set; }
        public string? AdditionalComment { get; set; }
        public List<string>? PhotoUrls { get; set; }

        public TourRating(int guestId, int tourOccurrenceId, int guideKnowledge, int guideLanguage, int interesting, string additionalComment, List<string> photoUrls)
        {
            GuestId = guestId;
            TourOccurrenceId = tourOccurrenceId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            Interesting = interesting;
            AdditionalComment = additionalComment;
            PhotoUrls = photoUrls;
        }

        public TourRating()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { GuestId.ToString(), TourOccurrenceId.ToString(), GuideKnowledge.ToString(), GuideLanguage.ToString(), Interesting.ToString(), AdditionalComment};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            GuestId = int.Parse(values[0]);
            TourOccurrenceId = int.Parse(values[1]);
            GuideKnowledge = int.Parse(values[2]);
            GuideLanguage = int.Parse(values[3]);
            Interesting = int.Parse(values[4]);
            AdditionalComment = values[5];
        }
    }
}
