using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using TravelAgency.Model;
using TravelAgency.Serializer;

namespace TravelAgency.Repository
{
    public class AccommodationRepository : IRepository<Accommodation>
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> serializer;

        private List<Accommodation> accommodations;

        public AccommodationRepository(UserRepository userRepository, LocationRepository locationRepository, AccommodationPhotoRepository imageRepository)
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);

            foreach (Accommodation accommodation in accommodations)
            {
                foreach (User user in userRepository.GetUsers())
                {
                    if (accommodation.OwnerId == user.Id)
                    {
                        accommodation.Owner = user;
                        break;
                    }
                }
                foreach (Location location in locationRepository.GetAll())
                {
                    if (accommodation.LocationId == location.Id)
                    {
                        accommodation.Location = location;
                        break;
                    }
                }

                foreach (AccommodationPhoto image in imageRepository.GetAll())
                {
                    if (accommodation.Id == image.ObjectId)
                    {
                        accommodation.Photos.Add(image);
                    }
                }
            }
        }

        public int NextId()
        {
            if (accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public List<Accommodation> GetByUser(User user)
        {
            return accommodations.FindAll(c => c.OwnerId == user.Id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

        List<Accommodation> IRepository<Accommodation>.GetAll()
        {
            throw new NotImplementedException();
        }

        public Accommodation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations.Add(accommodation);
            serializer.ToCSV(FilePath, accommodations);
            return accommodation;
        }

        public void SaveAll(IEnumerable<Accommodation> entities)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Accommodation entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        public List<Accommodation> Search(AccommodationSearchFilter filter)
        {
            List<Accommodation> searchedAccommodations = GetAll();
            searchedAccommodations = SerachByName(filter.NameFilter, searchedAccommodations);
            searchedAccommodations = SerachByCountry(filter.CountryFilter, searchedAccommodations);
            searchedAccommodations = SerachByCity(filter.CityFilter, searchedAccommodations);
            searchedAccommodations = SerachByType(filter.TypeFilter, searchedAccommodations);
            searchedAccommodations = SerachByGuestNumber(filter.GuestNumberFilter, searchedAccommodations);
            searchedAccommodations = SerachByDayNumber(filter.DayNumberFilter, searchedAccommodations);

            searchedAccommodations = SortBySuperOwnersFirst(searchedAccommodations);

            return searchedAccommodations;
        }

        public List<Accommodation> SerachByName(string nameFilter, List<Accommodation> accommodations)
        {
            if (nameFilter != "")
            {
                return accommodations.Where(accommodation => accommodation.Name.ToLower().Trim().Contains(nameFilter.ToLower().Trim())).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> SerachByCountry(string countryFilter, List<Accommodation> accommodations)
        {
            if (countryFilter != "Not specified")
            {
                return accommodations.Where(accommodation => accommodation.Location.Country.ToLower().Contains(countryFilter.ToLower())).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> SerachByCity(string cityFilter, List<Accommodation> accommodations)
        {
            if (cityFilter != "Not specified")
            {
                return accommodations.Where(accommodation => accommodation.Location.City.ToLower().Contains(cityFilter.ToLower())).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> SerachByType(string typeFilter, List<Accommodation> accommodations)
        {
            switch (typeFilter)
            {
                case "Appartment":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.APARTMENT).ToList();
                case "House":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.HOUSE).ToList();
                case "Hut":
                    return accommodations.Where(accommodation => accommodation.Type == AccommodationType.HUT).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> SerachByGuestNumber(int guestNumberFilter, List<Accommodation> accommodations)
        {
            if (guestNumberFilter > 0)
            {
                return accommodations.Where(accommodation => guestNumberFilter <= accommodation.MaxGuests).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> SerachByDayNumber(int DayFilter, List<Accommodation> accommodations)
        {
            if (DayFilter > 0)
            {
                return accommodations.Where(accommodation => DayFilter >= accommodation.MinDays).ToList();
            }
            return accommodations;
        }

        public List<Accommodation> GetAllSortedBySuperOwnersFirst()
        {
            return SortBySuperOwnersFirst(accommodations);
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
    }
}
