using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Serializer;

namespace TravelAgency.Domain.Models
{
    public class CommentDislike : ISerializable
    {
        public int Id { get; set; }
        public Comment Comment { get; set; }
        public User Owner { get; set; }

        public CommentDislike()
        {
            Comment = new Comment();
            Owner = new User();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Comment.Id.ToString(),
                Owner.Id.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Comment.Id = Convert.ToInt32(values[1]);
            Owner.Id = Convert.ToInt32(values[2]);
        }
    }
}
