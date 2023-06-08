
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.Domain.Models;
using TravelAgency.Domain.RepositoryInterfaces;

namespace TravelAgency.Services
{
    public class UserService
    {
        public IUserRepository IUserRepository { get; set; }
        public ITourOccurrenceRepository ITourOccurrenceRepository { get; set; }
        public ITourRepository ITourRepository { get; set; }
        public ITourRatingRepository ITourRatingRepository { get; set; }

        public UserService()
        {
            IUserRepository = Injector.Injector.CreateInstance<IUserRepository>();
            ITourOccurrenceRepository = Injector.Injector.CreateInstance<ITourOccurrenceRepository>();
            ITourRatingRepository = Injector.Injector.CreateInstance<ITourRatingRepository>();
            ITourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            LinkTourOccurrences();
            CheckSuperGuideStatus();
        }

        private void CheckSuperGuideStatus()
        {
            foreach(var user in IUserRepository.GetAll())
            {
                if(user.Role == Roles.Guide)
                {
                    if (user.IsSuperGuide)
                    {
                        if (!((GetFinishedToursByLanguageForGuide(user.Id, user.Language) >= 20) && (GetGuidesAverageGradeByLanguageForLastYear(user.Id, user.Language) > 4)))
                        {
                            InvalidateSuperGuideStatus(user);
                        }
                    }
                    foreach(var language in GetUniqueLanguagesForGuide(user.Id))
                    {
                        if((GetFinishedToursByLanguageForGuide(user.Id, language) >= 20) && (GetGuidesAverageGradeByLanguageForLastYear(user.Id, language) > 4))
                        {
                            SetSuperGuideStatus(user, language);
                            return;
                        }
                    }
                }
            }
        }
        private void InvalidateSuperGuideStatus(User guide)
        {
            IUserRepository.UpdateSuperGuideStatus(guide.Id, false, "");
        }
        private void SetSuperGuideStatus(User guide, string language)
        {
            IUserRepository.UpdateSuperGuideStatus(guide.Id, true, language);
        }
        public List<string> GetUniqueLanguagesForGuide(int id)
        {
            HashSet<string> uniqueLanguages = new HashSet<string>();
            foreach (var t in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                uniqueLanguages.Add(t.Tour.Language);
            }
            return uniqueLanguages.ToList<string>();
        }
        public double GetGuidesAverageGradeByLanguageForLastYear(int id, string language)
        {
            double sum = 0.0;
            int ratingsCount = 0;
            DateTime lastYearsDateTime = DateTime.Now.AddYears(-1);
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                if ((tourOccurrence.Tour.Language.Equals(language)) && (tourOccurrence.DateTime >= lastYearsDateTime))
                {
                    foreach (var tourRating in ITourRatingRepository.GetRatingsByTourOccurrenceId(tourOccurrence.Id))
                    {
                        sum += tourRating.GuideLanguage;
                        sum += tourRating.GuideKnowledge;
                        sum += tourRating.Interesting;
                        ratingsCount++;
                    }
                }
            }
            sum /= 3.0;
            sum /= (double)ratingsCount;
            return sum;
        }
        public int GetFinishedToursByLanguageForGuide(int id, string language)
        {
            int count = 0;
            foreach (var tourOccurrence in ITourOccurrenceRepository.GetFinishedOccurrencesForGuide(id))
            {
                if (tourOccurrence.Tour.Language.Equals(language))
                {
                    count++;
                }
            }
            return count;
        }
        private void LinkTourOccurrences()
        {
            foreach (TourOccurrence tourOccurrence in ITourOccurrenceRepository.GetAll())
            {
                Tour tour = ITourRepository.GetAll().Find(t => t.Id == tourOccurrence.TourId);
                if (tour != null)
                {
                    tourOccurrence.Tour = tour;
                }
            }
        }
        public User GetById(int id)
        {
            return IUserRepository.GetById(id);
        }

        public List<User> GetAllUsers()
        {
            return IUserRepository.GetUsers();
        }

        public void SaveUser(User user)
        {
            IUserRepository.SaveUser(user);
        }

        public void LogInUser(User user)
        {
            IUserRepository.LogInUser(user);
        }

        public User GetLoggedInUser()
        {
            return IUserRepository.GetLoggedInUser();
        }
        public void UpdateNewUsername(int userId, string newUsername)
        {
            IUserRepository.UpdateNewUsername(userId, newUsername);
        }
        public void UpdateNewPassword(int userId, string newPassword)
        {
            IUserRepository.UpdateNewPassword(userId, newPassword);
        }
        public bool CheckPassword(int userId, string Password)
        {
            return IUserRepository.CheckPassword(userId, Password);
        }
        public void DeleteUser(int id)
        {
            IUserRepository.DeleteUser(id);
        }
    }
}
