using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.Model;
using TravelAgency.Repository;
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourReviews.xaml
    /// </summary>
    public partial class TourRatingsView : Window
    {
        
        public TourRatingsView(User activeGuide)
        {
            InitializeComponent();
            DataContext = new TourRatingsViewModel(activeGuide.Id);
        }
    }
}
