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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for SpecialTourRequestsView.xaml
    /// </summary>
    public partial class SpecialTourRequestsView : Page
    {
        int id;
        public SpecialTourRequestsView(int guestId, bool requestMade = false)
        {
            id = guestId;
            InitializeComponent();
            DataContext = new SpecialTourRequestsViewModel(guestId);
            if (requestMade)
                MessageBox.Show("The request was made successfully.");
        }

        private void NewRequest_Click(object sender, RoutedEventArgs e)
        {
            SpecialTourRequestForm tourRequestForm = new SpecialTourRequestForm(id);
            this.NavigationService.Navigate(tourRequestForm);
        }
    }
}
