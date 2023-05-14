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

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerRatingsPage.xaml
    /// </summary>
    public partial class OwnerRatingsPage : Page
    {
        public OwnerRatingsViewModel ViewModel { get; set; }
        public OwnerRatingsPage()
        {
            InitializeComponent();
            ViewModel = new OwnerRatingsViewModel();
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
