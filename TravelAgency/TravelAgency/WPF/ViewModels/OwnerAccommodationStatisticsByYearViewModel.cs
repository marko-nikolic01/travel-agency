using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.DTOs;
using TravelAgency.Domain.Models;

namespace TravelAgency.WPF.ViewModels
{
    public class OwnerAccommodationStatisticsByYearViewModel : ViewModelBase
    {
        public int Year { get; set; }
        private Accommodation accommodation;

        public string PageHeaderText { get; set; }

        public AccommodationStatisticsByYearDTO SelectedStats { get; set; }

        public OwnerAccommodationStatisticsByYearViewModel(AccommodationStatisticsByYearDTO selectedStats)
        {
            SelectedStats = selectedStats;

            accommodation = SelectedStats.Accommodation;
            Year = selectedStats.Year;

            PageHeaderText = "Accommodations > Statistics > " + accommodation.Name + ", Year " + Year.ToString();
        }
    }
}
