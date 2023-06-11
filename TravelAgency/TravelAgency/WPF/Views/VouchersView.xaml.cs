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
using TravelAgency.Domain.Models;
using TravelAgency.Repositories;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for VouchersView.xaml
    /// </summary>
    public partial class VouchersView : Page
    {
        public VouchersView(int guestId)
        {
            InitializeComponent();
            this.DataContext = new VoucherViewModel(guestId);
        }
    }
}
