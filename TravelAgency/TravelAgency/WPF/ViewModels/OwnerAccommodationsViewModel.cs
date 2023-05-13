using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.WPF.Commands;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAccommodationsViewModel : ViewModelBase
    {
        private OwnerManageAccommodationsViewModel ownerManageAccommodationsViewModel;
        public MyICommand<string> NavCommand { get; set; }

        public OwnerAccommodationsViewModel(MyICommand<string> navCommand)
        {
            NavCommand = navCommand;
        }
    }
}
