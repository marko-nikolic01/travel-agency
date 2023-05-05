using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TravelAgency.Commands;
using TravelAgency.Services;
using TravelAgency.WPF.Views;
using Xceed.Wpf.Toolkit.Primitives;

namespace TravelAgency.WPF.ViewModels
{
    public class TourRequestStatisticsViewModel : INotifyPropertyChanged
    {
        public List<string> Countries { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Years { get; set; }
        public LocationService LocationService { get; set; }
        public TourRequestService TourRequestService { get; set; }

        private bool isCountrySelected;
        public bool IsCountrySelected
        {
            get { return isCountrySelected; }
            set 
            { 
                isCountrySelected = value;
                OnPropertyChanged();   
            }
        }

        private ObservableCollection<string> cities;
        public ObservableCollection<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged();
            }
        }
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if(YearsStatistic != null )
                {
                    if (!("< select a city >").Equals(value))
                    {
                        YearsStatistic = TourRequestService.GetYearCityCountryStatistics(Years, SelectedCountry, value);
                        MonthsStatisticByYear = TourRequestService.GetMonthsLocationStatistics(SelectedCountry, value, SelectedYear);
                    }
                }
                selectedCity = value;
                OnPropertyChanged();
            }
        }
        private string selectedCountry;
        public string SelectedCountry
        {
            get { return selectedCountry; }
            set 
            {
                if(value.Equals(Countries[0]))
                {
                    Cities.Clear();
                    Cities.Add("< select a city >");
                    IsCountrySelected=false;
                    YearsStatistic?.Clear();
                    MonthsStatisticByYear?.Clear();
                }
                else
                {
                    Cities.Clear();
                    Cities.Add("< select a city >");
                    foreach(var city in TourRequestService.GetUniqueCitiesByCountry(value))
                    {
                        Cities.Add(city);
                    }
                    IsCountrySelected = true;
                    MonthsStatisticByYear = TourRequestService.GetMonthsLocationStatistics(value, SelectedCity, SelectedYear);
                    YearsStatistic = TourRequestService.GetYearCountryStatistics(Years, value);
                }
                SelectedCity = Cities[0];
                selectedCountry = value; 
                OnPropertyChanged();
            }
        }
        private string selectedLanguage;
        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                MonthsStatisticByYear = TourRequestService.GetMonthsLanguageStatistics(value, selectedYear);
                YearsStatistic = TourRequestService.GetYearLanguageStatistics(Years, value);
                selectedLanguage = value;
                OnPropertyChanged();
            }
        }
        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (isLanguageChecked)
                {
                    MonthsStatisticByYear = TourRequestService.GetMonthsLanguageStatistics(SelectedLanguage, value);
                }
                else
                {
                    MonthsStatisticByYear = TourRequestService.GetMonthsLocationStatistics(SelectedCountry, SelectedCity, value);
                }
                selectedYear = value;
                OnPropertyChanged();
            }
        }
        private bool isLanguageChecked;
        public bool IsLanguageChecked
        {
            get { return isLanguageChecked; }
            set
            {
                if(value == true)
                {
                    MonthsStatisticByYear = TourRequestService.GetMonthsLanguageStatistics(SelectedLanguage, SelectedYear);
                    YearsStatistic = TourRequestService.GetYearLanguageStatistics(Years, SelectedLanguage);
                }
                isLanguageChecked = value;
                OnPropertyChanged();
            }
        }
        private bool isLocationChecked;
        public bool IsLocationChecked
        {
            get { return isLocationChecked; }
            set
            {
                if (value == true && !SelectedCountry.Equals(Countries[0]) && !SelectedCity.Equals(Cities[0]))
                {
                    YearsStatistic = TourRequestService.GetYearCityCountryStatistics(Years, SelectedCountry, SelectedCity);
                    MonthsStatisticByYear = TourRequestService.GetMonthsLocationStatistics(SelectedCountry, SelectedCity, SelectedYear);
                }
                else if(value == true && !SelectedCountry.Equals(Countries[0]))
                {
                    YearsStatistic = TourRequestService.GetYearCountryStatistics(Years, SelectedCountry);
                    MonthsStatisticByYear = TourRequestService.GetMonthsLocationStatistics(SelectedCountry, SelectedCity, SelectedYear);
                }
                else if(value == true)
                {
                    MonthsStatisticByYear.Clear();
                    YearsStatistic.Clear();
                }
                isLocationChecked = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<KeyValuePair<string, int>> yearsStatistic;
        public ObservableCollection<KeyValuePair<string, int>> YearsStatistic
        {
            get { return yearsStatistic; }
            set 
            {
                yearsStatistic = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<KeyValuePair<string, int>> monthsStatisticByYear;
        public ObservableCollection<KeyValuePair<string, int>> MonthsStatisticByYear
        {
            get { return monthsStatisticByYear; }
            set
            {
                monthsStatisticByYear = value;
                OnPropertyChanged();
            }
        }
        public ButtonCommandNoParameter CreateTourForLocationCommand { get; set; }
        public ButtonCommandNoParameter CreateTourForLanguageCommand { get; set; }
        public NavigationService NavigationService { get; set; }
        public int ActiveGuideId { get; set; }

        public TourRequestStatisticsViewModel(System.Windows.Navigation.NavigationService navService, int activeGuideId)
        {
            NavigationService = navService;
            ActiveGuideId = activeGuideId;
            Cities = new ObservableCollection<string>() { "< select a city >" };
            SelectedCity = Cities[0];
            Countries = new List<string>() { "< select a country >" };
            SelectedCountry = Countries[0];
            LocationService = new LocationService();
            TourRequestService = new TourRequestService();
            Countries.AddRange(TourRequestService.GetUniqueCountries());
            Years = new List<string>() { "YEARS", "2023", "2022", "2021", "2020", "2019"};
            SelectedYear = Years[0];
            Languages = new List<string>(TourRequestService.GetUniqueLanguages());
            if(Languages.Count > 0)
            {
                SelectedLanguage = Languages[0];
            }
            YearsStatistic = TourRequestService.GetYearLanguageStatistics(Years, SelectedLanguage);
            MonthsStatisticByYear = TourRequestService.GetMonthsLanguageStatistics(SelectedLanguage, SelectedYear);
            IsLocationChecked = false;
            IsLanguageChecked = true;
            CreateTourForLocationCommand = new ButtonCommandNoParameter(CreateTourForLocation);
            CreateTourForLanguageCommand = new ButtonCommandNoParameter(CreateTourForLanguage);
        }
        public void CreateTourForLocation()
        {
            Page createTourPage = new CreateTourForm(ActiveGuideId, NavigationService, location : TourRequestService.GetMostRequestedLocation()) ;
            NavigationService.Navigate(createTourPage);
        }
        public void CreateTourForLanguage()
        {
            Page createTourPage = new CreateTourForm(ActiveGuideId, NavigationService, TourRequestService.GetMostRequestedLanguage());
            NavigationService.Navigate(createTourPage);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
