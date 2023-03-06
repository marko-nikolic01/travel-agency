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
using System.Windows.Threading;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for Guest1Main.xaml
    /// </summary>
    public partial class Guest1Main : Window
    {
        public Guest1Main()
        {
            InitializeComponent();
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.9);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.9);
        }

        

        private void LoadDateTime(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.statusBarDateTime.Content = DateTime.Now.ToString("dd/mm/yyyy     hh:mm:ss");
            }, this.Dispatcher);
            timer.Start();
        }


    }
}
