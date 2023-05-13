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
        public SpecialTourRequestsView(int guestId)
        {
            InitializeComponent();
            DataContext = new SpecialTourRequestsViewModel(guestId);
        }

        private void NewRequest_Click(object sender, RoutedEventArgs e)
        {
            SpecialTourRequestForm tourRequestForm = new SpecialTourRequestForm(0);
            this.NavigationService.Navigate(tourRequestForm);
        }
    }
}
