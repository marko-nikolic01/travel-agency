using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using TravelAgency.Commands;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class CreatedRequestsStatisticsViewModel : INotifyPropertyChanged
    {
        private string selectedYear;
        private string yearAcceptance;
        private string yearAveragePeopleNum;
        private string languageChartVisibility;
        private string locationChartVisibility;
        private bool languageSelected;
        private bool locationSelected;
        private bool graphHelpClicked;
        public bool GraphHelpClicked
        {
            get { return graphHelpClicked; }
            set { graphHelpClicked = value; OnPropertyChanged(); }
        }
        public string LanguageChartVisibility
        {
            get { return languageChartVisibility; }
            set { languageChartVisibility = value; OnPropertyChanged(); }
        }
        public string LocationChartVisibility
        {
            get { return locationChartVisibility; }
            set { locationChartVisibility = value; OnPropertyChanged(); }
        }
        public bool LanguageSelected
        {
            get { return languageSelected; }
            set { languageSelected = value; OnPropertyChanged(); }
        }
        public bool LocationSelected
        {
            get { return locationSelected; }
            set { locationSelected = value; OnPropertyChanged(); }
        }
        public string YearAcceptance
        {
            get { return yearAcceptance; }
            set { yearAcceptance = value; OnPropertyChanged(); }
        }
        public string YearAveragePeopleNum
        {
            get { return yearAveragePeopleNum; }
            set { yearAveragePeopleNum = value; OnPropertyChanged(); }
        }
        public string AllTimeAcceptance { get; set; }
        public string AllTimeAveragePeopleNum { get; set; }
        public ObservableCollection<string> Years { get; set; }
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                YearAcceptance = requestService.GetAcceptedTourPercentage(currentGuestId, int.Parse(value)) + "%";
                YearAveragePeopleNum = requestService.GetAveragePeopleNumber(currentGuestId, int.Parse(value));
                selectedYear = value;
                OnPropertyChanged();
            }
        }
        TourRequestService requestService;
        private int currentGuestId;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] BarLabels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection SeriesCollectionLocation { get; set; }
        public string[] BarLabelsLocation { get; set; }
        public Func<double, string> FormatterLocation { get; set; }
        public ButtonCommandNoParameter GraphHelpCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CreatedRequestsStatisticsViewModel(int guestId) 
        {
            requestService = new TourRequestService();
            currentGuestId = guestId;
            LanguageSelected = false;
            LocationSelected = true;
            ChangeChart();
            SetAllTimeAcceptance();
            SetAllTimeAveragePeopleNum();
            FillOptions();
            FillChartColumns();
            GraphHelpCommand = new ButtonCommandNoParameter(GraphClick);
            UpdateHelpText();
        }
        private void GraphClick()
        {
            GraphHelpClicked = !GraphHelpClicked;
        }
        private void SetAllTimeAcceptance()
        {
            AllTimeAcceptance = requestService.GetAcceptedTourPercentage(currentGuestId) + "%";
        }
        private void SetAllTimeAveragePeopleNum()
        {
            AllTimeAveragePeopleNum = requestService.GetAveragePeopleNumber(currentGuestId);
        }
        private void FillOptions()
        {
            Years = new ObservableCollection<string>() {"2023", "2022", "2021", "2020", "2019" };
            SelectedYear = Years[0];
        }
        public void ChangeChart()
        {
            if (LanguageSelected)
            {
                LanguageChartVisibility = "Visible";
                LocationChartVisibility = "Hidden";
            }
            else
            {
                LocationChartVisibility = "Visible";
                LanguageChartVisibility = "Hidden";
            }
        }
        private void FillChartColumns()
        {
            FillForLanguages();
            FillForLocations();
        }
        private void FillForLanguages()
        {
            List<string> languages = requestService.GetLanguages(currentGuestId);
            var groupedLanguages = languages.GroupBy(x => x)
                                   .Select(g => new { Value = g.Key, Count = g.Count() });
            SeriesCollection = new SeriesCollection();
            foreach (var language in groupedLanguages) 
            {
                SeriesCollection.Add(new ColumnSeries
                {
                    Title=language.Value,
                    Values = new ChartValues<double> { language.Count }
                });
            }
            BarLabels = new[] { "Languages" };
            Formatter = value => null;
        }
        private void FillForLocations()
        {
            List<string> countries = requestService.GetCountriesForGuest(currentGuestId);
            var groupedCountries = countries.GroupBy(x => x)
                                   .Select(g => new { Value = g.Key, Count = g.Count() });
            SeriesCollectionLocation = new SeriesCollection();
            foreach (var country in groupedCountries)
            {
                SeriesCollectionLocation.Add(new ColumnSeries
                {
                    Title = country.Value,
                    Values = new ChartValues<double> { country.Count }
                });
            }
            BarLabelsLocation = new[] { "Locations" };
            FormatterLocation = value => null;
        }
        private void UpdateHelpText()
        {
            string file = @"../../../Resources/HelpTexts/StatisticsHelp.txt";
            Guest2MainViewModel.HelpText = File.ReadAllText(file);
        }
    }
}
