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
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerAddAccommodationPage.xaml
    /// </summary>
    public partial class OwnerAddAccommodationView : Page
    {
        public OwnerAddAccommodationViewModel ViewModel { get; set; }

        public OwnerAddAccommodationView(OwnerAddAccommodationViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);

            nameTextBox.Focus();
        }

        public OwnerAddAccommodationView(OwnerAddAccommodationViewModel viewModel, string preselectedCountry, string preselectedCity) : this(viewModel)
        {
            Loaded += (s, e) => this.PreselectCountryAndCity(preselectedCountry, preselectedCity);
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NavigateBack.Execute();
        }

        private void AddPhoto_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddPhotoCommand.Execute();
        }

        private void RemovePhoto_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemovePhotoCommand.Execute();
        }

        private void AddAccommodation_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddAccommodationCommand.Execute();
        }

        public void PreselectCountryAndCity(string preselectedCountry, string preselectedCity)
        {
            CountryComboBox.SelectedItem = preselectedCountry;
            CityComboBox.SelectedItem = preselectedCity;
        }
    }
}
