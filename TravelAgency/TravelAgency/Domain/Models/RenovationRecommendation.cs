using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Repositories;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public enum UrgencyLevel { LEVEL1, LEVEL2, LEVEL3, LEVEL4, LEVEL5 }

    public class RenovationRecommendation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public int RatingId { get; set; }

        private string _description;
        private UrgencyLevel _urgencyLevel;

        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public UrgencyLevel UrgencyLevel
        {
            get => _urgencyLevel;
            set
            {
                if (value != _urgencyLevel)
                {
                    _urgencyLevel = value;
                    OnPropertyChanged();
                }
            }
        }

        public RenovationRecommendation()
        {
            Id = -1;
            RatingId = -1;
            Description = "";
            UrgencyLevel = UrgencyLevel.LEVEL1;
        }

        public RenovationRecommendation(AccommodationOwnerRating rating)
        {
            Id = -1;
            RatingId = rating.Id;
            Description = "";
            UrgencyLevel = UrgencyLevel.LEVEL1;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                RatingId.ToString(),
                Description,
                Convert.ToInt32(UrgencyLevel).ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            RatingId = Convert.ToInt32(values[1]);
            Description = values[2];
            UrgencyLevel = (UrgencyLevel)Convert.ToInt32(values[3]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Description")
                {
                    if (Description == "")
                    {
                        return "You must describe the state of the accommodation";
                    }
                }
                else if (columnName == "UrgencyLevel")
                {
                    if (UrgencyLevel == null)
                    {
                        return "You must select the renovation urgency level";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Description", "UrgencyLevel"};

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }
    }
}
