using Syncfusion.Windows.PdfViewer;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TravelAgency.Domain.DTOs;
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
            AccommodationReservationRepository.LinkAccommodations(AccommodationRepository.GetActive());
            ForumRepository.LinkLocations(LocationRepository.GetAll());
            ForumRepository.LinkAdmins(UserRepository.GetAll());
            ForumRepository.LinkComments(CommentRepository.GetAll());
            CommentRepository.LinkUsers(UserRepository.GetAll());
            CommentRepository.LinkForums(ForumRepository.GetAll());
        }

        public List<Forum> GetForums()
        {
            return ForumRepository.GetAll();
        }

        public List<Forum> GetForumsByAdmin(User admin) 
        {
            return ForumRepository.GetByAdmin(admin);
        }

        public List<Forum> GetForumsByCountryAndCity(string country, string city)
        {
            List<Forum> forums = new List<Forum>();
            foreach (Forum forum in ForumRepository.GetAll())
            {
                if (forum.Location.Country == country && forum.Location.City == city)
                {
                    forums.Add(forum);
                }
            }

            return forums;
        }

        public List<Forum> GetForumsByCountry(string country)
        {
            List<Forum> forums = new List<Forum>();
            foreach (Forum forum in ForumRepository.GetAll())
            {
                if (forum.Location.Country == country)
                {
                    forums.Add(forum);
                }
            }
            return forums;
        }

        public List<Forum> GetForumsByCity(string city)
        {
            List<Forum> forums = new List<Forum>();
            foreach (Forum forum in ForumRepository.GetAll())
            {
                if (forum.Location.City == city)
                {
                    forums.Add(forum);
                }
            }
            return forums;
        }

        public bool OpenForum(Forum forum, Comment initialComment)
        {
            if (initialComment.Text != "")
            {
                ForumRepository.Save(forum);
                PostCommentByGuest(forum, initialComment);
                return true;
            }
            return false;
        }

        public void CloseForum(Forum forum)
        {
            forum.Close();
            ForumRepository.SaveAll();
        }

        public void PostCommentByGuest(Forum forum, Comment comment)
        {
            if (comment.Text != "" && !forum.Closed)
            {
                comment.Forum = forum;
                forum.Comments.Add(comment);
                CountImportantComments(forum, comment); 
                CommentRepository.Save(comment);
                ForumRepository.SaveAll();
            }
        }

        public void CountImportantComments(Forum forum, Comment comment)
        {
            if (DidUserVisitLocation(comment.User, forum.Location))
            {
                comment.LocationVisited = true;
                forum.CommentsByVisitors++;
            }
            if (IsUserAccommodationOwner(comment.User, forum.Location))
            {
                comment.OwnsAccommodationOnLocation = true;
                forum.CommentsByAccommodationOwners++;
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
            List<Accommodation> accommodations = AccommodationRepository.GetActiveByOwner(user);
            foreach (Accommodation accommodation in accommodations)
            {
                if (location == accommodation.Location)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ForumWithStatsDTO> GetForumsWithStatsByOwner(User owner)
        {
            var forums = GetForumsByOwner(owner);
            var dtos = new List<ForumWithStatsDTO>();

            foreach (var forum in forums)
            {
                ForumWithStatsDTO dto = new ForumWithStatsDTO(forum, GetNumberOfCommentsOnForum(forum), GetNumberOfAccommodationsOnLocationForOwner(owner, forum.Location));
                dtos.Add(dto);
            }

            return dtos;
        }

        /*public List<Location> GetLocationsForForumsByOwner(User owner)
        {

        }*/

        private bool LocationHasForum(Location location)
        {
            return GetNumberOfForumsForLocation(location) > 0;
        }

        private int GetNumberOfForumsForLocation(Location location)
        {
            return ForumRepository.GetByLocation(location).Count;
        }

        public List<Forum> GetForumsByOwner(User owner)
        {
            var forums = new List<Forum>();
            foreach (var forum in ForumRepository.GetAll())
            {
                if (OwnerHasAccommodationOnLocation(owner, forum.Location))
                {
                    forums.Add(forum);
                }
            }

            return forums;
        }

        public List<Comment> GetCommentsByForum(Forum forum)
        {
            return CommentRepository.GetByForum(forum);
        }

        private bool OwnerHasAccommodationOnLocation(User owner, Location location)
        {
            return GetNumberOfAccommodationsOnLocationForOwner(owner, location) > 0;
        }

        private int GetNumberOfAccommodationsOnLocationForOwner(User owner, Location location)
        {
            return AccommodationRepository.GetActiveByLocationAndOwner(location, owner).Count;
        }

        private int GetNumberOfCommentsOnForum(Forum forum)
        {
            return GetCommentsByForum(forum).Count;
        }
    }
}
