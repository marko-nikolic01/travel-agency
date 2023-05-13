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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.Domain.Models;
using TravelAgency.Services;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerManageAccommodationsPage.xaml
    /// </summary>
    public partial class OwnerManageAccommodationsPage : Page
    {
        public OwnerManageAccommodationsViewModel ViewModel { get; set; }

        public OwnerManageAccommodationsPage()
        {
            InitializeComponent();
            ViewModel = new OwnerManageAccommodationsViewModel();
            DataContext = ViewModel;
        }
    }
}
