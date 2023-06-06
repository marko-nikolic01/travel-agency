using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ICommentDislikeRepository
    {
        public List<CommentDislike> GetAll();
        public int NextId();
        public List<CommentDislike> GetByComment(Comment comment);
        public CommentDislike Save(CommentDislike commentDislike);
        public CommentDislike Delete(CommentDislike commentDislike);
        public void LinkUsers(List<User> users);
        public void LinkComments(List<Comment> comments);
    }
}
