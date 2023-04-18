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
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for VouchersView.xaml
    /// </summary>
    public partial class VouchersView : Window
    {
        VoucherViewModel voucherViewModel;
        User ActiveGuest;
        public VouchersView(User ActiveGuest)
        {
            InitializeComponent();
            voucherViewModel = new VoucherViewModel(ActiveGuest.Id);
            this.DataContext = voucherViewModel;
            this.ActiveGuest = ActiveGuest;
        }

        private void MyToursButton_Click(object sender, RoutedEventArgs e)
        {
            MyTours myTours = new MyTours(ActiveGuest.Id);
            myTours.Show();
            Close();
        }
        private void OfferedToursButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2Main offeredTours = new Guest2Main(ActiveGuest);
            offeredTours.Show();
            Close();
        }
    }
}
