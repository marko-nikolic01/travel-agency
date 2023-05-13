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
using TravelAgency.Commands;
using TravelAgency.Domain.Models;
using TravelAgency.WPF.Commands;
using TravelAgency.WPF.Controls.CustomControls;
using TravelAgency.WPF.ViewModels;

namespace TravelAgency.WPF.Views
{
    /// <summary>
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window
    {
        public MyICommand<string> NavigateCommand { get; set; }
        public OwnerMainViewModel ViewModel { get; set; }

        public OwnerWindow()
        {
            NavigateCommand = new MyICommand<string>(OnNavigateCommandExecuted);
            InitializeComponent();
            ViewModel = new OwnerMainViewModel(NavigationFrame.NavigationService);
            DataContext = ViewModel;
        }

        private void OnNavigateCommandExecuted(string button)
        {
            switch (button)
            {
                case "profile":
                    myProfileRadioButton.IsChecked = true;
                    ViewModel.NavigateToMyProfilePageCommand.Execute(null);
                    break;
                case "accommodations":
                    accommodationsRadioButton.IsChecked = true;
                    ViewModel.NavigateToAccommodationsPageCommand.Execute(null);
                    break;
                case "reservations":
                    reservationsRadioButton.IsChecked = true;
                    ViewModel.NavigateToReservationsPageCommand.Execute(null);
                    break;
                default:
                    MessageBox.Show("Ne radi :(");
                    break;
            }
        }
    }
}
