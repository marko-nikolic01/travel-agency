using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Observer;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class TourRequestService
    {
        public ILocationRepository ILocationRepository { get; set; }
        public ITourRequestRepository ITourRequestRepository { get; set; }
        public IRequestAcceptedNotificationRepository IRequestAcceptedNotificationRepository { get; set; }
        
        public TourRequestService()
        {
            ILocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            ITourRequestRepository = Injector.Injector.CreateInstance<ITourRequestRepository>();
            IRequestAcceptedNotificationRepository = Injector.Injector.CreateInstance<IRequestAcceptedNotificationRepository>();
            LinkRequestLocation();
            CheckIfRequestsAreInvalid();
        }
        public List<RequestAcceptedNotification> GetNewAcceptedRequests(int guestId)
        {
            return IRequestAcceptedNotificationRepository.GetNewAcceptedRequests(guestId);
        }
        public bool NewAcceptedRequestExists(int guestId)
        {
            return IRequestAcceptedNotificationRepository.NewAcceptedRequestExists(guestId);
        }
        private void CheckIfRequestsAreInvalid()
        {
            int currentDays = DateOnly.FromDateTime(DateTime.Now).DayNumber;
            foreach (var request in ITourRequestRepository.GetAll())
            {
                if (request.MinDate.DayNumber - currentDays < 3)
                {
                    request.Status = RequestStatus.Invalid;
                }
            }
        }

        private void LinkRequestLocation()
        {
            foreach (var request in ITourRequestRepository.GetAll())
            {
                Location location = ILocationRepository.GetAll().Find(l => l.Id == request.LocationId);
                if (location != null)
                {
                    request.Location = location;
                }
            }
        }
        public List<TourRequest> GetPendings()
        {
            List<TourRequest> pendings = new List<TourRequest>();
            foreach (var request in ITourRequestRepository.GetAll())
            {
                if(request.Status == RequestStatus.Pending)
                {
                    pendings.Add(request);
                }
            }
            return pendings;
        }
        public List<string> getCountries()
        {
            return ILocationRepository.GetAllCountries();
        }
        public List<string> getCities(string country)
        {
            return ILocationRepository.GetCitiesByCountry(country);
        }
        public List<TourRequest> GetRequestsByGuestId(int id)
        {
            return ITourRequestRepository.GetRequestsByGuestId(id);
        }
        public bool SaveRequest(string selectedCountry, string selectedCity, string language, string numberOfGuests, DateOnly minDate, DateOnly maxDate, string description, int guestId)
        {
            Location location = ILocationRepository.GetLocationForCountryAndCity(selectedCountry, selectedCity);
            TourRequest request = new TourRequest();
            if (request.Valid(language, numberOfGuests, minDate, maxDate))
            {
                request.Location = location;
                request.LocationId = location.Id;
                request.Description = description;
                request.GuestId = guestId;
                request.Status = RequestStatus.Pending;
                ITourRequestRepository.Save(request);
                return true;
            }
            return false;
        }

        public void Subscribe(IObserver observer)
        {
            ITourRequestRepository.Subscribe(observer);
        }
        public void UpdateRequestStatus(TourRequest request)
        {
            ITourRequestRepository.UpdateRequestStatus(request);
        }
        public void SaveNotification(RequestAcceptedNotification requestAcceptedNotification)
        {
            IRequestAcceptedNotificationRepository.Save(requestAcceptedNotification);
        }
        public void UpdateNotification(RequestAcceptedNotification requestAcceptedNotification)
        {
            IRequestAcceptedNotificationRepository.Update(requestAcceptedNotification);
        }

        public ObservableCollection<KeyValuePair<string, int>> GetYearStatistics(List<string> years)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            foreach (var year in years)
            {
                if(year == years[0])
                {
                    continue;
                }
                result.Add(new KeyValuePair<string, int>(year, ITourRequestRepository.GetCountForYear(year)));
            }
            return result;
        }

        public ObservableCollection<KeyValuePair<string, int>>? GetYearLanguageStatistics(List<string> years, string selectedLanguage)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            foreach (var year in years)
            {
                if (year == years[0])
                {
                    continue;
                }
                result.Add(new KeyValuePair<string, int>(year, ITourRequestRepository.GetCountForYearByLanguage(year, selectedLanguage)));
            }
            return result;
        }
        public List<string> GetUniqueLanguages()
        {
            HashSet<string> uniqueLanguages = new HashSet<string>();
            foreach (var r in ITourRequestRepository.GetAll())
            {
                uniqueLanguages.Add(r.Language);
            }
            return uniqueLanguages.ToList<string>();
        }

        public List<string> GetUniqueCountries()
        {
            HashSet<string> uniqueCountries = new HashSet<string>();
            foreach (var r in ITourRequestRepository.GetAll())
            {
                uniqueCountries.Add(r.Location.Country);
            }
            return uniqueCountries.ToList<string>();
        }

        public ObservableCollection<KeyValuePair<string, int>> GetYearCountryStatistics(List<string> years, string selectedCountry)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            foreach (var year in years)
            {
                if (year == years[0])
                {
                    continue;
                }
                result.Add(new KeyValuePair<string, int>(year, ITourRequestRepository.GetCountForYearByCountry(year, selectedCountry)));
            }
            return result;
        }

        public List<string> GetUniqueCitiesByCountry(string country)
        {
            HashSet<string> uniqueCities = new HashSet<string>();
            foreach (var r in ITourRequestRepository.GetAll())
            {
                if (r.Location.Country.Equals(country))
                {
                uniqueCities.Add(r.Location.City);
                }
            }
            return uniqueCities.ToList<string>();
        }

        public ObservableCollection<KeyValuePair<string, int>> GetYearCityCountryStatistics(List<string> years, string selectedCountry, string city)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            foreach (var year in years)
            {
                if (year == years[0])
                {
                    continue;
                }
                result.Add(new KeyValuePair<string, int>(year, ITourRequestRepository.GetCountForYearByCountryCity(year, selectedCountry, city)));
            }
            return result;
        }

        public string GetAcceptedTourPercentage(int guestId, int year = 0)
        {
            double accepted = 0;
            double count = 0;
            foreach(TourRequest request in ITourRequestRepository.GetAll())
            {                                   //ako je year 0 onda ce se gledati all time statistika
                if(request.GuestId == guestId && (year == 0 || request.MinDate.Year == year)) 
                {
                    count++;
                    if (request.Status == RequestStatus.Accepted)
                        accepted++;
                }
            }
            if (count == 0)
                return "0";
            return ConvertToString((accepted / count) * 100);
        }

        public string GetAveragePeopleNumber(int guestId, int year = 0)
        {
            double sum = 0;
            double count = 0;
            foreach (TourRequest request in ITourRequestRepository.GetAll())
            {
                if (request.GuestId == guestId && request.Status == RequestStatus.Accepted && (year == 0 || request.MinDate.Year == year))
                {
                    count++;
                    sum += request.GuestNumber;
                }
            }
            if (count == 0)
                return "0";
            return ConvertToString(sum / count);
        }
        private string ConvertToString(double number)
        {
           return String.Format("{0:0.00}", number);
        }
        public List<string> GetLanguages(int guestId)
        {
           return ITourRequestRepository.GetLanguages(guestId);
        }
        public List<string> GetCountriesForGuest(int guestId)
        {
            return ITourRequestRepository.GetCountriesForGuest(guestId);
        }

        public string GetMostRequestedLanguage()
        {
            string mostVisited = "";
            int maxCount = 0;
            foreach (var r1 in ITourRequestRepository.GetAll())
            {
                int count = 0;
                foreach(var r2 in ITourRequestRepository.GetAll())
                {
                    if(r1.Language == r2.Language)
                    {
                        count++;
                    }
                }
                if(count > maxCount)
                {
                    maxCount = count;
                    mostVisited = r1.Language;
                }
            }
            return mostVisited;
        }
        public Location GetMostRequestedLocation()
        {
            Location mostVisited = new Location();
            int maxCount = 0;
            foreach (var r1 in ITourRequestRepository.GetAll())
            {
                int count = 0;
                foreach (var r2 in ITourRequestRepository.GetAll())
                {
                    if (r1.LocationId == r2.LocationId)
                    {
                        count++;
                    }
                }
                if (count > maxCount)
                {
                    maxCount = count;
                    mostVisited = r1.Location;
                }
            }
            return mostVisited;
        }

        public ObservableCollection<KeyValuePair<string, int>> GetMonthsLanguageStatistics(string selectedLanguage, string selectedYear)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            if (selectedYear.Equals("YEARS") || selectedYear==null)
            {
                return result;
            }
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for ( int i = 0; i < months.Length; i++)
            {
                result.Add(new KeyValuePair<string, int>(months[i], ITourRequestRepository.GetCountForYearByLanguageAndYear(selectedYear, selectedLanguage, i+1)));
            }
            return result;
        }

        public ObservableCollection<KeyValuePair<string, int>> GetMonthsLocationStatistics(string selectedCountry, string selectedCity, string selectedYear)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            if (selectedCountry == null || selectedCity == null)
            {
                return result;
            } 
            else if(selectedCountry.Equals("< select a country >"))
            {
                return result;
            }
            else if(selectedCity.Equals("< select a city >"))
            {
                return getMonthsCountryStatistics( selectedCountry,  selectedYear);
            }
            else
            {
                string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                for (int i = 0; i < months.Length; i++)
                {
                    result.Add(new KeyValuePair<string, int>(months[i], ITourRequestRepository.GetCountForYearByLocationAndYear(selectedYear, selectedCountry, selectedCity, i + 1)));
                }
                return result;
            }
        }

        private ObservableCollection<KeyValuePair<string, int>> getMonthsCountryStatistics(string selectedCountry, string selectedYear)
        {
            var result = new ObservableCollection<KeyValuePair<string, int>>();
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int i = 0; i < months.Length; i++)
            {
                result.Add(new KeyValuePair<string, int>(months[i], ITourRequestRepository.GetCountForYearByCountryAndYear(selectedYear, selectedCountry, i + 1)));
            }
            return result;
        }
    }
}
