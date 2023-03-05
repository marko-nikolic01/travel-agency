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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for CreateTour.xaml
    /// </summary>
    public partial class CreateTour : Window
    {
        public CreateTour()
        {
            InitializeComponent();
        }

        private void AddKeyPointClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(KeyPointsText.Text))
            {
                return;
            }
            ListKeyPoints.Items.Add(KeyPointsText.Text);
            KeyPointsText.Clear();
            KeyPointsText.Focus();
        }

        private void AddDateTimeClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DateTimeText.Text))
            {
                return;
            }
            ListDateTimes.Items.Add(DateTimeText.Text);
            DateTimeText.Clear();
            DateTimeText.Focus();
        }

        private void AddImagesClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ImageText.Text))
            {
                return;
            }
            ListImages.Items.Add(ImageText.Text);
            ImageText.Clear();
            ImageText.Focus();
        }
    }
}
