using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.Injector;
using TravelAgency.Observer;
using TravelAgency.Repository;
using TravelAgency.RepositoryInterfaces;
using System.Globalization;
using System.Windows.Controls;

namespace TravelAgency.Services
{
    public class TourOccurrenceService
    {
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public ITourRepository ITourRepository { get; set; }
        public IPhotoRepository IPhotoRepository { get; set; }
        public IKeyPointRepository IKeyPointRepository { get; set; }
        public IVoucherRepository IVoucherRepository { get; set; }
        public ITourReservationRepository ITourReservationRepository { get; set; }
        public ILocationRepository ILocationRepository { get; set; }
        public ITourOccurrenceAttendanceRepository ITourOccurrenceAttendanceRepository { get; set; }
        public TourOccurrenceService()
        {
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IUserRepository userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            IPhotoRepository = Injector.Injector.CreateInstance<IPhotoRepository>();
            ITourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            IKeyPointRepository = Injector.Injector.CreateInstance<IKeyPointRepository>();
            ILocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            ITourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            ITourOccurrenceAttendanceRepository = Injector.Injector.CreateInstance<ITourOccurrenceAttendanceRepository>();
            IVoucherRepository = Injector.Injector.CreateInstance<IVoucherRepository>();
            LinkTourLocations(ILocationRepository, ITourRepository);
            LinkTourPhotos(IPhotoRepository, ITourRepository);
            LinkTourOccurrences(ITourRepository);
            LinkTourGuests(ITourReservationRepository, userRepository);
            LinkKeyPoints(IKeyPointRepository);
        }
        private void LinkKeyPoints(IKeyPointRepository keyPointRepository)
        {
            foreach (KeyPoint keyPoint in keyPointRepository.GetAll())
            {
                TourOccurrence tourOccurrence = ITourOccurrenceRepository.GetAll().Find(tO => tO.Id == keyPoint.TourOccurrenceId);
                if (tourOccurrence != null)
                {
                    tourOccurrence.KeyPoints.Add(keyPoint);
                }
            }
        }
        private void LinkTourGuests(ITourReservationRepository reservationRepository, IUserRepository userRepository)
        {
            foreach (TourReservation tourReservation in reservationRepository.GetTourReservations())
            {
                TourOccurrence tourOccurrence = ITourOccurrenceRepository.GetAll().Find(x => x.Id == tourReservation.TourOccurrenceId);
                User guest = userRepository.GetUsers().Find(x => x.Id == tourReservation.UserId);
                if (tourOccurrence != null && guest != null)
                {
                    tourOccurrence.Guests.Add(guest);
                }
            }
        }
        private void LinkTourOccurrences(ITourRepository tourRepository)
        {
            foreach (TourOccurrence tourOccurrence in ITourOccurrenceRepository.GetAll())
            {
                Tour tour = tourRepository.GetAll().Find(t => t.Id == tourOccurrence.TourId);
                if (tour != null)
                {
                    tourOccurrence.Tour = tour;
                }
            }
        }
        private static void LinkTourPhotos(IPhotoRepository photoRepository, ITourRepository tourRepository)
        {
            foreach (Photo photo in photoRepository.GetAll())
            {
                Tour tour = tourRepository.GetAll().Find(t => t.Id == photo.TourId);
                if (tour != null)
                {
                    tour.Photos.Add(photo);
                }
            }
        }
        private static void LinkTourLocations(ILocationRepository locationRepository, ITourRepository tourRepository)
        {
            foreach (var tour in tourRepository.GetAll())
            {
                Location location = locationRepository.GetAll().Find(l => l.Id == tour.LocationId);
                if (location != null)
                {
                    tour.Location = location;
                }
            }
        }

        public void CancelTour(TourOccurrence SelectedTourOccurrence, int ActiveGuideId)
        {
            foreach (var guest in SelectedTourOccurrence.Guests)
            {
                IVoucherRepository.Save(new Voucher() { GuestId = guest.Id, GuideId = ActiveGuideId, Deadline = DateTime.Now.AddYears(1) });
            }
            ITourOccurrenceRepository.Delete(SelectedTourOccurrence);
        }

        public void Subscribe(IObserver observer)
        {
            ITourOccurrenceRepository.Subscribe(observer);
        }

        public TourOccurrence GetMostVisitedAllTime(int guideId)
        {
            TourOccurrence mostVisited = ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId)[0];
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId))
            {
                if(ITourOccurrenceAttendanceRepository.GetCountForTour(tourOccurrence.Id) > ITourOccurrenceAttendanceRepository.GetCountForTour(mostVisited.Id)){
                    mostVisited = tourOccurrence;
                }
            }
            return mostVisited;
        }

        public TourOccurrence GetMostVisitedByYear(int guideId, int year)
        {
            TourOccurrence mostVisited = ITourOccurrenceRepository.GetFinishedOccurrencesForGuideByYear(guideId, year)[0];
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuideByYear(guideId, year))
            {
                if (ITourOccurrenceAttendanceRepository.GetCountForTour(tourOccurrence.Id) > ITourOccurrenceAttendanceRepository.GetCountForTour(mostVisited.Id))
                {
                    mostVisited = tourOccurrence;
                }
            }
            return mostVisited;
        }

        public List<TourOccurrence> GetFinishedOccurrencesForGuide(int guideId)
        {
            return ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId);
        }

        public List<TourOccurrence> GetUpcomingToursForGuide(int guideId)
        {
            return ITourOccurrenceRepository.GetUpcomings(guideId);
        }
        public List<TourOccurrence> GetFinishedToursForGuide(int guideId)
        {
            return ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId);
        }

        public void SaveNewTours(Tour newTour, ItemCollection links, ItemCollection dateTimes, ItemCollection keyPoints, User activeGuide)
        {
            ITourRepository.Save(newTour);
            SavePhotos(newTour, links);
            foreach (string dateTimeItem in dateTimes)
            {
                DateTime dateTime = DateTime.ParseExact(dateTimeItem, "dd-MM-yyyy HH:mm", new CultureInfo("en-US"));
                TourOccurrence tourOccurrence = SaveTourOccurrence(dateTime, newTour, activeGuide);

                foreach (string keyPointItem in keyPoints)
                {
                    SaveKeyPoint(keyPointItem, tourOccurrence);
                }
            }
        }
        private void SavePhotos(Tour newTour, ItemCollection links)
        {
            foreach (string link in links)
            {
                Photo photo = new Photo();
                photo.TourId = newTour.Id;
                photo.Link = link;
                newTour.Photos.Add(photo);
                IPhotoRepository.Save(photo);
            }
        }
        private TourOccurrence SaveTourOccurrence(DateTime dateTime, Tour newTour, User activeGuide)
        {
            TourOccurrence tourOccurrence = new TourOccurrence();
            tourOccurrence.TourId = newTour.Id;
            tourOccurrence.Tour = newTour;
            tourOccurrence.DateTime = dateTime;
            tourOccurrence.FreeSpots = newTour.MaxGuestNumber;
            return ITourOccurrenceRepository.SaveTourOccurrence(tourOccurrence, activeGuide);
        }
        private void SaveKeyPoint(string keyPointItem, TourOccurrence tourOccurrence)
        {
            KeyPoint keyPoint = new KeyPoint();
            keyPoint.TourOccurrenceId = tourOccurrence.Id;
            keyPoint.Name = keyPointItem;
            tourOccurrence.KeyPoints.Add(keyPoint);
            IKeyPointRepository.Save(keyPoint);
        }
    }
}
