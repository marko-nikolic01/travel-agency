using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Repository;
using TravelAgency.RepositoryInterfaces;

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
        { typeof(ILocationRepository), new LocationRepository() },
        { typeof(ITourOccurrenceAttendanceRepository), new TourOccurrenceAttendanceRepository() },
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
