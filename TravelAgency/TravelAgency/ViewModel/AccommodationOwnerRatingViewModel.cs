using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Model;
using TravelAgency.Services;
using TravelAgency.View;

namespace TravelAgency.ViewModel
{
    public class AccommodationOwnerRatingViewModel
    {
        public AccommodationOwnerRatingService RatingService { get; set; }
        public AccommodationReservation Stay { get; set; }
        public AccommodationOwnerRating Rating { get; set; }
        public string Photo { get; set; }
        public AccommodationOwnerRatingViewModel(AccommodationReservation stay)
        {
            RatingService = new AccommodationOwnerRatingService();

            Stay = stay;
            Rating = new AccommodationOwnerRating(Stay);
            Photo = "";
        }

        public bool RateAccommodationOwner()
        {
            if (Rating.IsValid)
            {
                RatingService.CreateRating(Rating);
                Guest1Main.Stays.Remove(Stay);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddPhoto()
        {
            AccommodationRatingPhoto photo = new AccommodationRatingPhoto(Photo);
            Rating.Photos.Add(photo);
            Photo = "";
        }
    }
}
