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
        public MyICommand LogOutCommand { get; set; }
        public OwnerMainViewModel ViewModel { get; set; }

        public OwnerWindow()
        {
            NavigateCommand = new MyICommand<string>(OnNavigateCommandExecuted);
            LogOutCommand = new MyICommand(Execute_LogOutCommand);
            InitializeComponent();
            ViewModel = new OwnerMainViewModel(NavigationFrame.NavigationService);
            DataContext = ViewModel;

            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }

        private void Execute_LogOutCommand()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            Execute_LogOutCommand();
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
                case "ratings":
                    ratingsRadioButton.IsChecked = true;
                    ViewModel.NavigateToRatingsPageCommand.Execute(null);
                    break;
                case "forum":
                    forumRadioButton.IsChecked = true;
                    ViewModel.NavigateToForumPageCommand.Execute(null);
                    break;
                default:
                    MessageBox.Show("Ne radi :(");
                    break;
            }
        }
    }
}
