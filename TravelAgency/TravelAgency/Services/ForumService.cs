﻿using Syncfusion.Windows.PdfViewer;
using Syncfusion.Windows.Shared;
using System;
using System.CodeDom;
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
        public ICommentDislikeRepository CommentDislikeRepository { get; set; }
        public INotificationRepository NotificationRepository { get; set; }

        public ForumService()
        {
            ForumRepository = Injector.Injector.CreateInstance<IForumRepository>();
            CommentRepository = Injector.Injector.CreateInstance<ICommentRepository>();
            UserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            AccommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            LocationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
            AccommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            CommentDislikeRepository = Injector.Injector.CreateInstance<ICommentDislikeRepository>();
            NotificationRepository = Injector.Injector.CreateInstance<INotificationRepository>();

            AccommodationRepository.LinkLocations(LocationRepository.GetAll());
            AccommodationRepository.LinkOwners(UserRepository.GetOwners());
            AccommodationReservationRepository.LinkGuests(UserRepository.GetAll());
            AccommodationReservationRepository.LinkAccommodations(AccommodationRepository.GetActive());
            ForumRepository.LinkLocations(LocationRepository.GetAll());
            ForumRepository.LinkAdmins(UserRepository.GetAll());
            ForumRepository.LinkComments(CommentRepository.GetAll());
            CommentRepository.LinkUsers(UserRepository.GetAll());
            CommentRepository.LinkForums(ForumRepository.GetAll());
            CommentDislikeRepository.LinkUsers(UserRepository.GetAll());
            CommentDislikeRepository.LinkComments(CommentRepository.GetAll());
            NotificationRepository.LinkUsers(UserRepository.GetAll());
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
            if (initialComment.IsValid && forum.IsValid)
            {
                ForumRepository.Save(forum);
                PostCommentByGuest(forum, initialComment);
                SendNotifications(forum);
                return true;
            }
            return false;
        }

        private void SendNotifications(Forum forum)
        {
            foreach (var owner in UserRepository.GetOwners())
            {
                if (OwnerHasAccommodationOnLocation(owner, forum.Location))
                {
                    Notification notification = new Notification();
                    notification.User = owner;
                    notification.Text = "Forum opened for location: " + forum.Location.City;
                    NotificationRepository.Save(notification);
                }
            }
        }

        private bool OwnerHasAccommodationOnLocation(User owner, Location location)
        {
            foreach (var accommodation in AccommodationRepository.GetActiveByOwner(owner))
            {
                if (accommodation.Location == location)
                {
                    return true;
                }
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
            if (comment.IsValid && !forum.Closed)
            {
                comment.Forum = forum;
                forum.Comments.Add(comment);
                if (DidUserVisitLocation(comment.User, forum.Location))
                {
                    comment.LocationVisited = true;
                    forum.CommentsByVisitors++;
                    ForumRepository.SaveAll();
                }
                CommentRepository.Save(comment);
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

        public List<ForumWithStatsDTO> GetForumsWithStatsForLocation(Location location)
        {
            var forums = new List<ForumWithStatsDTO>();

            foreach (var forum in GetForumsByLocation(location))
            {
                var dto = GetForumWithStats(forum);
                forums.Add(dto);
            }

            return forums;
        }

        private ForumWithStatsDTO GetForumWithStats(Forum forum)
        {
            int numberOfOwnerComments = GetNumberOfOwnerComments(forum);
            int numberOfGuestComments = GetNumberOfGuestComments(forum);
            bool isForumVeryUseful = IsForumVeryUserful(numberOfGuestComments, numberOfOwnerComments);
            var dto = new ForumWithStatsDTO(forum, numberOfOwnerComments, numberOfGuestComments, isForumVeryUseful);
            return dto;
        }

        private int GetNumberOfOwnerComments(Forum forum)
        {
            int count = 0;
            foreach (var comment in CommentRepository.GetByForum(forum))
            {
                if (comment.User.Role == Roles.Owner)
                {
                    count++;
                }
            }

            return count;
        }

        private int GetNumberOfGuestComments(Forum forum)
        {
            int count = 0;
            foreach (var comment in CommentRepository.GetByForum(forum))
            {
                if (comment.User.Role == Roles.Guest1)
                {
                    count++;
                }
            }

            return count;
        }

        public List<Location> GetLocationsForForumsByOwner(User owner)
        {
            LocationService locationService = new LocationService();

            var locationsForOwner = locationService.GetLocationsByOwner(owner);
            var locations = new List<Location>();

            foreach (var location in locationsForOwner)
            {
                if (LocationHasForum(location))
                {
                    locations.Add(location);
                }
            }

            return locations;
        }

        private bool LocationHasForum(Location location)
        {
            return GetNumberOfForumsForLocation(location) > 0;
        }

        private int GetNumberOfForumsForLocation(Location location)
        {
            return ForumRepository.GetByLocation(location).Count;
        }

        public List<Forum> GetForumsByLocation(Location location)
        {
            return ForumRepository.GetByLocation(location);
        }

        public List<CommentWithDataDTO> GetCommentsWithDataByForum(Forum forum, User owner)
        {
            var dtos = new List<CommentWithDataDTO>();

            foreach (var comment in CommentRepository.GetByForum(forum))
            {
                var dto = GetCommentWithDataByForum(comment, comment.User, forum.Location, owner);
                dtos.Add(dto);
            }

            return dtos;
        }

        public CommentWithDataDTO GetCommentWithDataByForum(Comment comment, User user, Location location, User owner)
        {
            int commentDislikeCount = GetCommentDislikeCount(comment);
            bool isCommentOfOwner = IsCommentOfOwner(comment);
            bool ownerDislikedComment = OwnerDislikedComment(owner, comment);
            bool guestVisited = (user.Role == Roles.Owner) ? true : DidUserVisitLocation(user, location);
            var dto = new CommentWithDataDTO(comment, commentDislikeCount, isCommentOfOwner, ownerDislikedComment, guestVisited);
            return dto;
        }

        private int GetCommentDislikeCount(Comment comment)
        {
            return CommentDislikeRepository.GetByComment(comment).Count;
        }

        public bool OwnerDislikedComment(User owner, Comment comment)
        {
            foreach (var commentDislike in CommentDislikeRepository.GetByComment(comment))
            {
                if (commentDislike.Owner == owner)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsCommentOfOwner(Comment comment)
        {
            return comment.User.Role == Roles.Owner;
        }

        public void PostCommentByOwnerToForum(string commentText, Forum forum, User user)
        {
            if (!OwnerHasAccommodationOnLocation(user, forum.Location))
            {
                return;
            }

            Comment newComment = new Comment();
            newComment.Text = commentText;
            newComment.OwnsAccommodationOnLocation = true;
            forum.CommentsByAccommodationOwners++;
            ForumRepository.SaveAll();
            forum.Comments.Add(newComment);
            PostCommentByOwnerToForum(newComment, forum, user);
        }

        public void PostCommentByOwnerToForum(Comment comment, Forum forum, User user)
        {
            comment.Forum = forum;
            comment.User = user;
            PostCommentByOwnerToForum(comment);
        }

        public void PostCommentByOwnerToForum(Comment comment)
        {
            CommentRepository.Save(comment);
        }

        public bool IsForumVeryUserful(ForumWithStatsDTO forum)
        {
            return IsForumVeryUserful(forum.NumberOfGuestComments, forum.NumberOfOwnerComments);
        }

        public bool IsForumVeryUserful(int numberOfGuestComments, int numberOfOwnerComments)
        {
            return numberOfGuestComments >= 20 || numberOfOwnerComments >= 10;
        }

        public void DislikeComment(Comment comment, User user)
        {
            CommentDislike commentDislike = new CommentDislike();
            commentDislike.Comment = comment;
            commentDislike.Owner = user;
            CommentDislikeRepository.Save(commentDislike);
        }

        public bool CanOwnerDislikeComment(User owner, Comment comment)
        {
            return !OwnerDislikedComment(owner, comment) && !DidUserVisitLocation(comment.User, comment.Forum.Location) && !IsCommentOfOwner(comment);
        }
    }
}
