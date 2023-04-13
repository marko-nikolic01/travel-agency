using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Repository;

namespace TravelAgency.Services
{
    public class TourOccurrenceService
    {
        public TourOccurrenceService()
        {
        }

        public TourOccurrence GetMostVisitedAllTime(int guideId)
        {
            UserRepository userRepository = new UserRepository();
            PhotoRepository photoRepository = new PhotoRepository();
            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            KeyPointRepository keyPointRepository = new KeyPointRepository();  
            TourOccurrenceRepository tourOccurrenceRepository = new TourOccurrenceRepository(photoRepository, locationRepository, tourRepository, tourReservationRepository, userRepository, keyPointRepository);
            TourOccurrenceAttendanceRepository attendanceRepository = new TourOccurrenceAttendanceRepository();
            TourOccurrence mostVisited = tourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId)[0];
            foreach (var tourOccurrence in tourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId))
            {
                if(attendanceRepository.GetCountForTour(tourOccurrence.Id) > attendanceRepository.GetCountForTour(mostVisited.Id)){
                    mostVisited = tourOccurrence;
                }
            }
            return mostVisited;
        }

        public TourOccurrence GetMostVisitedByYear(int guideId, int year)
        {
            UserRepository userRepository = new UserRepository();
            PhotoRepository photoRepository = new PhotoRepository();
            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            KeyPointRepository keyPointRepository = new KeyPointRepository();
            TourOccurrenceRepository tourOccurrenceRepository = new TourOccurrenceRepository(photoRepository, locationRepository, tourRepository, tourReservationRepository, userRepository, keyPointRepository);
            TourOccurrenceAttendanceRepository attendanceRepository = new TourOccurrenceAttendanceRepository();
            TourOccurrence mostVisited = tourOccurrenceRepository.GetFinishedOccurrencesForGuideByYear(guideId, year)[0];
            foreach (var tourOccurrence in tourOccurrenceRepository.GetFinishedOccurrencesForGuideByYear(guideId, year))
            {
                if (attendanceRepository.GetCountForTour(tourOccurrence.Id) > attendanceRepository.GetCountForTour(mostVisited.Id))
                {
                    mostVisited = tourOccurrence;
                }
            }
            return mostVisited;
        }

        public List<TourOccurrence> GetFinishedOccurrencesForGuide(int guideId)
        {
            UserRepository userRepository = new UserRepository();
            PhotoRepository photoRepository = new PhotoRepository();
            LocationRepository locationRepository = new LocationRepository();
            TourRepository tourRepository = new TourRepository();
            TourReservationRepository tourReservationRepository = new TourReservationRepository();
            KeyPointRepository keyPointRepository = new KeyPointRepository();
            TourOccurrenceRepository tourOccurrenceRepository = new TourOccurrenceRepository(photoRepository, locationRepository, tourRepository, tourReservationRepository, userRepository, keyPointRepository);
            return tourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId);
        }
    }
}
