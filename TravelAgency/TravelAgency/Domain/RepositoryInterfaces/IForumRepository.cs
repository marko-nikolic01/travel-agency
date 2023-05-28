using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;

namespace TravelAgency.Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public List<Forum> GetAll();
        public List<Forum> GetByAdmin(User owner);
        public Forum Save(Forum forum);
        public void SaveAll();
        public int NextId();
        public void LinkAdmins(List<User> users);
        public void LinkLocations(List<Location> locations);
        public void LinkComments(List<Comment> comments);
    }
}
