using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class AccommodationPhoto : ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int ObjectId { get; set; }

        public AccommodationPhoto()
        {
            Id = -1;
            Path = string.Empty;
            ObjectId = -1;
        }

        public AccommodationPhoto(int id, string path, int objectId)
        {
            Id = id;
            Path = path;
            ObjectId = objectId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Path,
                ObjectId.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Path = values[1];
            ObjectId = int.Parse(values[2]);
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
