using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> serializer;

        private List<Forum> forums;

        public ForumRepository()
        {
            serializer = new Serializer<Forum>();
            forums = serializer.FromCSV(FilePath);
        }

        public void LinkAdmins(List<User> users)
        {
            foreach (Forum forum in forums)
            {
                foreach (User user in users)
                {
                    if (forum.Admin.Id == user.Id)
                    {
                        forum.Admin = user;
                        break;
                    }
                }
            }
        }

        public void LinkLocations(List<Location> locations)
        {
            foreach (Forum forum in forums)
            {
                foreach (Location location in locations)
                {
                    if (forum.Location.Id == location.Id)
                    {
                        forum.Location = location;
                        break;
                    }
                }
            }
        }

        public void LinkComments(List<Comment> comments)
        {
            foreach (Forum forum in forums)
            {
                foreach (Comment comment in comments)
                {
                    if ((forum.Id == comment.Forum.Id) && !forum.Comments.Contains(comment))
                    {
                        forum.Comments.Add(comment);
                    }
                }
            }
        }

        public List<Forum> GetAll()
        {
            return forums;
        }

        public List<Forum> GetByAdmin(User owner)
        {
            List<Forum> forumsByAdmin = new List<Forum>();
            foreach (Forum forum in forums)
            {
                if (forum.Admin == owner) 
                {
                    forumsByAdmin.Add(forum);
                }
            }
            return forumsByAdmin;
        }

        public int NextId()
        {
            if (forums.Count < 1)
            {
                return 1;
            }
            return forums.Max(c => c.Id) + 1;
        }

        public Forum Save(Forum forum)
        {
            forum.Id = NextId();
            forums.Add(forum);
            serializer.ToCSV(FilePath, forums);
            return forum;
        }

        public void SaveAll()
        {
            serializer.ToCSV(FilePath, forums);
        }

        public List<Forum> GetByLocation(Location location)
        {
            return forums.FindAll(f => f.Location == location);
        }
    }
}
