using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAddAccommodationViewModel : ViewModelBase
    {
        private LocationService locationService;

        public Accommodation NewAccommodation { get; set; }
        private string selectedCountry;
        public string SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                UpdateLocations();
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }
        private string selectedCity;

        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                OnPropertyChanged(nameof(SelectedCity));
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        private List<string> countries;
        public List<string> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged(nameof(Countries));
            }
        }
        private List<string> cities;
        public List<string> Cities
        {
            get { return cities; }
            set
            {
                cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }


        public OwnerAddAccommodationViewModel()
        {
            NewAccommodation = new Accommodation();

            locationService = new LocationService();

            IsEnabled = false;

            Countries = locationService.GetCountries();
            Countries.Insert(0, "<Select country>");
            selectedCountry = Countries[0];

            Cities = new List<string>
            {
                "<Select city>"
            };
            SelectedCity = Cities[0];
        }

        private void UpdateLocations()
        {
            if (SelectedCountry != "<Select country>")
            {
                var tempList = new List<string>(locationService.GetCitiesByCountry(SelectedCountry));
                tempList.Insert(0, "<Select city>");
                Cities = tempList;
                SelectedCity = Cities[0];
                IsEnabled = true;
            }
            else
            {
                IsEnabled = false;
                Cities = new List<string>
                {
                    "<Select city>"
                };
                SelectedCity = Cities[0];
            }
        }
    }
}
