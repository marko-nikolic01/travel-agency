using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.DTOs
{
    public class CommentWithDataDTO
    {
        public Comment Comment { get; set; }
        public int DislikesCount { get; set; }
        public bool IsOwner { get; set; }
        public bool Disliked { get; set; }
        public bool GuestVisited { get; set; }

        public CommentWithDataDTO()
        {
            
        }

        public CommentWithDataDTO(Comment comment, int dislikesCount, bool isOwner, bool disliked, bool guestVisited)
        {
            Comment = comment;
            DislikesCount = dislikesCount;
            IsOwner = isOwner;
            Disliked = disliked;
            GuestVisited = guestVisited;
        }
    }
}
