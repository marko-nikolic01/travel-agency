using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class AccommodationRenovationRepository : IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovations.csv";

        private readonly Serializer<AccommodationRenovation> serializer;

        private List<AccommodationRenovation> renovations;

        public AccommodationRenovationRepository()
        {
            serializer = new Serializer<AccommodationRenovation>();
            renovations = serializer.FromCSV(FilePath);
        }

        public void LinkAccommodations(List<Accommodation> accommodations)
        {
            foreach (AccommodationRenovation renovation in renovations)
            {
                foreach (Accommodation accommodation in accommodations)
                {
                    if (renovation.AccommodationId == accommodation.Id)
                    {
                        renovation.Accommodation = accommodation;
                        break;
                    }
                }
            }
        }

        public List<AccommodationRenovation> GetAll()
        {
            return renovations;
        }

        public List<AccommodationRenovation> GetByAccommodation(Accommodation accommodation)
        {
            return GetByAccommodationId(accommodation.Id);
        }

        public int NextId()
        {
            if (renovations.Count < 1)
            {
                return 1;
            }

            return renovations.Max(c => c.Id) + 1;
        }

        public AccommodationRenovation Save(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();
            renovations.Add(accommodationRenovation);
            serializer.ToCSV(FilePath, renovations);
            return accommodationRenovation;
        }

        public List<AccommodationRenovation> GetByAccommodationId(int id)
        {
            List<AccommodationRenovation> filtered = new List<AccommodationRenovation>();

            foreach (AccommodationRenovation renovation in renovations)
            {
                if (renovation.AccommodationId == id)
                {
                    filtered.Add(renovation);
                }
            }

            return filtered;
        }

        public List<AccommodationRenovation> GetByOwner(User owner)
        {
            var filtered = new List<AccommodationRenovation>();

            foreach (var renovation in renovations)
            {
                if (renovation.Accommodation.OwnerId == owner.Id)
                {
                    filtered.Add(renovation);
                }
            }

            return filtered;
        }

        public void Delete(AccommodationRenovation renovation)
        {
            renovations.Remove(renovation);
            serializer.ToCSV(FilePath, renovations);
        }
    }
}
