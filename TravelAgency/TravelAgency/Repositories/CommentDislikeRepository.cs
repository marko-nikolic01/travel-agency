using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Serializer;

namespace TravelAgency.Repositories
{
    public class CommentDislikeRepository : ICommentDislikeRepository
    {
        private const string FilePath = "../../../Resources/Data/commentDislikes.csv";

        private readonly Serializer<CommentDislike> serializer;

        private List<CommentDislike> commentDislikes;

        public CommentDislikeRepository()
        {
            serializer = new Serializer<CommentDislike>();
            commentDislikes = serializer.FromCSV(FilePath);
        }

        public CommentDislike Delete(CommentDislike commentDislike)
        {
            commentDislikes.Remove(commentDislike);
            serializer.ToCSV(FilePath, commentDislikes);
            return commentDislike;
        }

        public List<CommentDislike> GetAll()
        {
            return commentDislikes;
        }

        public List<CommentDislike> GetByComment(Comment comment)
        {
            return commentDislikes.FindAll(cd => cd.Comment == comment);
        }

        public void LinkComments(List<Comment> comments)
        {
            foreach (Comment comment in comments)
            {
                foreach (var commentDislike in commentDislikes)
                {
                    if (commentDislike.Comment.Id == comment.Id)
                    {
                        commentDislike.Comment = comment;
                    }
                }
            }
        }

        public void LinkUsers(List<User> users)
        {
            foreach (User owner in users)
            {
                foreach (var commentDislike in commentDislikes)
                {
                    if (commentDislike.Owner.Id == owner.Id)
                    {
                        commentDislike.Owner = owner;
                    }
                }
            }
        }

        public int NextId()
        {
            if (commentDislikes.Count < 1)
            {
                return 1;
            }
            return commentDislikes.Max(c => c.Id) + 1;
        }

        public CommentDislike Save(CommentDislike commentDislike)
        {
            commentDislike.Id = NextId();
            commentDislikes.Add(commentDislike);
            serializer.ToCSV(FilePath, commentDislikes);
            return commentDislike;
        }
    }
}
