using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class AccommodationManagingSuggestionsService
    {

        public IAccommodationGuestRatingRepository GuestRatingRepository { get; set; }
        public IAccommodationOwnerRatingRepository AccommodationOwnerRatingRepository { get; set; }
        public IAccommodationReservationRepository ReservationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }


        public AccommodationManagingSuggestionsService() {
        
            GuestRatingRepository = Injector.Injector.CreateInstance<IAccommodationGuestRatingRepository>();
            AccommodationOwnerRatingRepository = Injector.Injector.CreateInstance<IAccommodationOwnerRatingRepository>();
            ReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            ReservationRepository.LinkGuests(UserRepository.GetUsers());
            ReservationRepository.LinkAccommodations(AccommodationRepository.GetActive());
            GuestRatingRepository.LinkReservations(ReservationRepository.GetAll());
            AccommodationOwnerRatingRepository.LinkReservations(ReservationRepository.GetAll());
        }

        public List<AccommodationWithNumberOfDaysBusyDTO> GetTop3WorstAccommodationsByOwner(User owner)
        {
            var sortedAccommodations = GetAccommodationsSortedByNumberOfBusyDays(owner);

            int range;
            if (sortedAccommodations.Count >= 3)
            {
                range = 3;
            }
            else
            {
                range = sortedAccommodations.Count;
            }

            return sortedAccommodations.GetRange(0, range).ToList();
        }

        public List<LocationWithNumberOfBusyDaysDTO> GetTop3BestLocationsByOwner(User owner)
        {
            var sortedLocations = GetLocationsSortedByWithNumberOfBusyDays(owner);

            int range;
            if (sortedLocations.Count >= 3)
            {
                range = 3;
            }
            else
            {
                range = sortedLocations.Count;
            }

            return sortedLocations.GetRange(0, range).ToList();
        }

        public List<AccommodationWithNumberOfDaysBusyDTO> GetAccommodationsSortedByNumberOfBusyDays(User owner)
        {
            return GetAccommodationsWithNumberOfBusyDays(owner).OrderBy(l => l.NumberOfBusyDays).ToList();
        }

        public List<AccommodationWithNumberOfDaysBusyDTO> GetAccommodationsWithNumberOfBusyDays(User owner)
        {
            var dtos = new List<AccommodationWithNumberOfDaysBusyDTO>();
            foreach (var accommodation in AccommodationRepository.GetActiveByOwner(owner))
            {
                var dto = new AccommodationWithNumberOfDaysBusyDTO(accommodation, GetNumberOfBusyDaysForAccommodation(accommodation));
                dtos.Add(dto);
            }

            return dtos;
        }

        public List<LocationWithNumberOfBusyDaysDTO> GetLocationsSortedByWithNumberOfBusyDays(User owner)
        {
            return GetLocationsWithNumberOfBusyDays(owner).OrderByDescending(l => l.NumberOfBusyDays).ToList();
        }

        public List<LocationWithNumberOfBusyDaysDTO> GetLocationsWithNumberOfBusyDays(User owner)
        {
            var dtos = new List<LocationWithNumberOfBusyDaysDTO>();
            foreach (var location in GetLocationsByOwner(owner))
            {
                var dto = new LocationWithNumberOfBusyDaysDTO(location, GetNumberOfBusyDaysForLocation(location, owner));
                dtos.Add(dto);
            }

            return dtos;
        }

        private int GetNumberOfBusyDaysForLocation(Location location, User owner)
        {
            int count = 0;
            foreach (var accommodation in AccommodationRepository.GetActiveByLocationAndOwner(location, owner))
            {
                count += GetNumberOfBusyDaysForAccommodation(accommodation);
            }

            return count;
        }

        private int GetNumberOfBusyDaysForAccommodation(Accommodation accommodation)
        {
            int count = 0;
            foreach (var reservation in ReservationRepository.GetByAccommodation(accommodation))
            {
                count += CalculateNumberOfDaysForReservation(reservation);
            }
            
            return count;
        }

        private int CalculateNumberOfDaysForReservation(AccommodationReservation reservation)
        {
            return reservation.DateSpan.EndDate.DayNumber - reservation.DateSpan.StartDate.DayNumber;
        }

        private List<Location> GetLocationsByOwner(User owner)
        {
            var locations = new List<Location>();
            foreach (var accommodation in AccommodationRepository.GetActiveByOwner(owner))
            {
                if (!locations.Contains(accommodation.Location))
                {
                    locations.Add(accommodation.Location);
                }
            }

            return locations;
        }
    }
}
