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
using TravelAgency.Domain.Models;
using TravelAgency.WPF.ViewModels;
using Xceed.Wpf.Toolkit;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationSearchView.xaml
    /// </summary>
    public partial class AccommodationSearchView : Page
    {
        private Guest1HomeView _mainWindow;
        public AccommodationSearchViewModel ViewModel { get; set; }
        public AccommodationSearchView(Guest1HomeView guest1HomeView)
        {
            InitializeComponent();
            _mainWindow = guest1HomeView;

            ViewModel = new AccommodationSearchViewModel();
            this.DataContext = ViewModel;
            integerUpDownGuestNumber.Value = 0;
            integerUpDownDayNumber.Value = 0;
        }

        private void ComboBoxLocation_LostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (sender as ComboBox);
            if (comboBox.Text != "")
            {
                comboBox.Text = comboBox.SelectedItem.ToString();
            }
            else
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateLocationsData(true);
            comboBoxCity.ItemsSource = ViewModel.Cities;
            comboBoxCity.SelectedItem = 0;
            comboBoxCity.Text = ViewModel.Cities[0];
        }

        private void ComboBoxCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.UpdateLocationsData(false);
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var upDown = (sender as IntegerUpDown);
            if (upDown.Value == 0)
            {
                upDown.Text = "";
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Search();
        }

        private void ButtonCancelSearch_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CancelSearch();
            textBoxName.Text = "";
            comboBoxType.SelectedIndex = 0;
            integerUpDownGuestNumber.Value = 0;
            integerUpDownDayNumber.Value = 0;
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private void TextBlockAccommodationsReservations_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            this.NavigationService.Navigate(new AccommodationsReservationsMenuView(_mainWindow));
        }
    }
}
