using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;
using TravelAgency.Repositories;

namespace TravelAgency.Services
{
    public class ForumService
    {
        public IForumRepository ForumRepository { get; set; }
        public ICommentRepository CommentRepository { get; set; }
        public ILocationRepository LocationRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IAccommodationRepository AccommodationRepository { get; set; }
        public IAccommodationReservationRepository AccommodationReservationRepository { get; set; }

        public ForumService()
        {
            ForumRepository = Injector.Injector.CreateInstance<IForumRepository>();
            CommentRepository = Injector.Injector.CreateInstance<ICommentRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationReservationRepository.LinkGuests(UserRepository.GetAll());
            AccommodationReservationRepository.LinkAccommodations(AccommodationRepository.GetAll());
            ForumRepository.LinkLocations(LocationRepository.GetAll());
            ForumRepository.LinkAdmins(UserRepository.GetAll());
            ForumRepository.LinkComments(CommentRepository.GetAll());
            CommentRepository.LinkUsers(UserRepository.GetAll());
            CommentRepository.LinkForums(ForumRepository.GetAll());
        }

        public bool OpenForum(Forum forum, Comment initialComment)
        {
            if (initialComment.Text != "")
            {
                CommentRepository.Save(initialComment);
                forum.Comments.Add(initialComment);
                ForumRepository.Save(forum);
                return true;
            }
            return false;
        }

        public void CloseForum(Forum forum)
        {
            forum.Close();
            ForumRepository.SaveAll();
        }

        public void PostComment(Forum forum, Comment comment)
        {
            if (comment.Text != "" && !forum.Closed)
            {
                comment.Forum = forum;
                forum.Comments.Add(comment);
                CommentRepository.Save(comment);
                if (DidUserVisitLocation(comment.User, forum.Location))
                {
                    comment.LocationVisited = true;
                    forum.CommentsByVisitors++;
                }

            }
        }

        public bool DidUserVisitLocation(User user, Location location)
        {
            List<AccommodationReservation> accommodationReservations = AccommodationReservationRepository.GetAllNotCanceledByGuest(user);
            foreach (AccommodationReservation reservation in accommodationReservations)
            {
                if (location == reservation.Accommodation.Location)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsUserAccommodationOwner(User user, Location location)
        {
            List<Accommodation> accommodations = AccommodationRepository.GetByOwner(user);
            if (accommodations.Count() > 0)
            {
                return true;
            }
            return false;
        }

    }
}
