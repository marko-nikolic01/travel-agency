using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface ICommentRepository
    {
        public List<Comment> GetAll();
        public Comment Save(Comment comment);
        public int NextId();
        public List<Comment> GetByForum(Forum forum);
        public void LinkUsers(List<User> users);
        public void LinkForums(List<Forum> forums);
    }
}
