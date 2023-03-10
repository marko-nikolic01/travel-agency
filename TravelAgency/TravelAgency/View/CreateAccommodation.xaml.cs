using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodation : Window
    {
        public CreateAccommodation()
        {
            InitializeComponent();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
        }
    }
}
