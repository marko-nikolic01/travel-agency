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
    public class AccommodationGuestRating : ISerializable, INotifyPropertyChanged, IDataErrorInfo
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
        private int _compliance;
        public int Compliance
        {
            get => _compliance;
            set
            {
                if (_compliance != value)
                {
                    _compliance = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _noisiness;
        public int Noisiness
        {
            get => _noisiness;
            set
            {
                if (_noisiness != value)
                {
                    _noisiness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _friendliness;
        public int Friendliness
        {
            get => _friendliness;
            set
            {
                if (_friendliness != value)
                {
                    _friendliness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _responsivenes;
        public int Responsivenes
        {
            get => _responsivenes;
            set
            {
                if (_responsivenes != value)
                {
                    _responsivenes = value;
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
        public AccommodationReservation? AccommodationReservation { get; set; }

        public AccommodationGuestRating()
        {
            Id = -1;
            AccommodationReservationId = -1;
            Compliance = 1;
            Cleanliness = 1;
            Noisiness = 1;
            Friendliness = 1;
            Responsivenes = 1;
            Comment = "";
        }

        public AccommodationGuestRating(int id, int accommodationReservationId, int cleanliness, int compliance, int noisiness, int friendliness, int responsivenes, string comment)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            Cleanliness = cleanliness;
            Compliance = compliance;
            Noisiness = noisiness;
            Friendliness = friendliness;
            Responsivenes = responsivenes;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                Cleanliness.ToString(),
                Compliance.ToString(),
                Noisiness.ToString(),
                Friendliness.ToString(),
                Responsivenes.ToString(),
                Comment
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationReservationId = Convert.ToInt32(values[1]);
            Cleanliness = Convert.ToInt32(values[2]);
            Compliance = Convert.ToInt32(values[3]);
            Noisiness = Convert.ToInt32(values[4]);
            Friendliness = Convert.ToInt32(values[5]);
            Responsivenes = Convert.ToInt32(values[6]);
            Comment = values[7];
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
                if (columnName == "Cleanliness")
                {
                    if (Cleanliness < 1 || Cleanliness > 5)
                    {
                        return "Ocena za čistoću mora imati vrednost od 1 do 5";
                    }
                }
                else if (columnName == "Compliance")
                {
                    if (Compliance < 1 || Compliance > 5)
                    {
                        return "Ocena za poštovanje pravila mora imati vrednost od 1 do 5";
                    }
                }
                else if (columnName == "Noisiness")
                {
                    if (Noisiness < 1 || Noisiness > 5)
                    {
                        return "Ocena za poštovanje pravila mora imati vrednost od 1 do 5";
                    }
                }
                else if (columnName == "Friendliness")
                {
                    if (Friendliness < 1 || Friendliness > 5)
                    {
                        return "Ocena za ljubaznost mora imati vrednost od 1 do 5";
                    }
                }
                else if (columnName == "Responsivenes")
                {
                    if (Responsivenes < 1 || Responsivenes > 5)
                    {
                        return "Ocena za odzivnost mora imati vrednost od 1 do 5";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Cleanliness", "Compliance", "Noisiness", "Friendliness", "Responsivenes" };

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
