using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Models;
using TravelAgency.Services;

namespace TravelAgency.WPF.ViewModels
{
    public class GuideProfileViewModel
    {
        public UserService UserService { get; set; }
        public User Guide { get; set; }
        public GuideProfileViewModel()
        {
            UserService = new UserService();
            Guide = UserService.GetLoggedInUser();
        }
    }
}
