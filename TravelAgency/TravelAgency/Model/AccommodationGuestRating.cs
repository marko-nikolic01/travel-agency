using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationGuestRating : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        private int _cleanliness;
        public int Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (_cleanliness != value)
                {
                    _cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _ruleCompliance;
        public int RuleCompliance
        {
            get => _ruleCompliance;
            set
            {
                if (_ruleCompliance != value)
                {
                    _ruleCompliance = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        public AccommodationGuestRating()
        {
            Id = -1;
            AccommodationReservationId = -1;
            RuleCompliance = 1;
            Cleanliness = 1;
            Comment = "";
        }

        public AccommodationGuestRating(int id, int ownerId, int accommodationReservationId, int cleanliness, int ruleCompliance, string comment)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            Cleanliness = cleanliness;
            RuleCompliance = ruleCompliance;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                Cleanliness.ToString(),
                RuleCompliance.ToString(),
                Comment
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationReservationId = Convert.ToInt32(values[1]);
            Cleanliness = Convert.ToInt32(values[2]);
            RuleCompliance = Convert.ToInt32(values[3]);
            Comment = values[4];
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
