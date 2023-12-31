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
    public interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();
        public List<TourRequest> GetAllIncludingSpecial();
        public List<TourRequest> GetPendings();
        public void NotifyObservers();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public TourRequest Save(TourRequest tourRequest);
        public void UpdateRequestStatus(TourRequest request, DateTime givenDateTime, int id);
        public List<TourRequest> GetRequestsByGuestId(int id);
        public int NextId();
        public int GetCountForYear(string year);
        int GetCountForYearByLanguage(string year, string selectedLanguage);
        int GetCountForYearByCountry(string year, string selectedCountry);
        int GetCountForYearByCountryCity(string year, string selectedCountry, string city);
        public List<string> GetLanguages(int guestId);
        public List<string> GetLocationsForGuest(int guestId);
        public int GetCountForYearByLanguageAndYear(string selectedYear, string selectedLanguage, int month);
        int GetCountForYearByLocationAndYear(string selectedYear, string selectedCountry, string selectedCity, int month);
        int GetCountForYearByCountryAndYear(string selectedYear, string selectedCountry, int month);
        public List<TourRequest> GetBySpecialRequestId(int id);
        public List<TourRequest> GetSpecialRequests();
        public int GetNumberOfAllRequests(int guestId);
        public bool ShouldNotifyGuestForLanguage(int guestId, string language);
        public bool ShouldNotifyGuestForLocation(int guestId, int locationId);
        public void BookTourRequest(int requestId, int guideId, DateOnly selectedDate);
        public void UndoBookTourRequest(int requestId);
        public List<TourRequest> GetAccepted(int specialId);
        public List<TourRequest> GetAcceptedForGuide(int id);
        public int GetSpecialByRequestId(int bookedRequest);
    }
}
