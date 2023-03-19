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
        private int cleanliness;
        public int Cleanliness
        {
            get => cleanliness;
            set
            {
                if (cleanliness != value)
                {
                    cleanliness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int compliance;
        public int Compliance
        {
            get => compliance;
            set
            {
                if (compliance != value)
                {
                    compliance = value;
                    OnPropertyChanged();
                }
            }
        }
        private int noisiness;
        public int Noisiness
        {
            get => noisiness;
            set
            {
                if (noisiness != value)
                {
                    noisiness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int friendliness;
        public int Friendliness
        {
            get => friendliness;
            set
            {
                if (friendliness != value)
                {
                    friendliness = value;
                    OnPropertyChanged();
                }
            }
        }
        private int responsivenes;
        public int Responsivenes
        {
            get => responsivenes;
            set
            {
                if (responsivenes != value)
                {
                    responsivenes = value;
                    OnPropertyChanged();
                }
            }
        }
        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (value != comment)
                {
                    comment = value;
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
                        return "Rating for cleanliness must be between 1 and 5";
                    }
                }
                else if (columnName == "Compliance")
                {
                    if (Compliance < 1 || Compliance > 5)
                    {
                        return "Rating for rule compliance must be between 1 and 5";
                    }
                }
                else if (columnName == "Noisiness")
                {
                    if (Noisiness < 1 || Noisiness > 5)
                    {
                        return "Rating for noisiness must be between 1 and 5";
                    }
                }
                else if (columnName == "Friendliness")
                {
                    if (Friendliness < 1 || Friendliness > 5)
                    {
                        return "Rating for friendliness must be between 1 and 5";
                    }
                }
                else if (columnName == "Responsivenes")
                {
                    if (Responsivenes < 1 || Responsivenes > 5)
                    {
                        return "Rating for responsivenes must be between 1 and 5";
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
