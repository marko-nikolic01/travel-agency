using System;
using System.Collections.Generic;
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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for FinishedTourDetailedView.xaml
    /// </summary>
    public partial class FinishedTourDetailedView : Window
    {
        private TourOccurrence tourOccurrence;
        public FinishedTourDetailedView(TourOccurrence occurrence)
        {
            InitializeComponent();
            tourOccurrence = occurrence;
            nameLabel.Content = tourOccurrence.Tour.Name;
            UserRepository userRepository = new UserRepository();
            User user = userRepository.GetById(tourOccurrence.GuideId);
            guideLabel.Content = user.Username;
            descripitonLabel.Content = tourOccurrence.Tour.Description;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
