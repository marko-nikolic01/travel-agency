﻿using System;
using System.Collections.Generic;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequests.csv";
        private readonly Serializer<TourRequest> _serializer;
        private List<TourRequest> tourRequests;
        private List<IObserver> observers;
        public TourRequestRepository()
        {
            _serializer = new Serializer<TourRequest>();
            tourRequests = _serializer.FromCSV(FilePath);
            observers = new List<IObserver>();
        }
        public int NextId()
        {
            if (tourRequests.Count == 0)
            {
                return 1;
            }
            return tourRequests[tourRequests.Count - 1].Id + 1;
        }

        public List<TourRequest> GetAll()
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest request in tourRequests)
            {
                if (request.SpecialTourRequestId == -1)
                    result.Add(request);
            }
            return result;
        }
        public List<TourRequest> GetAllIncludingSpecial()
        {
            return tourRequests;
        }
        public List<TourRequest> GetPendings()
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest request in tourRequests)
            {
                if (request.SpecialTourRequestId == -1 && request.Status == RequestStatus.Pending)
                    result.Add(request);
            }
            return result;
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            tourRequests.Add(tourRequest);
            _serializer.ToCSV(FilePath, tourRequests);
            return tourRequest;
        }

        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

        public void UpdateRequestStatus(TourRequest request, DateTime givenDateTime, int id)
        {
            NotifyObservers();
            TourRequest oldRequest = tourRequests.Find(t => t.Id == request.Id);
            oldRequest.Status = RequestStatus.Accepted;
            oldRequest.GivenDate = givenDateTime.ToString("dd/MM/yyyy");
            oldRequest.GuideId = id;
            _serializer.ToCSV(FilePath, tourRequests);
            NotifyObservers();
        }

        public List<TourRequest> GetRequestsByGuestId(int id)
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest tourRequest in tourRequests)
            {
                if (tourRequest.GuestId == id && tourRequest.SpecialTourRequestId == -1)
                {
                    result.Add(tourRequest);
                }
            }
            return result;
        }
        public int GetCountForYear(string year)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(year))
                {
                    result++;
                }
            }
            return result;
        }

        public int GetCountForYearByLanguage(string year, string selectedLanguage)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(year) && r.Language.Equals(selectedLanguage))
                {
                    result++;
                }
            }
            return result;
        }

        public int GetCountForYearByCountry(string year, string selectedCountry)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(year) && r.Location.Country.Equals(selectedCountry))
                {
                    result++;
                }
            }
            return result;
        }

        public int GetCountForYearByCountryCity(string year, string selectedCountry, string city)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(year) && r.Location.City.Equals(city) && r.Location.Country.Equals(selectedCountry))
                {
                    result++;
                }
            }
            return result;
        }
        public List<string> GetLanguages(int guestId)
        {
            List<string> result = new List<string>();
            foreach(TourRequest request in tourRequests)
            {
                if(request.GuestId == guestId && request.SpecialTourRequestId == -1)
                    result.Add(request.Language);
            }
            return result;
        }
        public List<string> GetLocationsForGuest(int guestId)
        {
            List<string> result = new List<string>();
            foreach (TourRequest request in tourRequests)
            {
                if (request.GuestId == guestId && request.SpecialTourRequestId == -1)
                {
                    string LocationName = request.Location.Country + ", " + request.Location.City;
                    result.Add(LocationName);
                }
            }
            return result;
        }

        public int GetCountForYearByLanguageAndYear(string selectedYear, string selectedLanguage, int month)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(selectedYear) && (int.Parse(r.MinDate.ToString("MM")) == month) && r.Language.Equals(selectedLanguage))
                {
                    result++;
                }
            }
            return result;
        }

        public int GetCountForYearByLocationAndYear(string selectedYear, string selectedCountry, string selectedCity, int month)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(selectedYear) && (int.Parse(r.MinDate.ToString("MM")) == month) && r.Location.Country.Equals(selectedCountry) && r.Location.City.Equals(selectedCity))
                {
                    result++;
                }
            }
            return result;
        }

        public int GetCountForYearByCountryAndYear(string selectedYear, string selectedCountry, int month)
        {
            int result = 0;
            foreach (var r in GetAll())
            {
                if (r.MinDate.ToString("yyyy").Equals(selectedYear) && (int.Parse(r.MinDate.ToString("MM")) == month) && r.Location.Country.Equals(selectedCountry))
                {
                    result++;
                }
            }
            return result;
        }
        public List<TourRequest> GetBySpecialRequestId(int id)
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach(TourRequest request in tourRequests)
            {
                if(request.SpecialTourRequestId == id)
                    result.Add(request);
            }
            return result;
        }
        public List<TourRequest> GetSpecialRequests()
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest request in tourRequests)
            {
                if (request.SpecialTourRequestId != -1)
                    result.Add(request);
            }
            return result;
        }
        public int GetNumberOfAllRequests(int guestId)
        {
            int number = 0;
            foreach (TourRequest request in tourRequests)
                if (request.GuestId == guestId)
                    number++;
            return number;
        }
        public bool ShouldNotifyGuestForLanguage(int guestId, string language)
        {
            bool existsInNotAcceptedRequests = false;
            foreach (TourRequest request in GetRequestsByGuestId(guestId))
            {
                if (request.Language == language)
                {
                    if (request.Status != RequestStatus.Accepted)
                        existsInNotAcceptedRequests = true;
                    // ako je vec prihvacen negde takav zahtev ne treba obavestiti gosta
                    else
                        return false;
                }
            }
            return existsInNotAcceptedRequests;
        }
        public bool ShouldNotifyGuestForLocation(int guestId, int locationId)
        {
            bool existsInNotAcceptedRequests = false;
            foreach (TourRequest request in GetRequestsByGuestId(guestId))
            {
                if (request.Location.Id == locationId)
                {
                    if (request.Status != RequestStatus.Accepted)
                        existsInNotAcceptedRequests = true;
                    // ako je vec prihvacen negde takav zahtev ne treba obavestiti gosta
                    else
                        return false;
                }
            }
            return existsInNotAcceptedRequests;
        }

        public void BookTourRequest(int requestId, int guideId, DateOnly selectedDate)
        {
            TourRequest oldRequest = tourRequests.Find(t => t.Id == requestId);
            oldRequest.Status = RequestStatus.Accepted;
            oldRequest.GivenDate = selectedDate.ToString("dd/MM/yyyy");
            oldRequest.GuideId = guideId;
            _serializer.ToCSV(FilePath, tourRequests);
        }
        public void UndoBookTourRequest(int requestId)
        {
            TourRequest oldRequest = tourRequests.Find(t => t.Id == requestId);
            oldRequest.Status = RequestStatus.Pending;
            oldRequest.GivenDate = "/";
            oldRequest.GuideId = -1;
            _serializer.ToCSV(FilePath, tourRequests);
        }

        public List<TourRequest> GetAccepted(int specialId)
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest request in tourRequests)
            {
                if (request.SpecialTourRequestId == specialId && request.Status == RequestStatus.Accepted)
                    result.Add(request);
            }
            return result;
        }

        public List<TourRequest> GetAcceptedForGuide(int id)
        {
            List<TourRequest> result = new List<TourRequest>();
            foreach (TourRequest request in tourRequests)
            {
                if (request.GuideId == id && request.Status == RequestStatus.Accepted)
                    result.Add(request);
            }
            return result;
        }

        public int GetSpecialByRequestId(int bookedRequest)
        {
            foreach (TourRequest request in tourRequests)
            {
                if (request.Id == bookedRequest)
                    return request.SpecialTourRequestId;
            }
            return -1;
        }
    }
}