using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class TourRating : Serializer.ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int TourOccurrenceId { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideLanguage { get; set; }
        public int Interesting { get; set; }
        public string? AdditionalComment { get; set; }
        public List<TourRatingPhoto> PhotoUrls { get; set; }

        private bool isValid;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsValid
        {
            get { return isValid; }
            set
            {
                if (value != isValid)
                {
                    isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public TourRating(int guestId, int tourOccurrenceId, int guideKnowledge, int guideLanguage, int interesting, string additionalComment, List<TourRatingPhoto> photoUrls)
        {
            GuestId = guestId;
            TourOccurrenceId = tourOccurrenceId;
            GuideKnowledge = guideKnowledge;
            GuideLanguage = guideLanguage;
            Interesting = interesting;
            AdditionalComment = additionalComment;
            PhotoUrls = photoUrls;
            IsValid = true;
        }

        public TourRating()
        {
            IsValid = true;
            PhotoUrls = new List<TourRatingPhoto>();
        }

        public string[] ToCSV()
        {
            string[] csvValues = {Id.ToString(), GuestId.ToString(), TourOccurrenceId.ToString(), GuideKnowledge.ToString(), GuideLanguage.ToString(), Interesting.ToString(), AdditionalComment, IsValid.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            TourOccurrenceId = int.Parse(values[2]);
            GuideKnowledge = int.Parse(values[3]);
            GuideLanguage = int.Parse(values[4]);
            Interesting = int.Parse(values[5]);
            AdditionalComment = values[6];
            IsValid = bool.Parse(values[7]);
        }
    }
}
