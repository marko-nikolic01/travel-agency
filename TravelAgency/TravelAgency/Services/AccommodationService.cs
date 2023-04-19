using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.WPF.Views;

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

        public List<Accommodation> GetAccommodations()
        {
            return AccommodationRepository.GetAll();
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
