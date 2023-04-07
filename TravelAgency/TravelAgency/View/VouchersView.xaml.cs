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
using TravelAgency.ViewModel;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for VouchersView.xaml
    /// </summary>
    public partial class VouchersView : Window
    {
        VoucherViewModel voucherViewModel;
        public VouchersView(int guestId)
        {
            InitializeComponent();
            voucherViewModel = new VoucherViewModel(guestId);
            this.DataContext = voucherViewModel;
        }
    }
}
