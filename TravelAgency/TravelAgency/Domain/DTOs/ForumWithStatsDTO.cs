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
        public int NumberOfOwnerComments { get; set; }
        public int NumberOfGuestComments { get; set; }
        public bool IsVeryUserful { get; set; }

        public ForumWithStatsDTO() { }

        public ForumWithStatsDTO(Forum forum, int numberOfOwnerComments, int numberOfGuestComments, bool isVeryUserful)
        {
            Forum = forum;
            NumberOfOwnerComments = numberOfOwnerComments;
            NumberOfGuestComments = numberOfGuestComments;
            IsVeryUserful = isVeryUserful;
        }
    }
}
