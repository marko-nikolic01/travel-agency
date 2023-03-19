using System;
using TravelAgency.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TravelAgency.Model
{
    public class Location : Serializer.ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (value != fullName)
                {
                    fullName = value;
                    OnPropertyChanged();
                }
            }
        }


        private Regex cityCountryFullName = new Regex("[A-Za-z][A-Za-z ]+,[A-Za-z][A-Za-z ]+");
        public string Error => throw new NotImplementedException();
        public string this[string columnName]
        {
            get
            {
                if (columnName == "FullName")
                {
                    if (string.IsNullOrEmpty(FullName))
                    {
                        return "You have to enter location!";
                    }
                    Match m = cityCountryFullName.Match(FullName);
                    if (!m.Success)
                    {
                        return "Format should be CITY,COUNTRY";
                    }
                }
                return null;
            }
        }

        private readonly string[] validatedProperties = { "FullName" };

        public bool IsValid
        {
            get
            {
                foreach (var property in validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Location(int id, string city, string country, string fullName)
        {
            Id = id;
            City = city;
            Country = country;
            FullName = fullName;
        }

        public Location()
        {
            Country = "";
            City = "";
            FullName = "";
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), City, Country };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
        }
    }
}
