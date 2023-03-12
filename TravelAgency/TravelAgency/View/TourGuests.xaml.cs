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

namespace TravelAgency.View
{
    /// <summary>
    /// Interaction logic for TourGuests.xaml
    /// </summary>
    public partial class TourGuests : Window
    {
        private int numberOfGuests;
        public TourGuests(int guests)
        {
            numberOfGuests = guests;
            InitializeComponent();
        }

        private void AddGuestClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GuestUsernameText.Text))
            {
                return;
            }
            GuestList.Items.Add(GuestUsernameText.Text);
            GuestUsernameText.Clear();
            GuestUsernameText.Focus();
            isListBoxFull();
        }
        private void isListBoxFull()
        {
            if(GuestList.Items.Count == numberOfGuests)
            {
                AddButton.IsEnabled = false;
            }
            else
            {
                AddButton.IsEnabled = true;
            }
        }
        private void RemoveGuestClick(object sender, RoutedEventArgs e)
        {
            if(GuestList.SelectedItem !=null)
            {
                GuestList.Items.RemoveAt(GuestList.SelectedIndex);
            }
            isListBoxFull();
        }
    }
}
