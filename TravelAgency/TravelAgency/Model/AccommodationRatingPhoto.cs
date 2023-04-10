using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationRatingPhoto: ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int RatingId { get; set; }

        public AccommodationRatingPhoto()
        {
            Id = -1;
            Path = string.Empty;
            RatingId = -1;
        }

        public AccommodationRatingPhoto(int id, string path, int ratingId)
        {
            Id = id;
            Path = path;
            RatingId = ratingId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Path,
                RatingId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Path = values[1];
            RatingId = int.Parse(values[2]);
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Path")
                {
                    if (Path == "")
                    {
                        return "Path cannot be empty";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Path" };

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
