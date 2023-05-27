using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    internal class CommentRepository : ICommentRepository
    {
        private const string FilePath = "../../../Resources/Data/comments.csv";

        private readonly Serializer<Comment> serializer;

        private List<Comment> comments;

        public CommentRepository()
        {
            serializer = new Serializer<Comment>();
            comments = serializer.FromCSV(FilePath);
        }

        public List<Comment> GetAll()
        {
            return comments;
        }

        public void LinkForums(List<Forum> forums)
        {
            foreach (Comment comment in comments)
            {
                foreach (Forum forum in forums)
                {
                    if (forum.Id == comment.Forum.Id)
                    {
                        comment.Forum = forum;
                        break;
                    }
                }
            }
        }

        public void LinkUsers(List<User> users)
        {
            foreach (Comment comment in comments)
            {
                foreach (User user in users)
                {
                    if (comment.User.Id == user.Id)
                    {
                        comment.User = user;
                        break;
                    }
                }
            }
        }

        public int NextId()
        {
            if(comments.Count < 1)
            {
                return 1;
            }
            return comments.Max(c => c.Id) + 1;
        }

        public Comment Save(Comment comment)
        {
            comment.Id = NextId();
            comments.Add(comment);
            serializer.ToCSV(FilePath, comments);
            return comment;
        }
    }
}
