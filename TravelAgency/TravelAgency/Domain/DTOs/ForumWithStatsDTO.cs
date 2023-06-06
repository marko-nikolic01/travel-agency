using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class ForumWithStatsDTO
    {
        public Forum Forum { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfAccommodations { get; set; }

        public ForumWithStatsDTO() { }

        public ForumWithStatsDTO(Forum forum, int numberOfComments, int numberOfAccommodations)
        {
            Forum = forum;
            NumberOfComments = numberOfComments;
            NumberOfAccommodations = numberOfAccommodations;
        }
    }
}
