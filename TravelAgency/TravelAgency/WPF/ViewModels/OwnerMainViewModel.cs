using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerMainViewModel : ViewModelBase
    {
		private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        public MyICommand<string> NavCommand { get; private set; }

        private OwnerProfileViewModel ownerProfileViewModel;
        private OwnerAccommodationsViewModel ownerAccommodationsViewModel;

        public OwnerMainViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);

            ownerProfileViewModel = new OwnerProfileViewModel();
            ownerAccommodationsViewModel = new OwnerAccommodationsViewModel();
            currentViewModel = ownerProfileViewModel;
        }

        public void OnNav(string destination)
        {
            switch (destination)
            {
                case "profile":
                    CurrentViewModel = ownerProfileViewModel;
                    break;
                case "accommodations":
                    CurrentViewModel = ownerAccommodationsViewModel;
                    break;
            }
        }
    }
}
