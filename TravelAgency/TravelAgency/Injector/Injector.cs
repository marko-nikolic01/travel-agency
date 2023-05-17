using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Injector
{
    public class Injector
    {
        private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
        {
            { typeof(IVoucherRepository), new VoucherRepository() },
            { typeof(ITourOccurrenceRepository), new TourOccurrenceRepository() },
            { typeof(IUserRepository), new UserRepository() },
            { typeof(IPhotoRepository), new PhotoRepository() },
            { typeof(IKeyPointRepository), new KeyPointRepository() },
            { typeof(ITourRepository), new TourRepository() },
            { typeof(ITourReservationRepository), new TourReservationRepository() },
            { typeof(ITourRatingRepository), new TourRatingRepository() },
            { typeof(ITourRatingPhotoRepository), new TourRatingPhotoRepository() },
            { typeof(ILocationRepository), new LocationRepository() },
            { typeof(ITourOccurrenceAttendanceRepository), new TourOccurrenceAttendanceRepository() },
            { typeof(IAccommodationRepository), new AccommodationRepository() },
            { typeof(IAccommodationGuestRatingRepository), new AccommodationGuestRatingRepository() },
            { typeof(IAccommodationOwnerRatingRepository), new AccommodationOwnerRatingRepository() },
            { typeof(IAccommodationPhotoRepository), new AccommodationPhotoRepository() },
            { typeof(IAccommodationRatingPhotoRepository), new AccommodationRatingPhotoRepository() },
            { typeof(IAccommodationReservationMoveRequestRepository), new AccommodationReservationMoveRequestRepository() },
            { typeof(IAccommodationReservationRepository), new AccommodationReservationRepository() },
            { typeof(ITourRequestRepository), new TourRequestRepository() },
            { typeof(IRequestAcceptedNotificationRepository), new RequestAcceptedNotificationRepository() },
            { typeof(INewTourNotificationRepository), new NewTourNotificationRepository() },
            { typeof(ISpecialTourRequestRepository), new SpecialTourRequestRepository() },
            { typeof(IRenovationRecommendationRepository), new RenovationRecommendationRepository() },
            { typeof(ISuperGuestTitleRepository), new SuperGuestTitleRepository() },
            { typeof(IAccommodationRenovationRepository), new AccommodationRenovationRepository() }
        // Add more implementations here
        };

        public static T CreateInstance<T>()
        {
            Type type = typeof(T);

            if (_implementations.ContainsKey(type))
            {
                return (T)_implementations[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
