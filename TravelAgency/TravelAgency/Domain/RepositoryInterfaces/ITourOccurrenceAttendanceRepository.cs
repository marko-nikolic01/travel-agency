﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ITourOccurrenceAttendanceRepository: ISubject
    {
        public List<TourOccurrenceAttendance> GetAll();
        public TourOccurrenceAttendance Save(TourOccurrenceAttendance tourOccurrenceAttendance);

        public void UpdateTourOccurrenceAttendaces(TourOccurrenceAttendance tourOccurrenceAttendance);

        public List<TourOccurrenceAttendance> GetByTourOccurrenceId(int id);
        public List<TourOccurrenceAttendance> GetByGuestId(int id);
        public List<int> GetGuestsByTourOccurrenceId(int id);

        public TourOccurrenceAttendance GetByTourOccurrenceIdAndGuestId(int TourOccurrenceId, int GuestId);

        public void SaveOrUpdate(TourOccurrenceAttendance attendance);

        public int GetCountForTour(int id);
    }
}
