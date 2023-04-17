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
    public class AccommodationOwnerRating : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public int AccommodationReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        private int accommodationCleanliness;
        private int accommodationComfort;
        private int accommodationLocation;
        private int ownerCorrectness;
        private int ownerResponsiveness;
        private string comment;
        public List<AccommodationRatingPhoto> Photos { get; set; }

        public int AccommodationCleanliness
        {
            get => accommodationCleanliness;
            set
            {
                if (accommodationCleanliness != value)
                {
                    accommodationCleanliness = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int AccommodationComfort
        {
            get => accommodationComfort;
            set
            {
                if (accommodationComfort != value)
                {
                    accommodationComfort = value;
                    OnPropertyChanged();
                }
            }
        }

        public int AccommodationLocation
        {
            get => accommodationLocation;
            set
            {
                if (accommodationLocation != value)
                {
                    accommodationLocation = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int OwnerCorrectness
        {
            get => ownerCorrectness;
            set
            {
                if (ownerCorrectness != value)
                {
                    ownerCorrectness = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int OwnerResponsiveness
        {
            get => ownerResponsiveness;
            set
            {
                if (ownerResponsiveness != value)
                {
                    ownerResponsiveness = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public AccommodationOwnerRating()
        {
            Id = -1;
            AccommodationReservationId = -1;
            AccommodationCleanliness = -1;
            AccommodationComfort = -1;
            AccommodationLocation = -1;
            OwnerCorrectness = -1;
            OwnerResponsiveness = -1;
            Comment = "";
            Photos = new List<AccommodationRatingPhoto>();
        }

        public AccommodationOwnerRating(AccommodationReservation reservation)
        {
            Id = -1;
            AccommodationReservationId = reservation.Id;
            AccommodationReservation = reservation;
            AccommodationCleanliness = -1;
            AccommodationComfort = -1;
            AccommodationLocation = -1;
            OwnerCorrectness = -1;
            OwnerResponsiveness = -1;
            Comment = "";
            Photos = new List<AccommodationRatingPhoto>();
        }

        public AccommodationOwnerRating(int id, int accommodationReservationId, int accommodationCleanliness, int accommodationComfort, int accommodationLocation, int ownerCorrectness, int ownerResponsiveness, string comment)
        {
            Id = id;
            AccommodationReservationId = accommodationReservationId;
            AccommodationCleanliness = accommodationCleanliness;
            AccommodationComfort = accommodationComfort;
            AccommodationLocation = accommodationLocation;
            OwnerCorrectness = ownerCorrectness;
            OwnerResponsiveness = ownerResponsiveness;
            Comment = comment;
            Photos = new List<AccommodationRatingPhoto>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservationId.ToString(),
                AccommodationCleanliness.ToString(),
                AccommodationComfort.ToString(),
                AccommodationLocation.ToString(),
                OwnerCorrectness.ToString(),
                OwnerResponsiveness.ToString(),
                Comment
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationReservationId = Convert.ToInt32(values[1]);
            AccommodationCleanliness = Convert.ToInt32(values[2]);
            AccommodationComfort = Convert.ToInt32(values[3]);
            AccommodationLocation = Convert.ToInt32(values[4]);
            OwnerCorrectness = Convert.ToInt32(values[5]);
            OwnerResponsiveness = Convert.ToInt32(values[6]);
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
                if (columnName == "AccommodationCleanliness")
                {
                    if (AccommodationCleanliness < 1 || AccommodationCleanliness > 5)
                    {
                        return "Rating for accommodation cleanliness must be between 1 and 5";
                    }
                }
                else if (columnName == "AccommodationComfort")
                {
                    if (AccommodationComfort < 1 || AccommodationComfort > 5)
                    {
                        return "Rating for accommodation comfort must be between 1 and 5";
                    }
                }
                else if (columnName == "AccommodationLocation")
                {
                    if (AccommodationLocation < 1 || AccommodationLocation > 5)
                    {
                        return "Rating for accommodation location must be between 1 and 5";
                    }
                }
                else if (columnName == "OwnerCorrectness")
                {
                    if (OwnerCorrectness < 1 || OwnerCorrectness > 5)
                    {
                        return "Rating for owner correctness must be between 1 and 5";
                    }
                }
                else if (columnName == "OwnerResponsiveness")
                {
                    if (OwnerResponsiveness < 1 || OwnerResponsiveness > 5)
                    {
                        return "Rating for owner responsiveness must be between 1 and 5";
                    }
                }

                return null;
            }
        }

        private readonly string[] _validatedProperties = { "AccommodationCleanliness", "AccommodationComfort", "AccommodationLocation", "OwnerCorrectness", "OwnerResponsiveness" };

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