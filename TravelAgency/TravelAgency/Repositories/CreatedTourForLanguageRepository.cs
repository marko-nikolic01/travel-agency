using System;
using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class NewTourNotificationRepository : INewTourNotificationRepository
    {
        private const string FilePath = "../../../Resources/Data/newtournotifications.csv";
        private readonly Serializer<NewTourNotification> _serializer;
        private List<NewTourNotification> tours;

        public NewTourNotificationRepository()
        {
            _serializer = new Serializer<NewTourNotification>();
            tours = _serializer.FromCSV(FilePath);
        }

        public List<NewTourNotification> GetAll()
        {
            return tours;
        }

        public NewTourNotification Save(NewTourNotification tour)
        {
            tours.Add(tour);
            _serializer.ToCSV(FilePath, tours);
            return tour;
        }
    }
}
