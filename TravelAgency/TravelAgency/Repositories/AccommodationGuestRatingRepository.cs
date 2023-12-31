﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class AccommodationGuestRatingRepository : IAccommodationGuestRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationGuestRatings.csv";
        private readonly Serializer<AccommodationGuestRating> serializer;
        private List<AccommodationGuestRating> accommodationGuestRatings;

        public AccommodationGuestRatingRepository()
        {
            serializer = new Serializer<AccommodationGuestRating>();
            accommodationGuestRatings = serializer.FromCSV(FilePath);
        }

        public void LinkReservations(List<AccommodationReservation> reservations)
        {
            foreach (var accommodationGuestRating in accommodationGuestRatings)
            {
                foreach (var accommodationReservation in reservations)
                {
                    if (accommodationGuestRating.AccommodationReservation.Id == accommodationReservation.Id)
                    {
                        accommodationGuestRating.AccommodationReservation = accommodationReservation;
                    }
                }
            }
        }

        public List<AccommodationGuestRating> GetAll()
        {
            return accommodationGuestRatings;
        }

        public int NextId()
        {
            if (accommodationGuestRatings.Count < 1)
            {
                return 1;
            }
            return accommodationGuestRatings.Max(c => c.Id) + 1;
        }

        public AccommodationGuestRating Save(AccommodationGuestRating entity)
        {
            entity.Id = NextId();
            accommodationGuestRatings.Add(entity);
            serializer.ToCSV(FilePath, accommodationGuestRatings);
            return entity;
        }

        public List<AccommodationGuestRating> GetByOwner(User owner)
        {
            return accommodationGuestRatings.FindAll(agr => agr.AccommodationReservation.Accommodation.Owner.Id == owner.Id);
        }

        public int GetRatingsCountByOwner(User owner)
        {
            return GetByOwner(owner).Count;
        }
    }
}
