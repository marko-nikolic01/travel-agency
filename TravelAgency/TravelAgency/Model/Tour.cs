using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TravelAgency.Model
{
    public class Tour : Serializer.ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string language;
        public string Language
        {
            get { return language; }
            set
            {
                if (value != language)
                {
                    language = value;
                    OnPropertyChanged();
                }
            }
        }
        public int MaxGuestNumber { get; set; }

        private string maxGuestNumberInput;
        public string MaxGuestNumberInput
        {
            get { return maxGuestNumberInput; }
            set
            {
                if (value != maxGuestNumberInput)
                {
                    maxGuestNumberInput = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Duration { get; set; }

        private string durationInput;
        public string DurationInput
        {
            get { return durationInput; }
            set
            {
                if (value != durationInput)
                {
                    durationInput = value;
                    OnPropertyChanged();
                }
            }
        }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public List<Photo> Photos { get; set; }

        //regex from Stackoverflow
        private Regex positiveNumbers = new Regex("^[1-9]+[0-9]*$");
        public string Error => throw new NotImplementedException();
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                    {
                        return "You have to enter the name!";
                    }
                }
                else if (columnName == "Description")
                {
                    if (string.IsNullOrEmpty(Description))
                    {
                        return "You have to write description!";
                    }
                }
                else if (columnName == "Language")
                {
                    if (string.IsNullOrEmpty(Language))
                    {
                        return "You have to enter the language!";
                    }
                }
                else if(columnName == "MaxGuestNumberInput")
                {
                    if (string.IsNullOrEmpty(MaxGuestNumberInput))
                    {
                        return "Enter number of guests";
                    }
                    else if(!positiveNumbers.IsMatch(MaxGuestNumberInput))
                    {
                        return "You should enter a positive integer";
                    }
                }
                else if (columnName == "DurationInput")
                {
                    if (string.IsNullOrEmpty(DurationInput))
                    {
                        return "Enter duration";
                    }
                    else if (!positiveNumbers.IsMatch(DurationInput))
                    {
                        return "You should enter a positive integer";
                    }
                }
                return null;
            }
        }

        private readonly string[] validatedProperties = { "Name", "Description", "Language", "MaxGuestNumberInput", "DurationInput" };

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

        public Tour(int id, string name, string description, string language, int maxGuestNumber, 
            int duration, int locationId, List<string> keyPoints, List<DateTime> dateTimes, List<Photo> photos)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            MaxGuestNumber = maxGuestNumber;
            Duration = duration;
            LocationId = locationId;
            Photos = photos;
        }

        public Tour()
        {
            Name = "";
            Description = "";
            Language = "";
            Location = new Location();
            Photos = new List<Photo>();
        }


        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Description, Language, MaxGuestNumber.ToString(), Duration.ToString(), 
                LocationId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            Description = values[2];
            Language = values[3];
            MaxGuestNumber = int.Parse(values[4]);
            Duration = int.Parse(values[5]);
            LocationId = int.Parse(values[6]);
        }
    }
}
