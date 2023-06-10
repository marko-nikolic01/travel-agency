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

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for Guest1WhereverWheneverSearchView.xaml
    /// </summary>
    public partial class Guest1WhereverWheneverSearchView : UserControl
    {
        public Guest1WhereverWheneverSearchView()
        {
            InitializeComponent();
            Loaded += (s, e) => Keyboard.Focus(this);
        }

        private void ButtonMakeReservation_Click(object sender, RoutedEventArgs e)
        {
            listViewAccommodations.SelectedItem = ((FrameworkElement)sender).DataContext;
        }
    }
}
