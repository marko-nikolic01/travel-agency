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
using System.Windows.Shapes;
using TravelAgency.Model;

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for Guest2Main.xaml
    /// </summary>
    public partial class Guest2Main : Window
    {
        public static ObservableCollection<TuraTest> Tours { get; set; }
        List<TuraTest> toursList;
        public Guest2Main()
        {
            InitializeComponent();
            DataContext = this;

            Tours = new ObservableCollection<TuraTest>
            {
                new TuraTest("bg", 20),
                new TuraTest("ns", 20)
            };
            toursList = Tours.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool tbLocEmpty = true, tbDurEmpty = true;
            if(tbLocation.Text != "" || tbDuration.Text != "")
            {
                if(tbLocation.Text != "")
                    tbLocEmpty= false;
                if (tbDuration.Text != "")
                    tbDurEmpty = false;
                //proverava da li tekst iz textbox zadovaljava kriterijum ili ako je prazan textbox onda svakako zadovoljava kriterijum
                var filteredList = toursList.Where(x => (x.Location.ToLower().Contains(tbLocation.Text) || tbLocEmpty) &&
                                                        (x.Duration.ToString().Contains(tbDuration.Text) || tbDurEmpty));
                ToursDataGrid.ItemsSource = filteredList;
            }
            else
            {
                ToursDataGrid.ItemsSource = Tours;
            }
        }
    }
}
