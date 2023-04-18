﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Model;
using TravelAgency.RepositoryInterfaces;
using TravelAgency.View;

namespace TravelAgency.Services
{
    public class AccommodationService
    {
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IAccommodationPhotoRepository AccommodationPhotoRepository { get; set; }

        public AccommodationService()
        {
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationPhotoRepository = Injector.Injector.CreateInstance<IAccommodationPhotoRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationRepository.LinkPhotos(AccommodationPhotoRepository.GetAll());
        }

        public List<Accommodation> GetAccommodationsSortedBySuperOwner()
        {
            return SortBySuperOwnersFirst(AccommodationRepository.GetAll());
        }

        public List<Accommodation> GetFilteredAccommodations(AccommodationSearchFilter filter)
        {
            return SortBySuperOwnersFirst(AccommodationRepository.GetFiltered(filter));
        }

        public List<Accommodation> SortBySuperOwnersFirst(List<Accommodation> accommodations)
        {
            var a = new List<Accommodation>(accommodations);
            List<Accommodation> sortedAccommodations = new List<Accommodation>();

            foreach (var accommodation in accommodations)
            {
                if (accommodation.Owner.IsSuperOwner)
                {
                    sortedAccommodations.Add(accommodation);
                    a.Remove(accommodation);
                }
            }

            foreach (var accommodation in a)
            {
                sortedAccommodations.Add(accommodation);
            }

            return sortedAccommodations;
        }

        public void CreateNew(Accommodation newAccommodation)
        {
            AccommodationRepository.Save(newAccommodation);
            foreach (var photo in newAccommodation.Photos)
            {
                photo.ObjectId = newAccommodation.Id;
            }
            AccommodationPhotoRepository.SaveAll(newAccommodation.Photos);
        }

        public List<Accommodation> GetByOwner(User owner)
        {
            return AccommodationRepository.GetByOwner(owner);
        }
    }
}