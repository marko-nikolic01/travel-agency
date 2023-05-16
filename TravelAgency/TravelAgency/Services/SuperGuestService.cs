using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class SuperGuestService
    {
        public ISuperGuestTitleRepository TitleRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }

        public SuperGuestService()
        {
            TitleRepository = Injector.Injector.CreateInstance<ISuperGuestTitleRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository.LinkSuperGuestTitles(TitleRepository.GetAll());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
        }

        public void CheckSuperGuest(User guest)
        {
            if (!guest.IsSuperGuest)
            {
                List<AccommodationReservation> reservations = ReservationRepository.GetAllNotCanceledByGuest(guest);
                if (CountLastYearReservations(reservations) >= 10)
                {
                    CreateSuperGuestTitle(guest);
                }
            }
            else if (!guest.SuperGuestTitle.IsActive())
            {
                EndSuperGuestTitle(guest);
            }
        }

        private int CountLastYearReservations(List<AccommodationReservation>  reservations)
        {
            int count = 0;
            foreach (AccommodationReservation reservation in reservations)
            {
                bool isWithinLastYear = (reservation.DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) <= 0) &&
                    (reservation.DateSpan.StartDate.CompareTo(DateOnly.FromDateTime(DateTime.Now).AddYears(-1)) > 0);
                if (isWithinLastYear)
                {
                    count++;
                }
            }
            return count;
        }

        private void CreateSuperGuestTitle(User guest)
        {
            guest.SuperGuestTitle = new SuperGuestTitle(guest);
            TitleRepository.Save(guest.SuperGuestTitle);
        }

        private void EndSuperGuestTitle(User guest)
        {
            guest.SuperGuestTitle = null;
            CheckSuperGuest(guest);
        }

        public void DeductPoint(User guest)
        {
            if (guest.IsSuperGuest)
            {
                guest.SuperGuestTitle.DeductPoint();
                TitleRepository.SaveAll();
            }
        }

        public void ReturnPoint(User guest)
        {
            if (guest.IsSuperGuest)
            {
                guest.SuperGuestTitle.ReturnPoint();
                TitleRepository.SaveAll();
            }
        }
    }
}
