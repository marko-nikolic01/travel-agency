using System;
using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using System.Globalization;
using System.Windows.Controls;
using TravelAgency.Domain.RepositoryInterfaces;
using System.Collections.ObjectModel;
using static System.Windows.Forms.LinkLabel;
using System.Windows;
using System.Linq;

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
        public IUserRepository IUserRepository { get; set; }
        public TourOccurrenceService()
        {
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
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
            LinkTourGuests(ITourReservationRepository, IUserRepository);
            LinkKeyPoints();
            LinkTourGuide();
        }
        private void LinkKeyPoints()
        {
            foreach (TourOccurrence tourOccurrence in ITourOccurrenceRepository.GetAll())
            {
                tourOccurrence.KeyPoints.Clear();
                tourOccurrence.KeyPoints.AddRange(IKeyPointRepository.GetByTourOccurrence(tourOccurrence.Id));
                foreach(var k in tourOccurrence.KeyPoints)
                {
                    if (k.IsChecked && tourOccurrence.CurrentState != CurrentState.Ended)
                    {
                        tourOccurrence.ActiveKeyPointId = k.Id;
                    }
                }
                if(tourOccurrence.ActiveKeyPointId != -1 && tourOccurrence.ActiveKeyPointId != tourOccurrence.KeyPoints[tourOccurrence.KeyPoints.Count-1].Id && tourOccurrence.CurrentState != CurrentState.Ended)
                {
                    tourOccurrence.CurrentState = CurrentState.Started;
                    UpdateTourOccurrence(tourOccurrence.Id);
                }
            }
        }
        private void LinkTourGuide()
        {
            foreach (TourOccurrence tourOccurrence in ITourOccurrenceRepository.GetAll())
            {
                User user = IUserRepository.GetById(tourOccurrence.GuideId);
                if (user != null)
                {
                    tourOccurrence.Guide = user;
                }
            }
        }
        private void LinkTourGuests(ITourReservationRepository reservationRepository, IUserRepository userRepository)
        {
            foreach (TourOccurrence tourOccurrence in ITourOccurrenceRepository.GetAll())
                tourOccurrence.Guests.Clear();
            foreach (TourReservation tourReservation in reservationRepository.GetAll())
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
        private void LinkTourPhotos(IPhotoRepository photoRepository, ITourRepository tourRepository)
        {
            foreach (Tour tour in ITourRepository.GetAll())
            {
                tour.Photos.Clear();
                tour.Photos.AddRange(IPhotoRepository.GetByTour(tour.Id));
            }
        }
        private void LinkTourLocations(ILocationRepository locationRepository, ITourRepository tourRepository)
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
        public int CancelTour(TourOccurrence SelectedTourOccurrence, int ActiveGuideId)
        {
            foreach (var guest in SelectedTourOccurrence.Guests)
            {
                IVoucherRepository.Save(new Voucher() { GuestId = guest.Id, GuideId = ActiveGuideId, Deadline = DateTime.Now.AddYears(1), CanceledTourOccurrenceId = SelectedTourOccurrence.Id });
            }
            return ITourOccurrenceRepository.Delete(SelectedTourOccurrence);
        }
        public void CancelTourResign(TourOccurrence SelectedTourOccurrence)
        {
            foreach (var guest in SelectedTourOccurrence.Guests)
            {
                IVoucherRepository.Save(new Voucher() { GuestId = guest.Id, GuideId = -1, Deadline = DateTime.Now.AddYears(2), CanceledTourOccurrenceId = SelectedTourOccurrence.Id });
            }
            ITourOccurrenceRepository.Delete(SelectedTourOccurrence);
        }
        public void CancelAllTours(int ActiveGuideId)
        {
            List<TourOccurrence> tours = new List<TourOccurrence>();
            tours.AddRange(ITourOccurrenceRepository.GetTodays(ActiveGuideId));
            tours.AddRange(ITourOccurrenceRepository.GetUpcomings(ActiveGuideId));
            foreach(TourOccurrence tourOccurrence in tours)
            {
                if(tourOccurrence.CurrentState == CurrentState.NotStarted)
                {
                    CancelTourResign(tourOccurrence);
                }
            }
        }
        public void Subscribe(IObserver observer)
        {
            ITourOccurrenceRepository.Subscribe(observer);
        }
        public TourOccurrence GetMostVisitedAllTime(int guideId)
        {
            if (ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId).Count == 0)
            {
                return null;
            }
            TourOccurrence mostVisited = ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId)[0];
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(guideId))
            {
                if (ITourOccurrenceAttendanceRepository.GetCountForTour(tourOccurrence.Id) > ITourOccurrenceAttendanceRepository.GetCountForTour(mostVisited.Id))
                {
                    mostVisited = tourOccurrence;
                }
            }
            return mostVisited;
        }

        public TourOccurrence GetMostVisitedByYear(int guideId, int year)
        {
            if (ITourOccurrenceRepository.GetFinishedOccurrencesForGuideByYear(guideId, year).Count == 0)
            {
                return null;
            }
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
        public void UpdateTour(TourOccurrence tourOccurrence)
        {
            ITourOccurrenceRepository.UpdateTourOccurrence(tourOccurrence);
        }
        public List<TourOccurrence> GetOfferedTours()
        {
            return ITourOccurrenceRepository.GetOffered();
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

        public Tour SaveNewTours(Tour newTour, ItemCollection links, ItemCollection dateTimes, ItemCollection keyPoints, User activeGuide)
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
            return newTour;
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
            return ITourOccurrenceRepository.SaveTourOccurrence(tourOccurrence, activeGuide.Id);
        }
        private void SaveKeyPoint(string keyPointItem, TourOccurrence tourOccurrence)
        {
            KeyPoint keyPoint = new KeyPoint();
            keyPoint.TourOccurrenceId = tourOccurrence.Id;
            keyPoint.Name = keyPointItem;
            tourOccurrence.KeyPoints.Add(keyPoint);
            IKeyPointRepository.Save(keyPoint);
        }
        public List<TourOccurrence> GetFinishedOccurrencesForGuest(int guestId)
        {
            List<TourOccurrence> result = new List<TourOccurrence>();
            foreach (TourOccurrence occurrence in ITourOccurrenceRepository.GetAll())
            {
                if (occurrence.CurrentState == CurrentState.Ended)
                {
                    if (WasGuestOnTour(occurrence, guestId))
                        result.Add(occurrence);
                }
            }
            return result;
        }
        private bool WasGuestOnTour(TourOccurrence occurrence, int guestId)
        {
            TourOccurrenceAttendance attendance;
            attendance = ITourOccurrenceAttendanceRepository.GetAll().Find(x => x.TourOccurrenceId == occurrence.Id && x.GuestId == guestId);
            if (attendance != null)
            {
                if (attendance.ResponseStatus == ResponseStatus.Accepted)
                    return true;
            }
            return false;
        }
        public string GetActiveTour(int guestId)
        {
            string result = "There is no active tour";
            foreach (TourOccurrence occurrence in ITourOccurrenceRepository.GetAll())
            {
                if (occurrence.CurrentState == CurrentState.Started && occurrence.DateTime.Date.Equals(DateTime.Now.Date))
                {
                    TourOccurrenceAttendance tourAttendance = ITourOccurrenceAttendanceRepository.GetByTourOccurrenceIdAndGuestId(occurrence.Id, guestId);
                    if (tourAttendance != null)
                    {
                        result = occurrence.GetActiveTourString(IKeyPointRepository.GetById(occurrence.ActiveKeyPointId).Name);
                        result += "\nStatus: " + tourAttendance.ResponseStatus.ToString();
                        return result;
                    }
                    else if (ITourReservationRepository.IsTourReserved(guestId, occurrence.Id))
                    {
                        result = occurrence.GetActiveTourString(IKeyPointRepository.GetById(occurrence.ActiveKeyPointId).Name);
                        result += "\nStatus: haven't arrived yet";
                        return result;
                    }
                }
            }
            return result;
        }
        public List<TourOccurrence> GetTodays(int activeGuideId)
        {
            return ITourOccurrenceRepository.GetTodays(activeGuideId);
        }
        public TourOccurrence GetByTourId(int id)
        {
            return ITourOccurrenceRepository.GetByTourId(id);
        }
        public TourOccurrence GetById(int id)
        {
            return ITourOccurrenceRepository.GetById(id);
        }

        public void UpdateTourOccurrence(int tourOccurrenceId)
        {
            TourOccurrence tourOccurrence = ITourOccurrenceRepository.GetAll().Find(t => t.Id == tourOccurrenceId);
            if(tourOccurrence != null)
            {
                ITourOccurrenceRepository.UpdateTourOccurrence(tourOccurrence);
            }
        }
        public int AcceptRequest(TourRequest request, DateTime dateTime, int GuideId, ObservableCollection<string> keyPoints, int duration)
        {
            Tour newTour = GenerateNewTour(request, duration);
            ITourRepository.Save(newTour);
            SavePhoto(newTour, "https://assets.thehansindia.com/h-upload/2019/12/27/248830-worldtour.jpg");
            TourOccurrence newTourOccurrence = GenerateNewTourOccurrence(newTour, dateTime);
            ITourOccurrenceRepository.SaveTourOccurrence(newTourOccurrence, GuideId);
            foreach(string keyPoint in keyPoints)
            {
                SaveKeyPoint(keyPoint, newTourOccurrence);
            }
            return newTourOccurrence.Id;
        }
        private TourOccurrence GenerateNewTourOccurrence(Tour newTour, DateTime dateTime)
        {
            TourOccurrence tourOccurrence = new TourOccurrence();
            tourOccurrence.TourId = newTour.Id;
            tourOccurrence.Tour = newTour;
            tourOccurrence.DateTime = dateTime;
            tourOccurrence.FreeSpots = newTour.MaxGuestNumber;
            return tourOccurrence;
        }
        private Tour GenerateNewTour(TourRequest request, int duration)
        {
            Tour newTour = new Tour();
            newTour.Language = request.Language;
            newTour.Description = request.Description;
            newTour.MaxGuestNumber = request.GuestNumber;
            newTour.LocationId = request.LocationId;
            newTour.Location = request.Location;
            newTour.Duration = duration;
            return newTour;
        }
        private void SavePhoto(Tour newTour, string v)
        {
            Photo photo = new Photo();
            photo.TourId = newTour.Id;
            photo.Link = v;
            newTour.Photos.Add(photo);
            IPhotoRepository.Save(photo);
        }
        public void UndoCancelTour(int canceledTour)
        {
            IVoucherRepository.DeleteByCanceledTourId(canceledTour);
            ITourOccurrenceRepository.UndoDelete(canceledTour);
        }
        public bool IsGuideFree(int guideId, DateTime concreteDateTime, int duration)
        {
            bool isGuideFree = true;
            var upcommingTourOccurrences = ITourOccurrenceRepository.GetUpcomings(guideId);
            foreach (var tourOccurrence in upcommingTourOccurrences)
            {
                isGuideFree = isGuideFree && tourOccurrence.IsDateTimeFree(concreteDateTime, duration);
            }
            return isGuideFree;
        }
        public int GetFinishedToursById(int id)
        {
            return ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id).Count;
        }
        public double GetFinishedForLanguage(int id, string l)
        {
            double sum = 0.0;
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                if (tourOccurrence.Tour.Language.Equals(l))
                {
                    sum++;
                }
            }
            return sum;
        }
        public string[] GetUniqeLanguages(int id)
        {
            HashSet<string> uniqueLanguages = new HashSet<string>();
            foreach (var r in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                uniqueLanguages.Add(r.Tour.Language);
            }
            return uniqueLanguages.ToArray<string>();
        }
        public Dictionary<string, double> GetFinishedForLanguages(int id)
        {
            Dictionary<string, double> stats = new Dictionary<string, double>();
            foreach (string l in GetUniqeLanguages(id))
            {
                stats[l] = GetFinishedForLanguage(id, l);
            }
            return stats;
        }
    }
}
