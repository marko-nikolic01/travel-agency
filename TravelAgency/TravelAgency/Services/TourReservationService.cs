using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class TourReservationService
    {
        public ITourReservationRepository ITourReservationRepository { get; set; }
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public IUserRepository IUserRepository { get; set; }
        public TourReservationService() 
        {
            ITourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }
        public void SubmitReservation(ObservableCollection<string> guestsList, TourOccurrence tourOccurrence)
        {
            List<User> users = new List<User>();
            TourReservation tourReservation;
            User user;
            for (int i = 0; i < guestsList.Count; i++)
            {
                user = GetUserByName(guestsList[i]);
                if (user == null)
                {
                    user = new User(guestsList[i], "ftn", Roles.Guest2, new DateOnly(2004, 2, 15));
                    IUserRepository.SaveUser(user);
                }
                users.Add(user);
                tourReservation = new TourReservation(tourOccurrence.Id, user.Id);
                ITourReservationRepository.Save(tourReservation);
            }
            tourOccurrence.Guests.AddRange(users);
            tourOccurrence.FreeSpots -= users.Count;
        }
        private User GetUserByName(string username)
        {
            return IUserRepository.GetAll().Find(x => (x.Username == username && x.Role == Roles.Guest2));
        }
        public bool IsTourReserved(int guestId, int tourOccurrenceId)
        {
            return ITourReservationRepository.IsTourReserved(guestId, tourOccurrenceId);
        }
        public List<TourOccurrence> GetAlternativeTours(TourOccurrence occurrence, int guestId)
        {
            
            List<TourOccurrence> result = ITourOccurrenceRepository.GetOfferedToursByLocation(occurrence.Tour.Location);
            result.Remove(occurrence);
            result = RemoveReservedTours(result, guestId);
            return result;
        }
        private List<TourOccurrence> RemoveReservedTours(List<TourOccurrence> occurrences, int guestId)
        {
            TourOccurrence tourOccurrence = null;
            foreach (TourReservation reservation in ITourReservationRepository.GetAll())
            {
                if (reservation.UserId == guestId && (tourOccurrence = occurrences.Find(t => t.Id == reservation.TourOccurrenceId)) != null)
                {
                    occurrences.Remove(tourOccurrence);
                }
            }
            return occurrences;
        }
    }
}
