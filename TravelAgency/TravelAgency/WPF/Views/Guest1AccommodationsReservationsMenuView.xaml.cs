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
    /// Interaction logic for Guest1AccommodationsReservationsMenuView.xaml
    /// </summary>
    public partial class Guest1AccommodationsReservationsMenuView : UserControl
    {
        public Guest1AccommodationsReservationsMenuView()
        {
            InitializeComponent();
            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
