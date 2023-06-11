using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Pages
{
    /// <summary>
    /// Interaction logic for OwnerAddAccommodationPage.xaml
    /// </summary>
    public partial class OwnerAddAccommodationView : Page
    {
        public MyICommand NextPhotoCommand { get; set; }
        public MyICommand PreviousPhotoCommand { get; set; }
        public OwnerAddAccommodationViewModel ViewModel { get; set; }

        public OwnerAddAccommodationView(OwnerAddAccommodationViewModel viewModel)
        {
            NextPhotoCommand = new MyICommand(Execute_NextPhotoCommand);
            PreviousPhotoCommand = new MyICommand(Execute_PreviousPhotoCommand);
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            Loaded += (s, e) => Keyboard.Focus(this);

            nameTextBox.Focus();
        }

        private void Execute_PreviousPhotoCommand()
        {
            photoViewer.Execute_PreviousPhotoCommand();
        }

        private void Execute_NextPhotoCommand()
        {
            photoViewer.Execute_NextPhotoCommand();
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
