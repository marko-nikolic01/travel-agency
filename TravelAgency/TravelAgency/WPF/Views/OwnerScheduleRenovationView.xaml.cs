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
    /// Interaction logic for OwnerScheduleRenovationView.xaml
    /// </summary>
    public partial class OwnerScheduleRenovationView : Page
    {
        public OwnerScheduleRenovationViewModel ViewModel { get; set; }

        public OwnerScheduleRenovationView(OwnerScheduleRenovationViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
            accommodationComboBox.Loaded += (object sender, RoutedEventArgs e) => accommodationComboBox.Focus();
        }

        private void AddRenovation_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ScheduleRenovationCmd.Execute();
        }
    }
}
