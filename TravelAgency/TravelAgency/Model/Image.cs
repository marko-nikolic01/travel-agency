using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Serializer;

namespace TravelAgency.Model
{
    public class Image : ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int ObjectId { get; set; }

        public Image()
        {
            Id = -1;
            Path = string.Empty;
            ObjectId = -1;
        }

        public Image(int id, string path, int objectId)
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
    }
}
