using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;

namespace TravelAgency.ViewModel
{
    public class TourDetailsViewModel : INotifyPropertyChanged
    {
        public TourRating TourRating { get; set; }
        public User Guest { get; set; }
        public TourOccurrence TourOccurrence { get; set; }
        public KeyPoint ArrivalKeyPoint { get; set; }

        private TourRatingPhoto currentPhoto;
        public TourRatingPhoto CurrentPhoto
        {
            get { return currentPhoto; }
            set
            {
                if (value != currentPhoto)
                {
                    currentPhoto = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourDetailsViewModel(TourRating tourRating)
        {
            TourRating = tourRating;
            if(tourRating.PhotoUrls.Count > 0)
            {
                CurrentPhoto = tourRating.PhotoUrls[0];
            }
        }
    }
}
